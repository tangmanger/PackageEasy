using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PackageEasy.Converters
{
    public class BoolRevolveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            bool flage = (bool)value;
            if (parameter == null)
                return flage;
            else
                return !flage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flage = (bool)value;
            if (parameter == null)
                return flage;
            else
                return !flage;
        }
    }
}
