using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFTracker.Utilities
{
    public class AsteriskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isModified && isModified)
                return "*";

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
