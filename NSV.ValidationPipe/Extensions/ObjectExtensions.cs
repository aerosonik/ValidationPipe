using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NSV.ValidationPipe.Extensions
{
    public static class ObjectExtensions
    {
        //NotNull
        public static IFieldValidatorCreator<TModel, TField> NotNull<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField> creator, 
            string message = null)
        {
            return creator.Must(x => x != null).WithMessage(message);
        }
        public static IValidatorCreator<TModel, TField> NotNull<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField> creator)
        {
            return creator.Must(x => x != null);
        }
    }
}
