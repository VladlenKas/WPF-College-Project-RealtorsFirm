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
        public static void ValidateInputCyrillic(TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[а-яА-Я\s]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        // Ввод ЦИФРЫ
        public static void ValidateInputNumbers(TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[0-9]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        // Ввод КИРРИЛИЦА + ЦИФРЫ + СИМВОЛЫ (для адреса)
        public static void ValidateInputDescriptionForAddress(TextCompositionEventArgs e)
        {
            // Ограничение на ввод описания (например, только буквы и пробелы)
            var regex = new Regex(@"[a-zA-Z\s-().,;""':]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        // Ввод ЭЛЕКТРОННАЯ ПОЧТА
        public static void ValidateInputEmail(TextCompositionEventArgs e)
        {
            // Ограничение на ввод эл. почты
            var regex = new Regex(@"^[a-zA-Z0-9\@\.]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        // Ввод ЛАТИНИЦА
        public static void ValidateInputPassword(TextCompositionEventArgs e)
        {
            // Ограничение на ввод пароля
            var regex = new Regex(@"^[a-zA-Z!@#$&*0-9]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
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

            if (!Regex.IsMatch(pastedText, @"^[a-zA-Z0-9\@\.]+$"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        // Вставка ЛАТИНИЦА
        public static void ValidatePastePassword(DataObjectPastingEventArgs e)
        {
            // Извлекаем текст из буфера обмена и конвертурием в текст
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"^[a-zA-Z!@#$&*0-9]+$"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }
        #endregion
    }
}
