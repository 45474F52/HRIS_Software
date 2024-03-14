using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace HRIS_Software.Models.Converters
{
    internal sealed class DynamicTypeToContentTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type type)
            {
                object resource;

                if (type.Equals(typeof(bool)))
                {
                    resource = Application.Current.TryFindResource("BooleanItemTemplate");
                }
                else if (type.Equals(typeof(int)))
                {
                    resource = Application.Current.TryFindResource("IntegerItemTemplate");
                }
                else if (type.Equals(typeof(double)))
                {
                    resource = Application.Current.TryFindResource("DoubleItemTemplate");
                }
                else if (type.Equals(typeof(string)))
                {
                    resource = Application.Current.TryFindResource("StringItemTemplate");
                }
                else if (type.Equals(typeof(Enum)))
                {
                    resource = Application.Current.TryFindResource("EnumItemTemplate");
                }
                else if (type.Equals(typeof(ICommand)))
                {
                    resource = Application.Current.TryFindResource("ButtonItemTemplate");
                }
                else if (type.Equals(typeof(DateTime)))
                {
                    resource = Application.Current.TryFindResource("DateItemTemplate");
                }
                else
                {
                    throw new InvalidOperationException($"Не удалось найти ресурс для типа \"{type.FullName}\"");
                }

                if (resource is DataTemplate)
                {
                    return resource;
                }
                else
                {
                    throw new InvalidOperationException($"Получен объект типа \"{resource.GetType().FullName}\"");
                }
            }

            throw new ArgumentException("Передаваемый объект должен быть типа System.Type");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
