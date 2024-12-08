using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace RealtorsFirm_3cursEO.Classes
{
    public static class DataValidatons
    {
        #region Валидация на написание текста
        // Ввод КИРРИЛИЦА
        public static bool ValidateInputCyrillic(TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[а-яА-Я\s]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return false;
            }
            return true;
        }

        // Ввод ЦИФРЫ
        public static bool ValidateInputNumbers(TextCompositionEventArgs e)
        {
            var regex = new Regex(@"0-9");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return false;
            }
            return true;
        }

        // Ввод КИРРИЛИЦА + ЦИФРЫ + СИМВОЛЫ (для адреса)
        public static bool ValidateInputDescriptionForAddress(TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[a-zA-Z\s-().,;""':]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return false;
            }
            return true;
        }

        // Ввод ЭЛЕКТРОННАЯ ПОЧТА
        public static bool ValidateInputEmail(TextCompositionEventArgs e)
        {
            // Ограничение на ввод описания (например, только буквы и пробелы)
            var regex = new Regex(@"[a-zA-Z\s]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}
