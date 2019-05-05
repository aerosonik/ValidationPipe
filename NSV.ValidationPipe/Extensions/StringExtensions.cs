using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NSV.ValidationPipe.Extensions
{
    public static class StringExtensions
    {
        private static string GuidPattern = @"^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$";
        //Equal
        public static IFieldValidatorCreator<TModel, string> Equal<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value, 
            string message = null)
        {
            return creator.Must(x => x.Equals(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> Equal<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value)
        {
            return creator.Must(x => x.Equals(value));
        }
        //NotEqual
        public static IFieldValidatorCreator<TModel, string> NotEqual<TModel>(
           this IFieldValidatorCreator<TModel, string> creator,
           string value, 
           string message = null)
        {
            return creator.Must(x => !x.Equals(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> NotEqual<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value)
        {
            return creator.Must(x => !x.Equals(value));
        }
        //Contains
        public static IFieldValidatorCreator<TModel, string> Contains<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value, 
            string message = null)
        {
            return creator.Must(x => x.Contains(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> Contains<TModel>(
           this IFieldValidatorCreator<TModel, string> creator,
           string value)
        {
            return creator.Must(x => x.Contains(value));
        }
        //StartWith
        public static IFieldValidatorCreator<TModel, string> StartWith<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value, 
            string message = null)
        {
            return creator.Must(x => x.StartsWith(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> StartWith<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value)
        {
            return creator.Must(x => x.StartsWith(value));
        }
        //EndWith
        public static IFieldValidatorCreator<TModel, string> EndWith<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string value, string message = null)
        {
            return creator.Must(x => x.EndsWith(value)).WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> EndWith<TModel>(
           this IFieldValidatorCreator<TModel, string> creator,
           string value)
        {
            return creator.Must(x => x.EndsWith(value));
        }
        //Regexp
        public static IFieldValidatorCreator<TModel, string> IsMatch<TModel>(
            this IFieldValidatorCreator<TModel, string> creator,
            string pattern, 
            string message = null)
        {
            return creator.Must(x => Regex.IsMatch(x, pattern, RegexOptions.CultureInvariant))
                          .WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> IsMatch<TModel>(
           this IFieldValidatorCreator<TModel, string> creator,
           string pattern)
        {
            return creator.Must(x => Regex.IsMatch(x, pattern, RegexOptions.CultureInvariant));
        }
        public static IValidatorCreator<TModel, string> IsGuid<TModel>(
           this IFieldValidatorCreator<TModel, string> creator)
        {
            return creator.IsMatch(GuidPattern);
        }
        public static IFieldValidatorCreator<TModel, string> IsGuid<TModel>(
           this IFieldValidatorCreator<TModel, string> creator,
           string message = null)
        {
            return creator.IsMatch(GuidPattern).WithMessage(message);
        }
        //IsEmpty
        public static IFieldValidatorCreator<TModel, string> IsEmpty<TModel>(
            this IFieldValidatorCreator<TModel, string> creator, string message = null)
        {
            return creator.Must(x => string.IsNullOrWhiteSpace(x))
                          .WithMessage(message);
        }
        public static IValidatorCreator<TModel, string> IsEmpty<TModel>(
           this IFieldValidatorCreator<TModel, string> creator)
        {
            return creator.Must(x => string.IsNullOrWhiteSpace(x));
        }
        //NotEmpty
        /// <summary>
        /// Not null, not empty, not white space
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="creator"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IFieldValidatorCreator<TModel, string> NotEmpty<TModel>(
            this IFieldValidatorCreator<TModel, string> creator, string message = null)
        {
            return creator.Must(x => !string.IsNullOrWhiteSpace(x))
                          .WithMessage(message);
        }
        /// <summary>
        /// Not null, not empty, not white space
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static IValidatorCreator<TModel, string> NotEmpty<TModel>(
           this IFieldValidatorCreator<TModel, string> creator)
        {
            return creator.Must(x => !string.IsNullOrWhiteSpace(x));
        }
    }
}
