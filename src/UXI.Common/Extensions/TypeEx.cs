using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Extensions
{
    public static class TypeEx
    {
        /// <summary>
        /// Returns default value for the type passed through parameter. The default value is retrieved using reflection to call generic default(T) of the type.
        /// </summary>
        /// <param name="type">Type which to get default value for.</param>
        /// <returns>Default value</returns>
        public static object GetDefault(this Type type)
        {
            return typeof(TypeEx).GetMethod(nameof(GetDefaultGeneric), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                                 .MakeGenericMethod(type)
                                 .Invoke(null, null);
        }

        private static T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}
