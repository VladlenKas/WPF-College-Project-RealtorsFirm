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
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
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

        #region Поля со старыми данными
        private string _originalRole;
        private string _originalName;
        private string _originalFirstname;
        private string _originalPatronymic;
        private DateOnly _originalBirthday;
        private string _originalPhone;
        private string _originalPassport;
        private string _originalEmail;
        private string _originalPassword;
        #endregion

        private RealtorsFirmContext dbContext;
        private Employee _thisEmployee;
        private Client _selectClient;

        public EditClient(Client client, Employee thisEmployee)
        {
            InitializeComponent();

            _thisEmployee = thisEmployee;
            _selectClient = client;

            dbContext = new RealtorsFirmContext();
        }

        private void DataEditEmployee()
        {
            // Проверка на изменения данных
            if (!HasDataChanged())
            {
                var resultChanged = MessageBox.Show("Данные не изменились. Внесите изменения для сохранения",
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            var resultCreate = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите отредактирвоать данные клиента?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultCreate == MessageBoxResult.Yes)
            {
                ModelActions.EditClient(_selectClient, Name, Firstname, Patronymic, Birthday, Phone, Passport, Email, Password);
                MessageBox.Show($"Клиент {Firstname} {Name} успешно отредактирован!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        private bool HasDataChanged()
        {
            // Сравниваем текущие значения с оригинальными
            return Name != _originalName ||
                   Firstname != _originalFirstname ||
                   (Patronymic ?? "") != (_originalPatronymic ?? "") ||
                   Birthday != _originalBirthday ||
                   Phone != _originalPhone ||
                   Passport != _originalPassport ||
                   Email != _originalEmail;
        }

        #region Обработчики событий
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorClient(_selectClient, Name,
                Firstname, Birthday, Phone, Passport, Email, Password);

            if (fieldsIsValid)
            {
                DataEditEmployee();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Сохраняем старые данные для проверки на то, были ли изменения
            _originalName = _selectClient.Name;
            _originalFirstname = _selectClient.Firstname;
            _originalPatronymic = _selectClient.Patronymic;
            _originalBirthday = _selectClient.Birthday;
            _originalPhone = _selectClient.Phone;
            _originalPassport = _selectClient.Passport;
            _originalEmail = _selectClient.Email;
            _originalPassword = _selectClient.Password;

            // Загружаем данные в текстбоксы для редактирования
            NameTextBox.Text = _originalName;
            FirstnameTextBox.Text = _originalFirstname;
            PatronymicTextBox.Text = _originalPatronymic;
            DateTextBox.Text = _originalBirthday;
            PhoneTextBox.Text = _originalPhone;
            PassportTextBox.Text = _originalPassport;
            EmailTextBox.Text = _originalEmail;
            PasswordTextBoxHid.Password = _originalPassword;
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
