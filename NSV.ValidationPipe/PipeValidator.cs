using NSV.ExecutionPipe;
using NSV.ExecutionPipe.Models;
using NSV.ExecutionPipe.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSV.ValidationPipe
{
    public abstract class PipeValidator<TModel> : IPipeValidator<TModel>
    {
        #region Private Fields
        private bool _asParallel = false;
        private Queue<IFieldValidatorExecutor<TModel>> _validators;
        private Optional<ILocalCache> _externalCache = Optional<ILocalCache>.Default;
        private Optional<IDictionary<object, object>> _localCache = Optional<IDictionary<object, object>>.Default;
        private Optional<Stack<Func<TModel, bool>>> _ifConditionStack = Optional<Stack<Func<TModel, bool>>>.Default;
        #endregion

        #region IPipeValidator<TModel>
        public IPipeValidator<TModel> AsParallel()
        {
            _asParallel = true;
            return this;
        }

        public IFieldValidatorCreator<TModel, TField> For<TField>(
            Expression<Func<TModel, TField>> field)
        {
            if (!_ifConditionStack.HasValue || !_ifConditionStack.Value.Any())
                return new FieldValidatorCreator<TModel, TField>(this, field);

            return new FieldValidatorCreator<TModel, TField>(
                this,
                field,
                _ifConditionStack.Value.ToArray());
        }

        public IFieldValidatorCreator<TModel, TField> ForCollection<TField>(
            Expression<Func<TModel, IEnumerable<TField>>> field)
        {
            if (!_ifConditionStack.HasValue || !_ifConditionStack.Value.Any())
                return new FieldValidatorCreator<TModel, TField>(this, field);

            return new FieldValidatorCreator<TModel, TField>(
                this, 
                field, 
                _ifConditionStack.Value.ToArray());
        }

        public IPipeValidator<TModel> If(Func<TModel, bool> condition)
        {
            if (condition == null)
                return this;

            if (!_ifConditionStack.HasValue)
                _ifConditionStack = new Stack<Func<TModel, bool>>();

            _ifConditionStack.Value.Push(condition);
            return this;
        }
        public IPipeValidator<TModel> EndIf()
        {
            if (!_ifConditionStack.HasValue || !_ifConditionStack.Value.Any())
                throw new Exception("Redundant EndIf");

            _ifConditionStack.Value.Pop();
            return this;
        }

        public IPipeValidator<TModel> EndAllIf()
        {
            if (!_ifConditionStack.HasValue || !_ifConditionStack.Value.Any())
                throw new Exception("Redundant EndAllIf");

            _ifConditionStack.Value.Clear();
            return this;
        }

        public IPipeValidator<TModel> ImportLocalCache(ILocalCache cache)
        {
            CheckCacheObject();

            _externalCache = new Optional<ILocalCache>(cache);
            return this;
        }

        public IPipeValidator<TModel> UseLocalCacheThreadSafe()
        {
            CheckCacheObject();

            _localCache = new ConcurrentDictionary<object, object>();
            return this;
        }

        public IPipeValidator<TModel> UseLocalCache()
        {
            CheckCacheObject();

            _localCache = new Dictionary<object, object>();
            return this;
        }

        public async Task<ValidateResult[]> ExecuteAsync(TModel model)
        {
            if (_ifConditionStack.HasValue && _ifConditionStack.Value.Any())
                throw new Exception("Expected EndIf or EndAllIf operators");
            if (_ifConditionStack.HasValue)
                _ifConditionStack = Optional<Stack<Func<TModel, bool>>>.Default;

            if (_asParallel)
            {
                var resultTasks = _validators
                    .AsParallel()
                    .Select(async x => await x.ExecuteValidationAsync(model))
                    .ToArray();
                var taskResults = await Task.WhenAll(resultTasks);
                return taskResults
                    .Where(x => x.IsSingleResult)
                    .Select(x => x.Result.Value)
                    .Concat(taskResults.Where(x => x.IsSetOfResults).SelectMany(x => x.Results.Value))
                    .ToArray();
            }
            else
            {
                var resultList = new List<ValidateResult>();
                if (!IsForCollection)
                {
                    while (_validators.Count > 0)
                    {
                        await HandleExecuteValidation(_validators.Dequeue(), model, resultList);
                    }
                }
                else
                {
                    foreach(var item in _validators)
                    {
                        await HandleExecuteValidation(item, model, resultList);
                    }
                }
                return resultList.ToArray();
            }
        }
        #endregion

        #region IValidatorAsync<TField>

        public bool IsForCollection { get; set; }
        public async Task<ValidateResultWrapper> ValidateAsync(TModel model)
        {
            return ValidateResultWrapper.Create(await ExecuteAsync(model));
        }
        #endregion

        #region ILocalCache
        public T GetObject<T>(object key)
        {
            if (!_localCache.HasValue && _externalCache.HasValue)
                return _externalCache.Value.GetObject<T>(key);

            if (_localCache.HasValue && !_externalCache.HasValue)
                if (_localCache.Value.TryGetValue(key, out var value))
                    return (T)value;

            return default(T);
        }

        public void SetObject<T>(object key, T value)
        {
            if (!_localCache.HasValue && _externalCache.HasValue)
            {
                _externalCache.Value.SetObject(key, value);
                return;
            }
            if (_localCache.HasValue && !_externalCache.HasValue)
            {
                _localCache.Value.Add(key, value);
                return;
            }
        }

        public ILocalCache GetLocalCacheObject()
        {
            return this;
        }
        #endregion

        #region Private methods
        private void CheckCacheObject()
        {
            if (_externalCache.HasValue)
                throw new Exception("External cache already in use");

            if (_localCache.HasValue)
                throw new Exception("local cache already in use");
        }
        #endregion

        internal void AddFieldValidator(IFieldValidatorExecutor<TModel> validator)
        {
            if (_validators == null)
                _validators = new Queue<IFieldValidatorExecutor<TModel>>();

            _validators.Enqueue(validator);
        }

        private async Task HandleExecuteValidation(
            IFieldValidatorExecutor<TModel> item, 
            TModel model, 
            List<ValidateResult> list)
        {
            var fieldResult = await item.ExecuteValidationAsync(model);
            if (fieldResult.IsSingleResult)
                list.Add(fieldResult.Result.Value);
            else
                list.AddRange(fieldResult.Results.Value);
        }

    }
}
