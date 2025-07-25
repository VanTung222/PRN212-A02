using System;
using System.Globalization;
using System.Windows.Data;

namespace FUMiniHotelManagement.Utils
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
                return date.ToString("dd/MM/yyyy");
            if (value is DateTime dt)
                return dt.ToString("dd/MM/yyyy");
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParseExact(value as string, "dd/MM/yyyy", null, DateTimeStyles.None, out var date))
                return date;
            if (DateOnly.TryParseExact(value as string, "dd/MM/yyyy", out var dOnly))
                return dOnly;
            return null;
        }
    }
}
