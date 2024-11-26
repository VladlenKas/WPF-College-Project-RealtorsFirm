﻿using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void CheckBoxPasswordView_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxPasswordView.IsChecked == true)
            {
                // Vissible pass
                TextBoxPassVisibility.Text = TextBoxPassHidden.Password;
                TextBoxPassVisibility.Visibility = Visibility.Visible;
                TextBoxPassHidden.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hidden pass
                TextBoxPassHidden.Password = TextBoxPassVisibility.Text;
                TextBoxPassVisibility.Visibility = Visibility.Hidden;
                TextBoxPassHidden.Visibility = Visibility.Visible;
            }
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Find user
            var employee = App.context.Employees.Include(e => e.IdRoleNavigation).ToList().Find(r => r.Email == TextBoxEmail.Text &&
                (r.Password == TextBoxPassVisibility.Text || r.Password == TextBoxPassHidden.Password));

            // Check role or null
            if (employee != null)
            {
                MessageBox.Show($"Добро пожаловать в систему, {employee.Name} {employee.Firstname}!" +
                    $"\nВы вошли как {employee.IdRoleNavigation.Name}", "Авторизация прошла успешно",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (employee.IdRole == 1)
                {
                    MenuForAdmin page = new MenuForAdmin(employee);
                    page.Show();
                    this.Close();
                }
                else if (employee.IdRole == 2)
                {
                    /*MenuPhotograph menuPhotograph = new MenuPhotograph();
                    menuPhotograph.Show();*/
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Данного пользователя не существует. Проверьте правильность набора символов", "Ошибка авторизации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}