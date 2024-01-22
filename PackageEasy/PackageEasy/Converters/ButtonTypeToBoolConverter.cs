using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PackageEasy.Converters
{
    /// <summary>
    /// author:TT
    /// time:2023-03-22 23:25:32
    /// desc:ButtonTypeToBoolConverter
    /// </summary>
    public class ButtonTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if (parameter == null) return false;
            ButtonType buttonType = (ButtonType)value;
            int d = 0;
            int.TryParse(parameter.ToString(), out d);
            ButtonType buttonType1 = (ButtonType)d;
            return buttonType1 == buttonType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
