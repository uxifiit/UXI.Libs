using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXI.Common.Helpers
{
    public static class AsyncHelper
    {
        public static async Task<TResult> InvokeAsync<TEventHandler, TResult>(Action action, Action<TEventHandler> addHandler, Action<TEventHandler> removeHandler, Func<TaskCompletionSource<TResult>, TEventHandler> createHandler, CancellationToken cancellationToken)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();  // TODO ? TaskCreationOptions.AttachedToParent

            cancellationToken.Register(() => tcs.TrySetCanceled());

            TEventHandler handler = createHandler.Invoke(tcs);

            addHandler(handler);

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                action.Invoke();

                return await tcs.Task;
            }
            finally
            {
                removeHandler(handler);
            }
        }

        public static Task<TResult> InvokeAsync<TEventHandler, TResult>(Action action, Action<TEventHandler> addHandler, Action<TEventHandler> removeHandler, Func<TaskCompletionSource<TResult>, TEventHandler> createHandler)
        {
            return InvokeAsync<TEventHandler, TResult>(action, addHandler, removeHandler, createHandler, CancellationToken.None);
        }

        public static Task InvokeAsync<TEventHandler>(Action action, Action<TEventHandler> addHandler, Action<TEventHandler> removeHandler, Func<TaskCompletionSource<bool>, TEventHandler> createHandler)
        {
            return InvokeAsync<TEventHandler, bool>(action, addHandler, removeHandler, createHandler, CancellationToken.None);
        }

        public static Task InvokeAsync<TEventHandler>(Action action, Action<TEventHandler> addHandler, Action<TEventHandler> removeHandler, Func<TaskCompletionSource<bool>, TEventHandler> createHandler, CancellationToken cancellationToken)
        {
            return InvokeAsync<TEventHandler, bool>(action, addHandler, removeHandler, createHandler, cancellationToken);
        }


        public static async Task<TResult> InvokeAsync<TCallback, TResult>(Action<TCallback> action, Func<TaskCompletionSource<TResult>, TCallback> createCallback, CancellationToken cancellationToken)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();  // TODO ? TaskCreationOptions.AttachedToParent

            TCallback callback = createCallback.Invoke(tcs);

            cancellationToken.ThrowIfCancellationRequested();

            cancellationToken.Register(() => tcs.TrySetCanceled());

            action.Invoke(callback);

            return await tcs.Task;
        }

        public static Task<TResult> InvokeAsync<TCallback, TResult>(Action<TCallback> action, Func<TaskCompletionSource<TResult>, TCallback> createCallback)
        {
            return InvokeAsync<TCallback, TResult>(action, createCallback, CancellationToken.None);
        }

        public static Task InvokeAsync<TCallback>(Action<TCallback> action, Func<TaskCompletionSource<bool>, TCallback> createCallback, CancellationToken cancellationToken)
        {
            return InvokeAsync<TCallback, bool>(action, createCallback, cancellationToken);
        }

        public static Task InvokeAsync<TCallback>(Action<TCallback> action, Func<TaskCompletionSource<bool>, TCallback> createCallback)
        {
            return InvokeAsync<TCallback, bool>(action, createCallback, CancellationToken.None);
        }
    }
}
