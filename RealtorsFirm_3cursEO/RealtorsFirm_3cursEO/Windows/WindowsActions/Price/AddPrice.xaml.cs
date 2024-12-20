using RealtorsFirm_3cursEO.Classes.DataOperations;
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
using System.Windows.Shapes;
using RealtorsFirm_3cursEO.Model;

namespace RealtorsFirm_3cursEO.WindowsActions.Price
{
    /// <summary>
    /// Логика взаимодействия для AddPrice.xaml
    /// </summary>
    public partial class AddPrice : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private string Name => NameTextBox.Text; // Не должен быть пустой

        private int Cost // Не должен быть пустой + не должен повторяться + правильный формат
        {
            get
            {
                if (CostTextBox.Text == "")
                {
                    return 0;
                }

                return int.Parse(CostTextBox.Text);
            }
        } 

        #endregion

        public AddPrice()   
        {
            InitializeComponent();
        }

        private void CreateNewEmployee()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите добавить новую услугу?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.AddPrice(Name, Cost);
                MessageBox.Show($"Новая услуга {Name} успешно добавлена!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorPrice(null, Name, Cost);

            if (fieldsIsValid)
            {
                CreateNewEmployee();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            bool closing = WindowHelper.WindowClose();
            if (closing)
            {
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputCyrillic(e);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteCyrillic(e);
        }

        private void TextBoxNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputNumbers(e);
        }

        private void TextBoxNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteNumbers(e);
        }

        private void PasswordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputPassword(e);
        }

        private void PasswordTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePastePassword(e);
        }
        #endregion
    }
}
