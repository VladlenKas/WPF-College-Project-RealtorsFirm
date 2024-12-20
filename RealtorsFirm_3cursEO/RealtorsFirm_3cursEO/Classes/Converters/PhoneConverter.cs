using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealtorsFirm_3cursEO.Classes.Converters
{
    public class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string Phone)
            {
                return Phone.Insert(1, " ").Insert(5, " ").Insert(9, " ").Insert(12, " ");     
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
