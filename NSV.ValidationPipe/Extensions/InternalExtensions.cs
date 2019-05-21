using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    internal static class InternalExtensions
    {
        internal static ValidateResultWrapper UnPack(this ValidateResultWrapper[] results)
        {
            var validationResults = results
                 .Where(x => x.IsSingleResult)
                 .Select(x => x.Result.Value)
                 .Concat(results.Where(x => x.IsSetOfResults).SelectMany(x => x.Results.Value))
                 .ToArray();

            return ValidateResultWrapper.Create(validationResults);
        }
    }
}
