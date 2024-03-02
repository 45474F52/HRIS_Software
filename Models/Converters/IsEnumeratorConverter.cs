using System;
using System.Globalization;
using System.Windows.Data;

namespace HRIS_Software.Models.Converters
{
    /// <summary>
    /// Определяет, является ли объект перечислением <see langword="enum"/>
    /// </summary>
    internal sealed class IsEnumeratorConverter : IValueConverter
    {
        /// <summary>
        /// Определяет, является ли переданный объект перечислением <see langword="enum"/>
        /// </summary>
        /// <param name="value">Тип <see langword="enum"/></param>
        /// <returns>Если <paramref name="value"/> — это <see cref="Type"/> и <paramref name="value"/> — это <see langword="enum"/>,
        /// то возвращает <see langword="true"/>, иначе <see langword="false"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type)
                return (value as Type).IsEnum;
            else
                return false;
        }

        [Obsolete("Метод не реализован")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
