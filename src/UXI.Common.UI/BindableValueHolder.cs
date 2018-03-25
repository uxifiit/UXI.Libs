using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UXI.Common.UI
{
    /// <summary>
    /// Holds the value.
    /// Can be used to change several objects' properties at a time.
    /// </summary>
    //[ContentProperty(Name = nameof(Value))]
    public class BindableValueHolder : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(BindableValueHolder), null);

        /// <summary>
        /// Gets or sets the held value.
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
