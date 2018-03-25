using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UXI.Common.Converters
{
    public class GenericDataTemplateSelector : DataTemplateSelector, IValueConverter
    {
        public List<DataTemplateOption> Templates { get; set; } = new List<DataTemplateOption>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                var type = item.GetType();
                foreach (var template in Templates)
                {
                    if (type == template.DataType
                        || type.IsSubclassOf(template.DataType)
                        || template.DataType.IsAssignableFrom(type)
                        || (type.IsGenericType && type.GetGenericTypeDefinition() == template.DataType))
                    {
                        return template.Template;
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.SelectTemplate(value, null);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class DataTemplateOption : DependencyObject
    {
        public string Type { get { return DataType?.FullName; } set { DataType = System.Type.GetType(value); } }

        public Type DataType { get; private set; }

        public DataTemplate Template { get; set; }
    }
}
