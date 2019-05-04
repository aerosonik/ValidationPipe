using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    public static class ArrayExtensions
    {
        //NotEmpty
        public static IFieldValidatorCreator<TModel, TField[]> NotEmpty<TModel, TField>(
           this IFieldValidatorCreator<TModel, TField[]> creator,
           string message = null)
        {
            return creator.Must(x => x != null && x.Any()).WithMessage(message);
        }
        public static IValidatorCreator<TModel, TField[]> NotEmpty<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator)
        {
            return creator.Must(x => x != null && x.Any());
        }

        //Any
        public static IFieldValidatorCreator<TModel, TField[]> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.Any(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, TField[]> Any<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.Any(condition));
        }

        //All
        public static IFieldValidatorCreator<TModel, TField[]> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            Func<TField, bool> condition,
            string message = null)
        {
            return creator.Must(x => x.All(condition)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, TField[]> All<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            Func<TField, bool> condition)
        {
            return creator.Must(x => x.All(condition));
        }

        //Contains
        public static IFieldValidatorCreator<TModel, TField[]> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            TField value,
            string message = null)
        {
            return creator.Must(x => x.Contains(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, TField[]> Contains<TModel, TField>(
            this IFieldValidatorCreator<TModel, TField[]> creator,
            TField value)
        {
            return creator.Must(x => x.Contains(value));
        }
    }
}
