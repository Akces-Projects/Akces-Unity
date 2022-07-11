using Akces.Unity.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Akces.Unity.App.Converters
{
    internal class AuthorisationTypeToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var authorisationType = (AuthorisationType)value;

            switch (authorisationType)
            {
                case AuthorisationType.Deny:
                    return "Brak";
                case AuthorisationType.AllowRead:
                    return "Tylko odczyt";
                case AuthorisationType.AllowReadWrite:
                    return "Odczyt i zapis";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
