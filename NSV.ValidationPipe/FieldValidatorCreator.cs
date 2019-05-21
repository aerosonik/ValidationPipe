using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NSV.ExecutionPipe;
using NSV.ExecutionPipe.Models;
using NSV.ValidationPipe.Extensions;

namespace NSV.ValidationPipe
{
    internal class FieldValidatorCreator<TModel, TField> :
        IFieldValidatorCreator<TModel, TField>,
        IValidatorCreator<TModel, TField>,
        IFieldValidatorExecutor<TModel>
    {
        private readonly Optional<Func<TModel, bool>[]> _ifConditions = 
            Optional<Func<TModel, bool>[]>.Default;
        private readonly Expression<Func<TModel, TField>> _field;
        private readonly Expression<Func<TModel, IEnumerable<TField>>> _collectionField;
        private readonly PipeValidator<TModel> _pipe;
        private readonly bool _isCollectionField = false;
        private bool _asParallel = false;
        private Func<TModel, bool> _when = (_) => true;
        private string _path = string.Empty;

        private List<ValidatorStruture<TField>> _queue;
        private ValidatorStruture<TField> _current;

        internal FieldValidatorCreator(
            PipeValidator<TModel> pipe,
            Expression<Func<TModel, TField>> field,
            params Func<TModel, bool>[] ifConditions) 
            : this(pipe, ifConditions)
        {
            _field = field;
        }
        internal FieldValidatorCreator(
            PipeValidator<TModel> pipe,
            Expression<Func<TModel, IEnumerable<TField>>> field,
            params Func<TModel, bool>[] ifConditions) 
            : this(pipe, ifConditions)
        {
            _collectionField = field;
            _isCollectionField = true;
        }
        private FieldValidatorCreator(
            PipeValidator<TModel> pipe, 
            params Func<TModel, bool>[] ifConditions)
        {
            _pipe = pipe;
            _current = new ValidatorStruture<TField>();
            _ifConditions = ifConditions;
        }

        public IPipeValidator<TModel> Add()
        {
            if (_queue != null && _current.StructureType != ValidatorStrutureType.Default)
            {
                _queue.Add(_current);
                _current = new ValidatorStruture<TField>();
            }
            _pipe.AddFieldValidator(this);
            return _pipe;
        }

        //take effect only if collection
        /// <summary>
        /// If Field is collection, call AsParallel() will 
        /// execute validation for each element ofcollection as parallel.
        /// If Field is NOT collection, call of this method will ignored
        /// </summary>
        /// <returns></returns>
        public IFieldValidatorCreator<TModel, TField> AsParallel()
        {
            _asParallel = true;
            return this;
        }

        public IValidatorCreator<TModel, TField> Must(Func<TField, bool> must)
        {
            CheckQueueAndCurrentValidator();
            _current = new ValidatorStruture<TField>(must);
            return this;
        }

        public IValidatorCreator<TModel, TField> Must(Func<TField, Task<bool>> must)
        {
            CheckQueueAndCurrentValidator();
            _current = new ValidatorStruture<TField>(must);
            return this;
        }

        IFieldValidatorCreator<TModel, TField> IValidatorCreator<TModel, TField>.WithMessage(
            string message)
        {
            _current.Message = message;
            return this;
        }

        public IFieldValidatorCreator<TModel, TField> Path(string path)
        {
            _path = path;
            return this;
        }

        public IFieldValidatorCreator<TModel, TField> Set(IValidator<TField> validator)
        {
            CheckQueueAndCurrentValidator();
            _current = new ValidatorStruture<TField>(validator);
            return this;
        }

        public IFieldValidatorCreator<TModel, TField> Set(IValidatorAsync<TField> validatorAsync)
        {
            CheckQueueAndCurrentValidator();
            if (_isCollectionField)
                validatorAsync.IsForCollection = true;

            _current = new ValidatorStruture<TField>(validatorAsync);
            return this;
        }

        public IFieldValidatorCreator<TModel, TField> When(Func<TModel, bool> condition)
        {
            _when = condition;
            return this;
        }

        async Task<ValidateResultWrapper> IFieldValidatorExecutor<TModel>.ExecuteValidationAsync(
            TModel model)
        {
            if (!CheckIfConditions(model))
                return ValidateResultWrapper.Create(ValidateResult.Default);

            if (_isCollectionField)
            {
                int index = 0;
                var fields = _collectionField
                    .Compile()
                    .Invoke(model)
                    .Select(x => (field : x, index: index++))
                    .ToArray();

                Task<ValidateResultWrapper>[] tasks = null;
                if (_asParallel)
                {
                    tasks = fields
                        .AsParallel()
                        .Select(async x => 
                            await InvokeValidationForField(x.field, x.index))
                        .ToArray();
                }
                else
                {
                    tasks = fields
                        .Select(async x => 
                            await InvokeValidationForField(x.field, x.index))
                        .ToArray();
                }
                var results = await Task.WhenAll(tasks);
                return results.UnPack();
            }
            else
            {
                TField field = _field.Compile().Invoke(model);
                return await InvokeValidationForField(field);
            }
        }

        private async Task<ValidateResultWrapper> InvokeValidationForField(
            TField field,
            int? index = null)
        {
            var indexstr = index == null ? string.Empty : $"[{index}]";
            if (_queue != null && _queue.Count > 0)
            {
                return await InvokeValidationForQueue(field, indexstr);
            }
            else
            {
                return await InvokeValidationForCurrent(field, indexstr);
            }
        }

        private async Task<ValidateResultWrapper> InvokeValidationForQueue(TField field, string index)
        {
            var path = _path + index;
            var tasks = _queue
                .Where(x => x.IsAsync)
                .Select(async x =>
                {
                    if (x.StructureType == ValidatorStrutureType.FuncAsync)
                    {
                        var asyncResult = await x.MustAsync(field)
                            ? ValidateResult.DefaultValid
                            : ValidateResult.DefaultFailed
                                .SetErrorMessage(x.Message)
                                .SetPath(path);
                        return ValidateResultWrapper.Create(asyncResult);
                    }
                    else
                    {
                        var asyncResultWrapper = await x.ValidatorAsync.ValidateAsync(field);
                        if(asyncResultWrapper.IsSingleResult && asyncResultWrapper.Result.Value.IsFailed)
                            asyncResultWrapper.Result.Value.SetPath(path);
                        return asyncResultWrapper;
                    }
                }).ToArray();

            var results = _queue
                .Where(x => !x.IsAsync)
                .Select(x =>
                {
                    if (x.StructureType == ValidatorStrutureType.Func)
                    {
                        var result = x.Must(field)
                            ? ValidateResult.DefaultValid
                            : ValidateResult.DefaultFailed
                                .SetErrorMessage(x.Message)
                                .SetPath(path);
                        return ValidateResultWrapper.Create(result);
                    }
                    var resultWrapper = x.Validator.Validate(field);
                    if (resultWrapper.IsSingleResult && resultWrapper.Result.Value.IsFailed)
                        resultWrapper.Result.Value.SetPath(path);
                    return resultWrapper;
                }).ToArray();

            return results
                .Concat(await Task.WhenAll(tasks))
                .ToArray()
                .UnPack();
        }

        private async Task<ValidateResultWrapper> InvokeValidationForCurrent(TField field, string index)
        {
            ValidateResult result = ValidateResult.DefaultValid;
            var path = _path + index;
            switch (_current.StructureType)
            {
                case ValidatorStrutureType.Func:
                    var rez = _current.Must(field);
                    if (!rez)
                    {
                        result = ValidateResult.DefaultFailed
                            .SetErrorMessage(_current.Message)
                            .SetPath(path);
                    }
                    break;

                case ValidatorStrutureType.FuncAsync:
                    var rez1 = await _current.MustAsync(field);
                    if (!rez1)
                    {
                        result = ValidateResult.DefaultFailed
                            .SetErrorMessage(_current.Message)
                            .SetPath(path);
                    }
                    break;

                case ValidatorStrutureType.Validator:
                    var wrappedResult = _current.Validator.Validate(field);
                    if (wrappedResult.IsSingleResult && wrappedResult.Result.Value.IsFailed)
                        wrappedResult.Result.Value.SetPath(path);

                    return wrappedResult;

                case ValidatorStrutureType.ValidatorAsync:
                    var asyncWrappedResult = await _current.ValidatorAsync.ValidateAsync(field);
                    if (asyncWrappedResult.IsSingleResult && asyncWrappedResult.Result.Value.IsFailed)
                        asyncWrappedResult.Result.Value.SetPath(path);

                    return asyncWrappedResult;
            }

            return ValidateResultWrapper.Create(result);
        }

        private void CheckQueueAndCurrentValidator()
        {
            if (_current.StructureType != ValidatorStrutureType.Default)
            {
                if (_queue == null)
                    _queue = new List<ValidatorStruture<TField>>();
                _queue.Add(_current);
            }
        }

        private bool CheckIfConditions(TModel model)
        {
            if (!_ifConditions.HasValue && _when(model))
                return true;

            if (!_ifConditions.Value.Any() && _when(model))
                return true;

            if (_ifConditions.Value.Select(x => x(model)).All(x => x) && _when(model))
                return true;

            return false;
        }
    }

