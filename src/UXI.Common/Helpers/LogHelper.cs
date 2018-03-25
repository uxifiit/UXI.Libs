using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Helpers
{
    public static class LogHelper
    {
        /// <summary>
        /// Prepares log message, omit caller parameter when calling to add it automatically.
        /// </summary>
        /// <param name="message">Logged message.</param>
        /// <param name="caller">Name of the caller of this method. Omit it and it will be added automatically.</param>
        /// <returns>Log message.</returns>
        public static string Prepare(string message = null, string arguments = null, [CallerMemberName] string caller = null)
        {
            return (caller ?? "") + "(" + arguments + "): " + (message ?? String.Empty);
        }


        public static string PrepareArguments(params ArgumentValue[] arguments)
        {
            return arguments?.Select(a => a.ToString()).Aggregate((a, b) => a + Environment.NewLine + b) ?? String.Empty;
        }
    }
}
