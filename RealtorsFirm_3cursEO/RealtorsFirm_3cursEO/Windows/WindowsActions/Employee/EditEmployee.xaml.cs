using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes.DataOperations;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

namespace RealtorsFirm_3cursEO.Edits
{
    /// <summary>
    /// Логика взаимодействия для EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private string Role => ComboBoxRole.Text; // Не должен быть пустой
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
        private Employee _selectEmployee;

        public EditEmployee(Employee selectEmployee, Employee thisEmployee)
        {
            InitializeComponent();

            _thisEmployee = thisEmployee;
            _selectEmployee = selectEmployee;

            dbContext = new RealtorsFirmContext();
            ComboBoxRole.ItemsSource = dbContext.RoleEmployees.Select(r => r.Name).ToList();
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

            var resultCreate = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите отредактирвоать данные сотрудника?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultCreate == MessageBoxResult.Yes)
            {
                ModelActions.EditEmployee(_selectEmployee, Role, Name, Firstname, Patronymic, Birthday, Phone, Passport, Email, Password);
                MessageBox.Show($"Сотрдуник {Firstname} {Name} успешно отредактирован!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        private bool HasDataChanged()
        {
            // Сравниваем текущие значения с оригинальными
            return Role != _originalRole ||
                   Name != _originalName ||
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
            bool fieldsIsValid = DataLimitators.LimitatorEmployee(_selectEmployee, Role, Name,
                Firstname, Birthday, Phone, Passport, Email, Password);

            if (fieldsIsValid)
            {
                DataEditEmployee();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Сохраняем старые данные для проверки на то, были ли изменения
            _originalRole = _selectEmployee.IdRoleNavigation.Name;
            _originalName = _selectEmployee.Name;
            _originalFirstname = _selectEmployee.Firstname;
            _originalPatronymic = _selectEmployee.Patronymic;
            _originalBirthday = _selectEmployee.Birthday;
            _originalPhone = _selectEmployee.Phone;
            _originalPassport = _selectEmployee.Passport;
            _originalEmail = _selectEmployee.Email;
            _originalPassword = _selectEmployee.Password;   

            // Загружаем данные в текстбоксы для редактирования
            ComboBoxRole.Text = _originalRole;
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
