using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
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
        RealtorsFirmContext dbContext;

        private string Email => TextBoxEmail.Text;
        private string Password => TextBoxPassVisibility.Visibility is Visibility.Visible ?  TextBoxPassVisibility.Text : TextBoxPassHidden.Password;

        public Authorization()
        {
            InitializeComponent();
            dbContext = new();
        }

        private void CheckBoxPasswordView_Click(object sender, RoutedEventArgs e)
        {
            TextBoxPatterns.HiddenPassword(sender, TextBoxPassHidden, TextBoxPassVisibility);
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            dbContext = new();

            // Находим пользователя
            var employee = dbContext.Employees
                .Include(e => e.IdRoleNavigation)
                .ToList()
                .SingleOrDefault(r => r.Email == Email && r.Password == Password);

            // Проверка на найденного пользователя
            if (employee != null)
            {
                MessageBox.Show($"Добро пожаловать в систему, {employee.Name} {employee.Firstname}!" +
                    $"\nВы вошли как {employee.IdRoleNavigation.Name}", "Авторизация прошла успешно",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (employee.IdRole == 1)
                {
                    MenuAdmin page = new MenuAdmin(employee);
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
                MessageBox.Show("Данного пользователя не существует. Проверьте правильность набора логина или пароля и повторите попытку", 
                    "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}