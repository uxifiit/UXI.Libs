using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Extensions
{
    public static class AsyncEx
    {
        public static async void Forget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }


        public static async void Forget(this Task task, Action<Exception> exceptionHandler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                exceptionHandler.Invoke(ex);
            }
        }


        public static async Task AwaitOrForget(this Task task, bool shouldAwait)
        {
            if (shouldAwait)
            {
                await task;
            }
            else
            {
                task.Forget();
            }
        }
    }
}
