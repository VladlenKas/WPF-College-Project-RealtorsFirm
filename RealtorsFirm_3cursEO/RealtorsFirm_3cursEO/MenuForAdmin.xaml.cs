using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Edits;
using RealtorsFirm_3cursEO.ModelsDB;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO
{
    /// <summary>
    /// Логика взаимодействия для MenuForAdmin.xaml
    /// </summary>
    public partial class MenuForAdmin : Window
    {
        private RealtorsFirmContext dbContext;  
        private Employee _employee;
        private Client _client;
        // лист с ролями
        private List<object> filterList = new List<object>();
        // лист с полями для пользователя
        private List<object> sortFields = new List<object>
        {
            "Без сортировки",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 },
            "Имя",
            "Фамилия",
            "Отчество",
            "Дата рождения",
            "Паспорт",
            "Телефон",
            "Email"
        };
        // выбранный пользователь
        private object selectUser;

        public MenuForAdmin(Employee employee)
        {
            this._employee = employee;
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Authorization page = new Authorization();
            page.Show();
            this.Close();
        }
        private void Update()
        {
            dbContext = new RealtorsFirmContext();

            EmployeesDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = null;
            ComboBoxFilter.ItemsSource = null;

            // загрузка таблиц 
            dbContext.Employees.Include(e => e.IdRoleNavigation).Load();
            dbContext.Clients.Load();

            // заугрзка фильтра 
            filterList.Add("Нет фильтров");
            filterList.Add(new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 });
            foreach (var role in dbContext.RoleEmployees)
            {
                filterList.Add(role.Name);
            }
            ComboBoxFilter.ItemsSource = filterList;
            ComboBoxFilter.SelectedIndex = 0;

            // загрузка сортировки 
            ComboBoxSort.ItemsSource = sortFields;
            ComboBoxSort.SelectedIndex = 0;

            // сотрудники 
            EmployeesDataGrid.ItemsSource = dbContext.Employees.Local.ToBindingList();
            EmployeesDataGrid.Items.Refresh();
            // клиенты
            ClientsDataGrid.ItemsSource = dbContext.Clients.Local.ToBindingList();
            ClientsDataGrid.Items.Refresh();
        }
        private void LoadInfo()
        {
            this.Title = $"Меню администратора. Вы вошли как: {_employee.FullName}";

            //// Image load
            //if (_employee.IdRole == 1)
            //{
            //    UserIcon.ImageSource = new BitmapImage(new Uri("MenuManager.png", UriKind.Relative));
            //}
            //else if (_employee.IdRole == 2)
            //{
            //    UserIcon.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/MenuPhotograph.png"));
            //}

            // user's fio and role load
            UserFio.Text = $"{_employee.Firstname} {_employee.Name} {_employee.Patronymic}";
            UserRole.Text = _employee.IdRoleNavigation.Name;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
            LoadInfo();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                selectUser = (Employee)EmployeesDataGrid.SelectedItem;
            }
        }
        private void ClientsDataGridDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem != null)
            {
                selectUser = (Client)ClientsDataGrid.SelectedItem;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectUser is Employee employee && employee.IdEmployee == _employee.IdEmployee)
            {
                    MessageBox.Show("Администратор не может удалять сам себя", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного пользователя?", "Подтверждение",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Remove(selectUser);
                    dbContext.SaveChanges();

                    Update();
                    selectUser = null;
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectUser is Employee)
            {
                EditEmployee page = new EditEmployee(selectUser as Employee, _employee);
                page.ShowDialog();

                Update();
                selectUser = null;
            }
            else if (selectUser is Client)
            {
                EditClient page = new EditClient(selectUser as Client);
                page.ShowDialog();

                Update();
                selectUser = null;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != string.Empty)
            {
                SearchTextHint.Opacity = 0;
            }
            else
            {
                SearchTextHint.Opacity = 0.5;
            }

            string searchBox = SearchTextBox.Text.ToLower();

            if (EmployeesDataGrid.Visibility == Visibility.Visible)
            {
                var employees = dbContext.Employees.Where(r => r.Firstname.ToLower().StartsWith(searchBox)
                    || r.Name.ToLower().StartsWith(searchBox) || r.Patronymic.ToLower().StartsWith(searchBox)).ToList();
                EmployeesDataGrid.ItemsSource = employees;
            }
            else if (ClientsDataGrid.Visibility == Visibility.Visible)
            {
                var clients = dbContext.Clients.Where(r => r.Firstname.ToLower().StartsWith(searchBox)
                    || r.Name.ToLower().StartsWith(searchBox) || r.Patronymic.ToLower().StartsWith(searchBox)).ToList();
                ClientsDataGrid.ItemsSource = clients;
            }
        }

        private void ButtonsClients_Click(object sender, RoutedEventArgs e)
        {
            EmployeesDataGrid.Visibility = Visibility.Hidden;
            ClientsDataGrid.Visibility = Visibility.Visible;

            AddButtonEmployee.Visibility = Visibility.Hidden;
            AddButtonClient.Visibility = Visibility.Visible;

            NameDataGrid.Text = "Клиенты";
        }

        private void ButtonEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployeesDataGrid.Visibility = Visibility.Visible;
            ClientsDataGrid.Visibility = Visibility.Hidden;

            AddButtonEmployee.Visibility = Visibility.Visible;
            AddButtonClient.Visibility = Visibility.Hidden;    

            NameDataGrid.Text = "Сотрудники";
        }

        private void AddButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee page = new AddEmployee();
            page.ShowDialog();

            Update();
        }

        private void AddButtonClient_Click(object sender, RoutedEventArgs e)
        {
            AddClient page = new AddClient();
            page.ShowDialog();

            Update();
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFilter.SelectedItem != null)
            {
                string selectedText = ComboBoxFilter.SelectedItem.ToString();
                
                foreach (var nameRole in filterList)
                {
                    if (selectedText == "Нет фильтров")
                    {
                        EmployeesDataGrid.ItemsSource = dbContext.Employees.Local.ToBindingList();
                        EmployeesDataGrid.Items.Refresh();
                    }
                    else
                    {
                        EmployeesDataGrid.ItemsSource = dbContext.Employees.Where(r => r.IdRoleNavigation.Name == selectedText).ToList();
                        EmployeesDataGrid.Items.Refresh();
                    }
                }
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSort.SelectedItem != null)
            {
                string selectedField = ComboBoxSort.SelectedItem.ToString();
                var sortedEmployees = GetSortedEmployees(selectedField);
                EmployeesDataGrid.ItemsSource = sortedEmployees;
                EmployeesDataGrid.Items.Refresh();
            }
        }
        private IEnumerable<Employee> GetSortedEmployees(string fieldName)
        {
            switch (fieldName)
            {
                case "Имя":
                    return dbContext.Employees.Local.OrderBy(e => e.Name).ToList();
                case "Фамилия":
                    return dbContext.Employees.Local.OrderBy(e => e.Firstname).ToList();
                case "Отчество":
                    return dbContext.Employees.Local.OrderBy(e => e.Patronymic).ToList();
                case "Дата рождения":
                    return dbContext.Employees.Local.OrderBy(e => e.Birthday).ToList();
                case "Паспорт":
                    return dbContext.Employees.Local.OrderBy(e => e.Passport).ToList();
                case "Телефон":
                    return dbContext.Employees.Local.OrderBy(e => e.Phone).ToList();
                case "Email":
                    return dbContext.Employees.Local.OrderBy(e => e.Email).ToList();
                default:
                    return dbContext.Employees.Local.ToList();
            }
        }
    }
}
