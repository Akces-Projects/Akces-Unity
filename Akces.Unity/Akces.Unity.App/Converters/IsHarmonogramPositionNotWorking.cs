using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Akces.Unity.App.Converters
{
    internal class IsHarmonogramPositionNotWorking : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var harmonogramId = (int)values[0];
                var workingHarmonogramId = (int?)values[1];

                return workingHarmonogramId == harmonogramId ? Visibility.Hidden : Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
