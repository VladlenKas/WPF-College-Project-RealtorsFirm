using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealtorsFirm_3cursEO.Classes
{
    public static class DataLimitators
    {
        public static bool LimitatorAge(short age)
        {
            // Ограничение по возрасту (например, не более 80 лет)
            if (age < 0 || age > 80)
            {
                MessageBox.Show("Вам не должно быть столько лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool LimitatorYear(short year)
        {
            // Ограничение по году (например, не раньше 1990 и не позже текущего года)
            int currentYear = DateTime.Now.Year;
            if (year < 1990 || year > currentYear)
            {
                MessageBox.Show("Год должен быть не раньше 1990 и не позже текущего года.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
