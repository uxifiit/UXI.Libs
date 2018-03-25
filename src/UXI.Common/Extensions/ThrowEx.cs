using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Extensions
{
    public static class ThrowEx
    {
        public static TObject ThrowIfNull<TObject>(this TObject @object, string argumentName)
        {
            if ((object)@object == null)
            {
                throw new ArgumentNullException(argumentName);        
            }
            return @object;
        }


        public static TObject ThrowIfNull<TObject>(this TObject @object, Predicate<TObject> additionalCondition, string argumentName)
        {
            if ((object)@object == null || additionalCondition.Invoke(@object))
            {
                throw new ArgumentNullException(argumentName);
            }
            return @object;
        } 


        public static TObject ThrowIfNull<TObject, TException>(this TObject @object, Func<TException> exceptionFactory) 
            where TException : Exception
        {
            if ((object)@object == null)
            {
                throw exceptionFactory.ThrowIfNull(nameof(exceptionFactory)).Invoke();
            }
            return @object;
        }


        public static TObject ThrowIf<TObject>(this TObject @object, Predicate<TObject> condition, string argumentName, string message = null)
        {
            if (condition.Invoke(@object))
            {
                throw new ArgumentException(message ?? "Invalid argument passed.", argumentName);
            }
            return @object;
        }


        public static TObject ThrowIf<TObject, TException>(this TObject @object, Predicate<TObject> condition, Func<TException> exceptionFactory) 
            where TException : Exception
        {
            if (condition.Invoke(@object))
            {
                throw exceptionFactory.Invoke();
            }
            return @object;
        }
    }
}
