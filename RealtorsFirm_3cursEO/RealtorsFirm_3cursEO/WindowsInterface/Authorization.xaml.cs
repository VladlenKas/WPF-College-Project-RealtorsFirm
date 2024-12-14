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
        private string Password => WindowHelper.GetPassword(TextBoxPassHidden, TextBoxPassVisibility);

        public Authorization()
        {
            InitializeComponent();
            dbContext = new();
        }

        private void CheckBoxPasswordView_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.HiddenPassword(sender, TextBoxPassHidden, TextBoxPassVisibility);
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
                if (employee.IsArchive == 1)
                {
                    MessageBox.Show($"Пользователь {employee.Name} {employee.Firstname} архивирован. " +
                        $"Вы не можете продолжить авторизацию за этого сотрудника.", "Предупреждение.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show($"Добро пожаловать в систему, {employee.Name} {employee.Firstname}!" +
                    $"\nВы вошли как {employee.IdRoleNavigation.Name}.", "Авторизация прошла успешно.",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (employee.IdRoleNavigation.Name == "Администратор")
                {
                    MenuAdmin page = new MenuAdmin(employee);
                    page.Show();
                    this.Close();
                }
                else if (employee.IdRoleNavigation.Name == "Риелтор")
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