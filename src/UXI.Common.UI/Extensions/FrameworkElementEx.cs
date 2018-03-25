using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UXI.Common.Helpers;

namespace UXI.Common.UI.Extensions
{
   public static class FrameworkElementEx
    {
        public static async Task WaitUntilLoadedAsync(this FrameworkElement element)
        {
            await AsyncHelper.InvokeAsync<RoutedEventHandler>
            (
                () => { },
                handler => element.Loaded += handler,
                handler => element.Loaded -= handler,
                tcs => (o, args) => tcs.SetResult(true)
            );
        }
    }
}
