using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    public static class ListExtensions
    {
        //NotEmpty
        public static IFieldValidatorCreator<TModel, List<TField>> NotEmpty<TModel, TField>(
           this IFieldValidatorCreator<TModel, List<TField>> creator,
           string message = null)
        {
            return creator.Must(x => x != null && x.Any()).WithMessage(message);
        }
        public static IValidatorCreator<TModel, List<TField>> NotEmpty<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator)
        {
            return creator.Must(x => x != null && x.Any());
        }

        //Any
        public static IFieldValidatorCreator<TModel, List<TField>> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.Any(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, List<TField>> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.Any(condition));
        }

        //All
        public static IFieldValidatorCreator<TModel, List<TField>> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.All(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, List<TField>> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.All(condition));
        }

        //Contains
        public static IFieldValidatorCreator<TModel, List<TField>> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            TField value,
            string message = null)
        {
            return creator.Must(x => x.Contains(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, List<TField>> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, List<TField>> creator,
            TField value)
        {
            return creator.Must(x => x.Contains(value));
        }
    }
}
