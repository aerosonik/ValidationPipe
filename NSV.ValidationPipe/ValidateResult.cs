using NSV.ExecutionPipe;
using NSV.ExecutionPipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe
{
    public struct ValidateResult
    {
        public ExecutionResult Success { get; set; }
        public string FieldPath { get; set; }
        public string ErrorMessage { get; set; }
        public int Code { get; set; }

        //public Optional<ValidateResult[]> SubResults { get; set; }

        public static ValidateResult Default
        {
            get
            {
                return new ValidateResult
                {
                    Success = ExecutionResult.Initial,
                    //SubResults = Optional<ValidateResult[]>.Default
                };
            }
        }

        public static ValidateResult DefaultValid
        {
            get
            {
                return new ValidateResult
                {
                    Success = ExecutionResult.Successful,
                    //SubResults = Optional<ValidateResult[]>.Default
                };
            }
        }

        public static ValidateResult DefaultFailed
        {
            get
            {
                return new ValidateResult
                {
                    Success = ExecutionResult.Failed,
                    //SubResults = Optional<ValidateResult[]>.Default
                };
            }
        }

        public bool IsFailed
        {
            get
            {
                return Success == ExecutionResult.Failed;
            }
        }

        public bool IsValid
        {
            get
            {
                return Success == ExecutionResult.Successful;
            }
        }

        public ValidateResult SetErrorMessage(string message)
        {
            ErrorMessage = message;
            return this;
        }

        public ValidateResult SetPath(string path)
        {
            FieldPath = path;
            return this;
        }

        public ValidateResult SetCode(int code)
        {
            Code = code;
            return this;
        }
    }

    public struct ValidateResultWrapper
    {
        public ValidateResultWrapper(ValidateResult[] results)
        {
            Results = results;
            Result = Optional<ValidateResult>.Default;
        }

        public ValidateResultWrapper(ValidateResult result)
        {
            Results = Optional<ValidateResult[]>.Default;
            Result = result;
        }

        public Optional<ValidateResult[]> Results { get; }
        public Optional<ValidateResult> Result { get; }

        public bool IsSingleResult
        {
            get { return Result.HasValue; }
        }

        public bool IsSetOfResults
        {
            get { return Results.HasValue; }
        }

        public static ValidateResultWrapper Create(ValidateResult[] results)
        {
            return new ValidateResultWrapper(results);
        }

        public static ValidateResultWrapper Create(ValidateResult result)
        {
            return new ValidateResultWrapper(result);
        }
    }

    public static class ValidateResultExtensions
    {
        public static bool IsAllValid(this ValidateResult[] results)
        {
            return results.All(x => x.IsValid);
        }

        public static bool IsAnyFailed(this ValidateResult[] results)
        {
            return results.Any(x => x.IsFailed);
        }
    }
}
