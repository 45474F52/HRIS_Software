using System;
using System.Windows.Data;
using System.Globalization;

namespace HRIS_Software.Models.Converters
{
    internal sealed class StringToFirstCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                text = text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    return char.ToUpper(text[0]);
                }
            }

            return '?';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
