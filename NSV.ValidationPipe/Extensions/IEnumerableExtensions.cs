using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    public static class IEnumerableExtensions
    {
        //NotEmpty
        public static IFieldValidatorCreator<TModel, IEnumerable<TField>> NotEmpty<TModel, TField>(
           this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
           string message = null)
        {
            return creator.Must(x => x != null && x.Any()).WithMessage(message);
        }
        public static IValidatorCreator<TModel, IEnumerable<TField>> NotEmpty<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator)
        {
            return creator.Must(x => x != null && x.Any());
        }

        //Any
        public static IFieldValidatorCreator<TModel, IEnumerable<TField>> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.Any(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, IEnumerable<TField>> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.Any(condition));
        }

        //All
        public static IFieldValidatorCreator<TModel, IEnumerable<TField>> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.All(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, IEnumerable<TField>> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.All(condition));
        }

        //Contains
        public static IFieldValidatorCreator<TModel, IEnumerable<TField>> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            TField value,
            string message = null)
        {
            return creator.Must(x => x.Contains(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, IEnumerable<TField>> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, IEnumerable<TField>> creator,
            TField value)
        {
            return creator.Must(x => x.Contains(value));
        }
    }
}
