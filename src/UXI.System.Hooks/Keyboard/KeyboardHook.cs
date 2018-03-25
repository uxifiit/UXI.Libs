using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.System.Hooks.Keyboard
{
    public class KeyboardHook : HookBase
    {
        internal const int WH_KEYBOARD_LL = 13;

        protected override int GetHookId()
        {
            return WH_KEYBOARD_LL;
        }

        /// <summary>
        /// This method processes the data from the hook and initiates event firing.
        /// </summary>
        /// <param name="wParam">The first Windows Messages parameter.</param>
        /// <param name="lParam">The second Windows Messages parameter.</param>
        /// <returns>
        /// True - The hook will be passed along to other applications.
        /// <para>
        /// False - The hook will not be given to other applications, effectively blocking input.
        /// </para>
        /// </returns>
        protected override bool ProcessCallback(int wParam, IntPtr lParam)
        {
            KeyEventArgs e = KeyEventArgs.FromRawData(wParam, lParam, true);

            TryInvokeKeyDown(e);
            TryInvokeKeyPress(wParam, lParam);
            TryInvokeKeyUp(e);

            return true;
        }

        /// <summary>
        /// Occurs when a key is pressed. 
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDown;

        private bool TryInvokeKeyDown(KeyEventArgs e)
        {
            if (e.IsKeyDown)
            {
                KeyDown?.Invoke(this, e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        ///set the <see cref="KeyPressEventArgs.Handled"/> property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public event EventHandler<KeyPressEventArgs> KeyPress;


        private bool TryInvokeKeyPress(int wParam, IntPtr lParam)
        {
            var e = KeyPressEventArgs.FromRawDataGlobal(wParam, lParam);
            if (e.IsNonChar)
            {
                KeyPress?.Invoke(this, e);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        private bool TryInvokeKeyUp(KeyEventArgs e)
        {
            if (e.IsKeyUp)
            {
                KeyUp?.Invoke(this, e);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to be used from <see cref="Dispose"/> and Finalizer.
        /// Override this method to release subclass specific references.
        /// </summary>
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                KeyPress = null;
                KeyDown = null;
                KeyUp = null;
            }

            base.Dispose(isDisposing);
        }
    }
}
