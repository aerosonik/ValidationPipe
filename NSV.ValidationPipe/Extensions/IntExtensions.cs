using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    public static class IntExtensions
    {
        //Equal
        public static IFieldValidatorCreator<TModel, int> Equal<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value, string message = null)
        {
            return creator.Must(x => x.Equals(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> Equal<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value)
        {
            return creator.Must(x => x.Equals(value));
        }

        // Greater
        public static IFieldValidatorCreator<TModel, int> Greater<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value, string message = null)
        {
            return creator.Must(x => x > value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> Greater<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value)
        {
            return creator.Must(x => x > value);
        }

        // GreaterOrEqual
        public static IFieldValidatorCreator<TModel, int> GreaterOrEqual<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value, string message = null)
        {
            return creator.Must(x => x >= value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> GreaterOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value)
        {
            return creator.Must(x => x >= value);
        }

        // less
        public static IFieldValidatorCreator<TModel, int> less<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value, string message = null)
        {
            return creator.Must(x => x < value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> less<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value)
        {
            return creator.Must(x => x < value);
        }

        // lessOrEqual
        public static IFieldValidatorCreator<TModel, int> lessOrEqual<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value, string message = null)
        {
            return creator.Must(x => x <= value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> lessOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value)
        {
            return creator.Must(x => x <= value);
        }
        // Between
        public static IFieldValidatorCreator<TModel, int> Between<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value1, int value2, string message = null)
        {
            return creator.Must(x => x > value1 && x < value2).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> Between<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value1, int value2)
        {
            return creator.Must(x => x > value1 && x < value2);
        }
        // Between or Equal
        public static IFieldValidatorCreator<TModel, int> BetweenOrEqual<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           int value1, int value2, string message = null)
        {
            return creator.Must(x => x >= value1 && x <= value2).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> BetweenOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            int value1, int value2)
        {
            return creator.Must(x => x >= value1 && x <= value2);
        }
        // In
        public static IFieldValidatorCreator<TModel, int> In<TModel>(
           this IFieldValidatorCreator<TModel, int> creator,
           IEnumerable<int> values, string message = null)
        {
            return creator.Must(x => values.Contains(x)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, int> In<TModel>(
            this IFieldValidatorCreator<TModel, int> creator,
            IEnumerable<int> values)
        {
            return creator.Must(x => values.Contains(x));
        }
    }
}
