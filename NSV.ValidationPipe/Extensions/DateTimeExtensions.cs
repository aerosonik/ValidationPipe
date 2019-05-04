using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSV.ValidationPipe.Extensions
{
    public static class DateTimeExtensions
    {
        //Equal
        public static IFieldValidatorCreator<TModel, DateTime> Equal<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value, string message = null)
        {
            return creator.Must(x => x.Equals(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> Equal<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value)
        {
            return creator.Must(x => x.Equals(value));
        }
        //Greater
        public static IFieldValidatorCreator<TModel, DateTime> Greater<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value, string message = null)
        {
            return creator.Must(x => x > value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> Greater<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value)
        {
            return creator.Must(x => x > value);
        }
        //Greater Or Equal
        public static IFieldValidatorCreator<TModel, DateTime> GreaterOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value, string message = null)
        {
            return creator.Must(x => x >= value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> GreaterOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value)
        {
            return creator.Must(x => x >= value);
        }
        //Less
        public static IFieldValidatorCreator<TModel, DateTime> Less<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value, string message = null)
        {
            return creator.Must(x => x < value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> Less<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value)
        {
            return creator.Must(x => x < value);
        }
        //Less Or Equal
        public static IFieldValidatorCreator<TModel, DateTime> LessOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value, string message = null)
        {
            return creator.Must(x => x <= value).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> LessOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value)
        {
            return creator.Must(x => x <= value);
        }

        //Between
        public static IFieldValidatorCreator<TModel, DateTime> Between<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value1,
            DateTime value2,
            string message = null)
        {
            return creator.Must(x => x > value1 && x < value2).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> Between<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value1,
            DateTime value2)
        {
            return creator.Must(x => x > value1 && x < value2);
        }
        //Between or Equal
        public static IFieldValidatorCreator<TModel, DateTime> BetweenOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value1,
            DateTime value2,
            string message = null)
        {
            return creator.Must(x => x >= value1 && x <= value2).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> BetweenOrEqual<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            DateTime value1,
            DateTime value2)
        {
            return creator.Must(x => x >= value1 && x <= value2);
        }

        //In
        public static IFieldValidatorCreator<TModel, DateTime> In<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            IEnumerable<DateTime> values,
            string message = null)
        {
            return creator.Must(x => values.Contains(x)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, DateTime> In<TModel>(
            this IFieldValidatorCreator<TModel, DateTime> creator,
            IEnumerable<DateTime> values)
        {
            return creator.Must(x => values.Contains(x));
        }
    }
}
