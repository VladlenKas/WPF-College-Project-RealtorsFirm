using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RealtorsFirm_3cursEO.Classes
{
    public static class DataLimitatorsMethods
    {
        #region Вспомогательные методы
        private static short CalculateAge(DateOnly birthDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;

            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            {
                age--;
            }

            return (short)age;
        }
        #endregion

        #region Ограничения

        // Ограничение по возрасту 
        public static bool LimitatorAge(DateOnly date)
        {
            short age = CalculateAge(date);

            if (age < 18 || age > 80)
            {
                return false;
            }
            return true;
        }

        // Ограничение по году 
        public static bool LimitatorYear(short year)
        {
            int currentYear = DateTime.Now.Year;
            if (year < 1990 || year > currentYear)
            {
                return false;
            }
            return true;
        } 

        // Ограничение на валидность почты
        public static bool LimitatorEmail(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,4}$");
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        } 

        #endregion
    }
}
