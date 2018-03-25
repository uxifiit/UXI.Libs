using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UXI.Common.Converters
{
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }

        public T FalseValue { get; set; }


        protected BoolToValueConverter() { }
        protected BoolToValueConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }


        private static bool IsInverted(object parameter)
        {
            return parameter != null ? System.Convert.ToBoolean(parameter) : false;
        }


        #region IValueConverter Members

        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = System.Convert.ToBoolean(value);

            return boolValue ^ IsInverted(parameter) ? TrueValue : FalseValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = value.Equals(TrueValue);

            return boolValue ^ IsInverted(parameter);
        }

        #endregion
    }



}
