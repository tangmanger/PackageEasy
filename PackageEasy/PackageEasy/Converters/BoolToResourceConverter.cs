using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PackageEasy.Converters
{
    /// <summary>
    /// author:TT
    /// desc:按布尔值从资源字典取资源，ConverterParameter 格式："TrueKey|FalseKey"
    /// </summary>
    public class BoolToResourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keys = parameter as string;
            if (string.IsNullOrWhiteSpace(keys))
                return null;
            var parts = keys.Split('|');
            var key = value is true ? parts[0] : (parts.Length > 1 ? parts[1] : parts[0]);
            return Application.Current?.TryFindResource(key);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
