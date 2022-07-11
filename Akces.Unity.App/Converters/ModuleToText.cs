using Akces.Unity.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Akces.Unity.App.Converters
{
    internal class ModuleToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var module = value as string;

            switch (module)
            {
                case Modules.Accounts:
                    return "Konta sprzedaży";
                case Modules.Reports:
                    return "Raporty";
                case Modules.Tasks:
                    return "Zadania";
                case Modules.Prizes:
                    return "Cenniki";
                case Modules.Users:
                    return "Konta użytkowników";
                case Modules.Harmonograms:
                    return "Harmonogramy";               
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
