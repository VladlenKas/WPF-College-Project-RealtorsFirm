﻿using RealtorsFirm_3cursEO.Model;
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

namespace RealtorsFirm_3cursEO.Edits
{
    /// <summary>
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
    {
        private RealtorsFirmContext dbContext;
        private Client _client;
        public EditClient(Client client)
        {
            InitializeComponent();

            _client = client;
            dbContext = new RealtorsFirmContext();

            LoadInfo();
        }
        public void LoadInfo()
        {
            DataContext = _client;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmation = true;
            string[] formats = { "d/M/yyyy", "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy" };
            bool date = DateTime.TryParseExact(BirthdayTextBox.Text, formats, null,
                    System.Globalization.DateTimeStyles.None, out DateTime result);

            string phone = NumberConvertation(NumberTextBox.Text);

            // Проверка на пустую строку
            if (NameTextBox.Text == string.Empty ||
                FirstnameTextBox.Text == string.Empty ||
                EmailTextBox.Text == string.Empty ||
                BirthdayTextBox.Text == string.Empty ||
                NumberTextBox.Text == string.Empty ||
                PassportTextBox.Text == string.Empty)
            {
                confirmation = false;
                MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Проверка на логин
                if ((App.context.Employees.Any(r => r.Email == EmailTextBox.Text)
                    || App.context.Clients.Any(r => r.Email == EmailTextBox.Text))
                    && EmailTextBox.Text != _client.Email)
                {
                    confirmation = false;
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на паспорт
                else if ((App.context.Employees.Any(r => r.Passport == PassportTextBox.Text)
                    || App.context.Clients.Any(r => r.Passport == PassportTextBox.Text))
                    && PassportTextBox.Text != _client.Passport)
                {
                    confirmation = false;
                    MessageBox.Show("Пользователь с таким паспортом уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на номер телефона
                else if ((App.context.Employees.Any(r => r.Phone == NumberTextBox.Text)
                    || App.context.Clients.Any(r => r.Phone == NumberTextBox.Text))
                    && NumberTextBox.Text != _client.Phone)
                {
                    confirmation = false;
                    MessageBox.Show("Пользователь с таким номером телефона уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на имя
                else if (!NameTextBox.Text.All(char.IsLetter))
                {
                    confirmation = false;
                    MessageBox.Show("В имени должны быть ТОЛЬКО буквы", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на фамилию
                else if (!FirstnameTextBox.Text.All(char.IsLetter))
                {
                    confirmation = false;
                    MessageBox.Show("В фамилии должны быть ТОЛЬКО буквы", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на отчество
                else if (PatronymicTextBox != null && !PatronymicTextBox.Text.All(char.IsLetter))
                {
                    confirmation = false;
                    MessageBox.Show("В отчестве должны быть ТОЛЬКО буквы", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на дату
                else if (date && (result.AddYears(18) >= DateTime.Now
                    || result.AddYears(100) < DateTime.Now))
                {
                    confirmation = false;
                    MessageBox.Show("Пользователь должен быть возраста от 18 до 100 лет", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (date == false)
                {
                    confirmation = false;
                    MessageBox.Show("Дата рождения должна быть корректной", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Проверка на паспорт
                else if (!PassportTextBox.Text.All(char.IsNumber) || PassportTextBox.Text.Length > 10)
                {
                    confirmation = false;
                    MessageBox.Show("Паспорт должен содержать ТОЛЬКО цифры в количестве: 10", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!phone.All(char.IsNumber))
                {
                    confirmation = false;
                    MessageBox.Show("Номер телефона должен содержать ТОЛЬКО цифры", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (phone.Length < 11)
                {
                    confirmation = false;
                    MessageBox.Show("Номер телефона должен содержать минимум 11 цифр", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (PassportTextBox.Text.Length != 10)
                {
                    MessageBox.Show("Паспорт не может содержать меньше 10 цифр", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (confirmation)
            {
                // Находим нужного пользователя для изменений его данных и сверки параметров
                var client = dbContext.Clients.Single(r => r.IdClient == _client.IdClient);

                client.Name = NameTextBox.Text;
                client.Firstname = FirstnameTextBox.Text;
                client.Patronymic = PatronymicTextBox?.Text;
                client.Birthday = DateOnly.FromDateTime(result);
                client.Passport = PassportTextBox.Text;
                client.Phone = phone;

                dbContext.SaveChanges();

                MessageBox.Show($"Инофрмация о пользователе {client.Name} успешно изменена", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NumberTextBox.Text != string.Empty)
            {
                hintNumber.Opacity = 0;
            }
            else if (NumberTextBox.Text == string.Empty)
            {
                hintNumber.Opacity = 1;
            }

            if (NumberTextBox.Text.Contains('+'))
            {
                if (NumberTextBox.Text.Length > 16)
                {
                    NumberTextBox.Text = NumberTextBox.Text[..16];
                }
            }
            else if (NumberTextBox.Text.Length > 15)
            {
                NumberTextBox.Text = NumberTextBox.Text[..15];
            }

            if (NumberTextBox.Text.Length == 1 && NumberTextBox.Text == "+")
            {
                NumberTextBox.Text += "7-"; // +
            }

            if (NumberTextBox.Text.Contains('+'))
            {
                if (NumberTextBox.Text.Length == 6)
                {
                    NumberTextBox.Text += "-"; // +7-999-
                }
                if (NumberTextBox.Text.Length == 10)
                {
                    NumberTextBox.Text += "-"; // +7-999-999-
                }
                if (NumberTextBox.Text.Length == 13)
                {
                    NumberTextBox.Text += "-"; // +7-999-999-99-
                }
            }
            else
            {
                if (NumberTextBox.Text.Length == 1)
                {
                    NumberTextBox.Text += "-"; // 8-
                }
                if (NumberTextBox.Text.Length == 5)
                {
                    NumberTextBox.Text += "-"; // 8-999-
                }
                if (NumberTextBox.Text.Length == 9)
                {
                    NumberTextBox.Text += "-"; // 8-999-999-
                }
                if (NumberTextBox.Text.Length == 12)
                {
                    NumberTextBox.Text += "-"; // 8-999-999-99-
                }
            }

            NumberTextBox.SelectionStart = NumberTextBox.Text.Length;
        }

        private string NumberConvertation(string oldNumber)
        {
            string newNumber = "";

            if (oldNumber.StartsWith("+"))
            {
                oldNumber = oldNumber.Substring(1);
            }
            newNumber = oldNumber.Replace("-", "");

            return newNumber;
        }

        private void BirthdayTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BirthdayTextBox.Text != string.Empty)
            {
                hintBirthday.Opacity = 0;
            }
            else if (BirthdayTextBox.Text == string.Empty)
            {
                hintBirthday.Opacity = 1;
            }

            if (BirthdayTextBox.Text.Length == 2)
            {
                BirthdayTextBox.Text += "/";
            }

            if (BirthdayTextBox.Text.Length == 5)
            {
                BirthdayTextBox.Text += "/";
            }

            BirthdayTextBox.SelectionStart = BirthdayTextBox.Text.Length;
        }

        private readonly char[] CyrillicLetters =
        {
            // Верхний регистр
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й',
            'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У',
            'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э',
            'Ю', 'Я',
            // Нижний регистр
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
            'й','к','л','м','н','о','п','р','с','т','у','ф',
            'х','ц','ч','ш','щ','ъ','ы','ь','э','ю','я'
        };

        private void ValidateInput(TextBox textBox)
        {
            foreach (char c in textBox.Text)
            {
                // Проверяем, все ли символы в строке являются кириллическими
                if (!CyrillicLetters.Contains(c) && !char.IsWhiteSpace(c))
                {
                    textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
                    textBox.CaretIndex = textBox.Text.Length;
                    return;
                }
            }
            // проверяем на первую букву
            if (textBox.Text.Length == 1)
            {
                textBox.Text = textBox.Text.ToUpper();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInput(NameTextBox);
        }

        private void FirstnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInput(FirstnameTextBox);
        }

        private void PatronymicTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInput(PatronymicTextBox);
        }
    }
}