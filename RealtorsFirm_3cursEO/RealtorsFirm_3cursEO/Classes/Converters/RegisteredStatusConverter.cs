using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealtorsFirm_3cursEO.Classes.Converters
{
    public class RegisteredStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is sbyte IsRegistered)
            {
                return IsRegistered == 1 ? "Пользователь" : "Гость";
            }
            return "Неизвестно"; // На случай, если значение не соответствует ожидаемому типу
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Обратное преобразование не требуется
        }
    }
}
