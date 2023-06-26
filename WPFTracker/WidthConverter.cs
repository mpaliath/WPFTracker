using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFTracker
{
    public class MainWindowWidthToPopupWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double mainWindowWidth = (double)value;
            // Adjust the desired width of the popup as per your requirement
            double popupWidth = mainWindowWidth * 0.8; // Example: 80% of main window width
            return popupWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