    internal struct ValidatorStruture<TField>
    {
        public ValidatorStruture(
            ValidatorStrutureType validatorStrutType = ValidatorStrutureType.Default)
        {
            StructureType = validatorStrutType;
            Message = null;
            Must = null;
            MustAsync = null;
            Validator = null;
            ValidatorAsync = null;
            IsAsync = false;
        }
        public ValidatorStruture(
            Func<TField, bool> must)
        {
            StructureType = ValidatorStrutureType.Func;
            Must = must;
            Message = null;
            IsAsync = false;
            MustAsync = null;
            Validator = null;
            ValidatorAsync = null;
        }
        public ValidatorStruture(
            Func<TField, Task<bool>> mustAsync)
        {
            StructureType = ValidatorStrutureType.FuncAsync;
            MustAsync = mustAsync;
            Message = null;
            IsAsync = true;
            Must = null;
            Validator = null;
            ValidatorAsync = null;
        }
        public ValidatorStruture(IValidator<TField> validator)
        {
            StructureType = ValidatorStrutureType.Validator;
            Validator = validator;
            IsAsync = false;
            Message = null;
            Must = null;
            MustAsync = null;
            ValidatorAsync = null;
        }
        public ValidatorStruture(IValidatorAsync<TField> validatorAsync)
        {
            StructureType = ValidatorStrutureType.ValidatorAsync;
            ValidatorAsync = validatorAsync;
            IsAsync = true;
            Message = null;
            Must = null;
            MustAsync = null;
            Validator = null;
        }
        public bool IsAsync { get; set; }
        public ValidatorStrutureType StructureType { get; }
        public string Message { get; set; }
        public Func<TField, bool> Must { get; }
        public Func<TField, Task<bool>> MustAsync { get; }
        public IValidator<TField> Validator { get; }
        public IValidatorAsync<TField> ValidatorAsync { get; }
    }

    internal enum ValidatorStrutureType
    {
        Default,
        Func,
        FuncAsync,
        Validator,
        ValidatorAsync
    }
}
