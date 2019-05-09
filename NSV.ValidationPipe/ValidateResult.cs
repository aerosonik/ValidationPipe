using NSV.ExecutionPipe;
using NSV.ExecutionPipe.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSV.ValidationPipe
{
    public struct ValidateResult
    {
        public ExecutionResult Success { get; set; }
        public string FieldPath { get; set; }
        public string ErrorMessage { get; set; }
        public Optional<ValidateResult[]> SubResults { get; set; }

        public static ValidateResult Default
        {
            get
            {
                return new ValidateResult
                {
                    Success = ExecutionResult.Initial,
                    SubResults = Optional<ValidateResult[]>.Default
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
                    SubResults = Optional<ValidateResult[]>.Default
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
                    SubResults = Optional<ValidateResult[]>.Default
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
    }
}
