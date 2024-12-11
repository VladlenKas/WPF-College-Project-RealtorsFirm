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
    public static class DataValidations
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
            var regex = new Regex(@"[0-9]");
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
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
                return false;
            }
            return true;
        }
        #endregion

        #region Валидация на вставку текста
        // Вставка КИРРИЛИЦА
        public static void ValidatePasteCyrillic(DataObjectPastingEventArgs e)
        {
            // Извлекаем текст из буфера обмена и конвертурием в текст
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"[а-яА-Я\s]"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        // Вставка ЦИФРЫ
        public static void ValidatePasteNumbers(DataObjectPastingEventArgs e)
        {
            // Извлекаем текст из буфера обмена и конвертурием в текст
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"[а-яА-Я\s]"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        // Вставка КИРРИЛИЦА + ЦИФРЫ + СИМВОЛЫ (для адреса)
        public static void ValidatePasteDescriptionForAddress(DataObjectPastingEventArgs e)
        {
            // Извлекаем текст из буфера обмена и конвертурием в текст
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"[a-zA-Z\s-().,;""':]"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        // Вставка ЭЛЕКТРОННАЯ ПОЧТА
        public static void ValidatePasteEmail(DataObjectPastingEventArgs e)
        {
            // Извлекаем текст из буфера обмена и конвертурием в текст
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }
        #endregion
    }
}
