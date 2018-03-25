using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UXI.Common.Converters
{
    /// <summary>
    /// Converts the value from relative to absolute and back on the specified range.
    /// </summary>
    public class RelativeToAbsolutePositionConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the minimum absolute value on the range used in conversion.
        /// Default value is <value>0</value>.
        /// </summary>
        public double Minimum { get; set; } = 0d;

        /// <summary>
        /// Gets or sets the maximum absolute value on the range used in conversion.
        /// Default value is <value>0</value>.
        /// </summary>
        public double Maximum { get; set; } = 0d;

        /// <summary>
        /// Gets or sets the flag whether the converter methods should throw exceptions in cases when the conversion fails or return zero or limit values.
        /// Default value is <value>true</value>. 
        /// </summary>
        public bool ThrowOnError { get; set; } = true;


        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double relative = (double)value;
            return Convert(relative);
        }

        /// <summary>
        /// Converts the relative value to absolute value on the range specified with the <seealso cref="Minimum"/> and <seealso cref="Maximum"/> values. 
        /// </summary>
        /// <param name="relative">Relative value to convert to absolute value.</param>
        /// <exception cref="ArithmeticException">Thrown if the absolute range is invalid.</exception>
        /// <returns>absolute double value</returns>
        public double Convert(double relative)
        {
            double min = Minimum;
            double max = Maximum;

            return AssertRange(min, max, ThrowOnError) ? (relative * (max - min) + min) : 0d;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double absolute = (double)value;

            return ConvertBack(absolute);
        }

        /// <summary>
        /// Converts the absolute value on the range specified with the <seealso cref="Minimum"/> and <seealso cref="Maximum"/> values to a normalized relative value. 
        /// If the <paramref name="absoluteValue"/> is out of range, returns the nearest boundary of the range or throws <see cref="ArgumentOutOfRangeException"/> exception based on the <seealso cref="ThrowOnError"/> property value.
        /// </summary>
        /// <param name="absoluteValue">Absolute value to convert to normalize using the specified range.</param>
        /// <exception cref="ArithmeticException">Thrown if the absolute range is invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="absoluteValue"/> is out of the range and <seealso cref="ThrowOnError"/> is set to true.</exception>
        /// <returns>relative double value</returns>
        public double ConvertBack(double absoluteValue)
        {
            double min = Minimum;
            double max = Maximum;

            if (min == max || AssertRange(min, max, ThrowOnError) == false)
            {
                return 0d;
            }
            else if (absoluteValue > max || absoluteValue < min)
            {
                if (ThrowOnError)
                {
                    throw new ArgumentOutOfRangeException(nameof(absoluteValue), $"Passed value {absoluteValue:0.00} does not fall into the range ({min:0.00}, {max:0.00})");
                }
                return Math.Max(Math.Min(max, absoluteValue), min);
            }

            return ((absoluteValue - min) / (max - min));
        }

        /// <summary>
        /// Checks whether the range is valid with <paramref name="min"/> value less than <paramref name="max"/> value.
        /// </summary>
        /// <param name="min">minimum value of the range to assert.</param>
        /// <param name="max">maximum value of the range to assert.</param>
        /// <param name="throwOnFail">flag whether to throw <see cref="ArithmeticException"/> exception if the assertion fails</param>
        /// <exception cref="ArithmeticException">thrown when the assertion fails and the <paramref name="throwOnFail"/> is set to true.</exception>
        /// <returns>true if the range is valid, false if the assertion fails and the <paramref name="throwOnFail"/> was set to false.</returns>
        private static bool AssertRange(double min, double max, bool throwOnFail)
        {
            if (min > max)
            {
                if (throwOnFail)
                {
                    throw new ArithmeticException($"{nameof(Minimum)} value is greater than {nameof(Maximum)}, it must be less or equal than {nameof(Maximum)}.");
                }
                return false;
            }

            return true;
        }
    }
}
