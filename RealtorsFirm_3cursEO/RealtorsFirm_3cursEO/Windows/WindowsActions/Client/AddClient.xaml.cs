using RealtorsFirm_3cursEO.Classes.DataOperations;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
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

namespace RealtorsFirm_3cursEO.WindowsActions.ClientAct
{
    /// <summary>
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private string Name => NameTextBox.Text; // Не должен быть пустой

        private string Firstname => FirstnameTextBox.Text; // Не должен быть пустой

        private string? Patronymic => PatronymicTextBox.Text;

        private DateOnly Birthday => DateTextBox.Text; // Не должен быть пустой + от 18 до 80 лет

        private string Phone => PhoneTextBox.Text; // Не должен быть пустой + не должен повторяться + минимум цифр

        private string Passport => PassportTextBox.Text; // Не должен быть пустой + не должен повторяться + минимум цифр

        private string Email => EmailTextBox.Text; // Не должен быть пустой + не должен повторяться + правильный формат

        private string Password => WindowHelper.GetPassword(PasswordTextBoxHid, PasswordTextBoxVis); // Не должен быть пустой
        #endregion

        private RealtorsFirmContext dbContext;

        public AddClient()
        {
            InitializeComponent();

            dbContext = new RealtorsFirmContext();
        }

        private void CreateNewEmployee()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите добавить нового клиента?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.AddClient(Name, Firstname, Patronymic, Birthday, Phone, Passport, Email, Password);
                MessageBox.Show($"Новый клиент {Firstname} {Name} успешно добавлен!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorClient(null, Name,
                Firstname, Birthday, Phone, Passport, Email, Password);

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

        private void ViewPasswordCheckBox_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.HiddenPassword(sender, PasswordTextBoxHid, PasswordTextBoxVis);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputCyrillic(e);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteCyrillic(e);
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
