using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Projekt_PK4
{
    public class BindingConverterPrice : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value + " zl";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BindingConverterInt32 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (int.TryParse(value.ToString(), out int number))
            {
                return number;
            }
            return 0;
        }
    }

    public class BindingConverterDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (Double.TryParse(value.ToString(), out double number))
            {
                return number;
            }
            return (Double)0;
        }
    }
}
