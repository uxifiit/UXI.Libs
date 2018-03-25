using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.System.Hooks
{
    public abstract class HookBase
    {
        /// <summary>
        /// The CallWndProc hook procedure is an application-defined or library-defined callback 
        /// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer 
        /// to this callback function. CallWndProc is a placeholder for the application-defined 
        /// or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
        /// </remarks>
        public delegate int HookCallbackDelegate(int nCode, int wParam, IntPtr lParam);
        

        /// <summary>
        /// Stores the handle to the Keyboard or Mouse hook procedure.
        /// </summary>
        protected int HookHandle { get; set; }

        /// <summary>
        /// Keeps the reference to prevent garbage collection of delegate. See: CallbackOnCollectedDelegate http://msdn.microsoft.com/en-us/library/43yky316(v=VS.100).aspx
        /// </summary>
        protected HookCallbackDelegate HookCallbackReferenceKeeper { get; set; }

        /// <summary>
        /// Override this method to modify logic of firing events.
        /// </summary>
        protected abstract bool ProcessCallback(int wParam, IntPtr lParam);

        /// <summary>
        /// A callback function which will be called every time a keyboard or mouse activity detected.
        /// <see cref="WinApi.HookCallback"/>
        /// </summary>
        protected int HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                bool shouldProcess = ProcessCallback(wParam, lParam);

                if (!shouldProcess)
                {
                    return -1;
                }
            }

            return CallNextHook(nCode, wParam, lParam);
        }

        private int CallNextHook(int nCode, int wParam, IntPtr lParam)
        {
            return WinApi.CallNextHookEx(HookHandle, nCode, wParam, lParam);
        }

        internal int Subscribe(int hookId, HookCallbackDelegate hookCallback)
        {
            int hookHandle = WinApi.SetWindowsHookEx(
                hookId,
                hookCallback,
                Process.GetCurrentProcess().MainModule.BaseAddress,
                0);

            if (hookHandle == 0)
            {
                throw new Exception();
            }

            return hookHandle;
        }

        internal void Unsubscribe(int handle)
        {
            int result = WinApi.UnhookWindowsHookEx(handle);
        }

        /// <summary>
        /// Subscribes to the hook and starts firing events.
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public void Start()
        {
            if (IsEnabled == false)
            {

                HookCallbackReferenceKeeper = new HookCallbackDelegate(HookCallback);
                try
                {
                    HookHandle = Subscribe(GetHookId(), HookCallbackReferenceKeeper);
                }
                catch (Exception)
                {
                    HookCallbackReferenceKeeper = null;
                    HookHandle = 0;
                    throw;
                }
            }
        }

        /// <summary>
        /// Unsubscribes from the hook and stops firing events.
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public void Stop()
        {
            if (IsEnabled)
            {
                try
                {
                    Unsubscribe(HookHandle);
                }
                finally
                {
                    HookCallbackReferenceKeeper = null;
                    HookHandle = 0;
                }
            }
        }

        /// <summary>
        /// Override to deliver correct id to be used for <see cref="HookNativeMethods.SetWindowsHookEx"/> call.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetHookId();

        /// <summary>
        /// Gets or Sets the enabled status of the Hook.
        /// </summary>
        /// <value>
        /// True - The Hook is presently installed, activated, and will fire events.
        /// <para>
        /// False - The Hook is not part of the hook chain, and will not fire events.
        /// </para>
        /// </value>
        public bool IsEnabled
        {
            get { return HookHandle != 0; }
        }

        /// <summary>
        /// Release delegates, unsubscribes from hooks.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method to be used from Dispose and Finalizer.
        /// Override this method to release subclass specific references.
        /// </summary>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Stop();
            }
            else
            {
                if (HookHandle != 0)
                {
                    WinApi.UnhookWindowsHookEx(HookHandle);
                }
            }
        }

        /// <summary>
        /// Unsubscribes from global hooks skipping error handling.
        /// </summary>
        ~HookBase()
        {
            Dispose(false);
        }
    }
}
