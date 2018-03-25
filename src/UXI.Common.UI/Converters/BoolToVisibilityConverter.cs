using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UXI.Common.Converters
{
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
    {
        public BoolToVisibilityConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }

}
