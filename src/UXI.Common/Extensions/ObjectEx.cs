using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Extensions
{
    public static class ObjectEx
    { 
        /// <summary>
        /// Gets the original value stored in the referenced storage, sets it to the new value and returns the original value.
        /// </summary>
        /// <typeparam name="TValue">Type of the value to be returned and replaced.</typeparam>
        /// <param name="storage">Source of the value to get and replace, i.e., local variable or field.</param>
        /// <param name="newValue">New value that is set to the storage.</param>
        /// <returns>Value stored in the storage before it was replaced by this method.</returns>
        public static TValue GetAndReplace<TValue>(ref TValue storage, TValue newValue)
        {
            TValue value = storage;
            storage = newValue;
            return value;
        }

        private static object InvokeGenericMethod(object source, Type sourceType, string methodName, Type[] typeArgs, object[] parameters)
        {
            MethodInfo method = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                          .Where(m => m.Name == methodName)
                                          .Where(m => m.GetGenericArguments().Count() == typeArgs.Length)
                                          .FirstOrDefault()
                                          .ThrowIfNull(nameof(methodName));

            MethodInfo generic = method.MakeGenericMethod(typeArgs);

            return generic.Invoke(source, parameters);
        }


        //public static object InvokeGenericMethod(Type sourceType, string methodName, Type[] typeArgs, object[] parameters)
        //{
        //    return InvokeGenericMethod(null, sourceType, methodName, typeArgs, parameters); 
        //}

        //private static object InvokeGenericMethod(object source, string methodName, Type[] typeArgs, object[] parameters)
        //{
        //    return InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        //}


        public static void CallGenericMethod(this object source, string methodName, Type[] typeArgs, params object[] parameters)
        {
            InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        }


        public static void CallGenericExtensionMethod(this object target, Type methodSourceType, string methodName, Type[] typeArgs, params object[] parameters)
        {
            InvokeGenericMethod(null, methodSourceType, methodName, typeArgs, parameters?.Prepend(target).ToArray() ?? new object[] { target });
        }


        public static void CallGenericMethod(this object source, string methodName, Type typeArg, params object[] parameters)
        {
            CallGenericMethod(source, methodName, new Type[] { typeArg }, parameters);
        }                                                                      


        public static void CallGenericExtensionMethod(this object target, Type methodSourceType, string methodName, Type typeArg, params object[] parameters)
        {
            CallGenericExtensionMethod(target, methodSourceType, methodName, new Type[] { typeArg }, parameters);
        }


        public static TResult CallGenericMethod<TResult>(this object source, string methodName, Type[] typeArgs, params object[] parameters)
        {
            return (TResult)InvokeGenericMethod(source, source.GetType(), methodName, typeArgs, parameters);
        }

        public static TResult CallGenericExtensionMethod<TResult>(this object target, Type methodSourceType, string methodName, Type[] typeArgs, params object[] parameters)
        {
            return (TResult)InvokeGenericMethod(null, methodSourceType, methodName, typeArgs, parameters?.Prepend(target).ToArray() ?? new object[] { target });
        }


        public static TResult CallGenericExtensionMethod<TResult>(this object target, Type methodSourceType, string methodName, Type typeArg, params object[] parameters)
        {
            return CallGenericExtensionMethod<TResult>(target, methodSourceType, methodName, new Type[] { typeArg }, parameters);
        }


        public static TResult CallGenericMethod<TResult>(this object source, string methodName, Type typeArg, params object[] parameters)
        {
            return CallGenericMethod<TResult>(source, methodName, new Type[] { typeArg }, parameters);
        }
    }
}
