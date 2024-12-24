using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Windows.WindowsInterface;
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

            if (Email == string.Empty || Password == string.Empty)
            {
                MessageBox.Show($"Заполните все поля.", "Предупреждение.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Находим пользователя
            var employee = dbContext.Employees
                .Include(e => e.IdRoleNavigation)
                .ToList()
                .SingleOrDefault(r => r.Email == Email && r.Password == Password);

            var client = dbContext.Clients
                .Where(r => r.IsRegistered == 1)
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
                    MenuAdmin menuAdmin = new MenuAdmin(employee);
                    menuAdmin.Show();
                    this.Close();
                }
                else if (employee.IdRoleNavigation.Name == "Риелтор")
                {
                    MenuRealtor menuRealtor = new MenuRealtor(employee);
                    menuRealtor.Show();
                    this.Close();
                }
            }
            else if (client != null)
            {
                if (client.IsArchive == 1)
                {
                    MessageBox.Show($"Пользователь {client.Name} {client.Firstname} архивирован. " +
                        $"Вы не можете продолжить авторизацию за этого клиента.", "Предупреждение.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show($"Добро пожаловать в систему, {client.Name} {client.Firstname}!" +
                    $"\nВы вошли как клиент.", "Авторизация прошла успешно.",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                MenuClient menuClient = new MenuClient(client);
                menuClient.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Данный пользователь не найден. Проверьте правильность набора логина или пароля и повторите попытку", 
                    "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}