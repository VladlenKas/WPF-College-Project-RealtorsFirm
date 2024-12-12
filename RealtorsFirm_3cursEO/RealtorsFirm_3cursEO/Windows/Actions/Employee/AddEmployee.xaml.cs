using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Classes.DataOperations;
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
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealtorsFirm_3cursEO
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
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

        private string Password => TextBoxPatterns.GetPassword(PasswordTextBoxHid, PasswordTextBoxVis); // Не должен быть пустой
        #endregion

        private RealtorsFirmContext dbContext;

        public AddEmployee()
        {
            InitializeComponent();

            dbContext = new RealtorsFirmContext();
            ComboBoxRole.ItemsSource = dbContext.RoleEmployees.Select(r => r.Name).ToList();
        }

        private void CreateNewEmployee()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите добавить нового сотрудника?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.AddEmployee(Role, Name, Firstname, Birthday, Phone, Passport, Email, Password);
                MessageBox.Show($"Новый сотрдуник {Firstname} {Name} успешно добавлен!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            } 
        }

        #region Обработчики событий

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.ValidationEmployee(null, Role, Name,
               Firstname, Birthday, Phone, Passport, Email, Password);

            if (fieldsIsValid)
            {
                CreateNewEmployee();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewPasswordCheckBox_Click(object sender, RoutedEventArgs e)
        {
            TextBoxPatterns.HiddenPassword(sender, PasswordTextBoxHid, PasswordTextBoxVis);
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
