using System;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;

namespace HRIS_Software.Models.Converters
{
    /// <summary>
    /// Конвертирует перечисление <see langword="enum"/> в массив
    /// </summary>
    internal sealed class EnumToArrayConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует переданное перечисление <see langword="enum"/> в массив <see cref="Array"/>
        /// </summary>
        /// <param name="value">Тип перечисления <see langword="enum"/></param>
        /// <returns>Возвращает массив значений <paramref name="value"/>
        /// как <see cref="Array"/>, если <paramref name="value"/> это <see cref="Type"/>,
        /// <br/> иначе возвращает пустой список <see cref="List{T}"/>,
        /// где <see langword="T"/> — это <see cref="string"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type)
                return Enum.GetValues(value as Type);
            else
                return new List<string>();
        }

        [Obsolete("Метод не реализован")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
