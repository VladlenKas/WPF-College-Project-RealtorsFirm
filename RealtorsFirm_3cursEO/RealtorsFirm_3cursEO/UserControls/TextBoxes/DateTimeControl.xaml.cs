using RealtorsFirm_3cursEO.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для DateTimeControl.xaml
    /// </summary>
    public partial class DateTimeControl : UserControl
    {
        /// <summary>
        /// Логика взаимодействия для DateTextBox.xaml
        /// </summary>
        private bool _isUpdatingText = false;

        public DateTime Text
        {
            get
            {
                string birthday = dateTextBox.Text;

                if (DateTime.TryParseExact(birthday, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime; 
                }
                else
                {
                    return DateTime.MinValue; 
                }
            }
        }

        public void Clear()
        {
            dateTextBox.Clear();
        }

        public void SetNow()
        {
            dateTextBox.Text = DateTime.Now.ToString().Remove(DateTime.Now.ToString().Length - 2, 2);
        }

        public DateTimeControl()
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
                if (text.EndsWith('.') || text.EndsWith(' ') || text.EndsWith(':'))
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
            text = text.Replace(" ", "");
            text = text.Replace(":", "");

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
                else if (text.Length <= 9)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4, 4)} {text.Substring(8, 1)}";
                }
                else if (text.Length <= 10)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4, 4)} {text.Substring(8, 2)}";
                }
                else if (text.Length <= 11)
                {
                   dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4, 4)} {text.Substring(8, 2)}:{text.Substring(10, 1)}";
                }
                else if (text.Length <= 12)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4, 4)} {text.Substring(8, 2)}:{text.Substring(10, 2)}";
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
