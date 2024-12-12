using RealtorsFirm_3cursEO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO.UserControls.TextBoxes
{
    /// <summary>
    /// Логика взаимодействия для DateTBUserControl.xaml
    /// </summary>
    public partial class DateTBUserControl : UserControl
    {
        /// <summary>
        /// Логика взаимодействия для DateTextBox.xaml
        /// </summary>
        private bool _isUpdatingText = false;

        private string _dateOnly;
        public DateOnly Text
        {
            get
            {
                string birthday = dateTextBox.Text;

                DateOnly.TryParseExact(birthday, "dd.MM.yyyy", out DateOnly date);
                return date;
            }
            set { }
        }

        public DateTBUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Удаление лишних символов в TextBox для даты
        /// </summary>
        /// <param name="e"></param>
        /// <param name="dateTextBox"></param>
        private void dateTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                string text = dateTextBox.Text;
                if (text.EndsWith("."))
                {
                    e.Handled = true; // Предотвращаем стандартное поведение Backspace или Delete

                    dateTextBox.Text = text.Substring(0, text.Length - 2); // Удалить два символа
                    dateTextBox.SelectionStart = dateTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Маска для TextBox с датой
        /// </summary>
        /// <param name="dateTextBox"></param>
        private void dateTextBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            if (_isUpdatingText) return;

            _isUpdatingText = true;

            string text = dateTextBox.Text.Replace(".", "");

            if (text.Length > 0)
            {
                if (text.Length <= 2)
                {
                    dateTextBox.Text = text;
                }
                else if (text.Length <= 4)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2)}";
                }
                else if (text.Length <= 8)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4)}";
                }

                dateTextBox.SelectionStart = dateTextBox.Text.Length;
            }

            _isUpdatingText = false;
        }

        /// <summary>
        /// Позволяет вводить только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputNumbers(e);
        }
    }
}
