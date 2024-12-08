using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Edits;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для EmployeesAdmin.xaml
    /// </summary>
    public partial class EmployeesAdmin : Page
    {
        private RealtorsFirmContext dbContext;
        private Employee _employee;
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

        private object selectedEmployee;

        public EmployeesAdmin(Employee employee)
        {
            this._employee = employee;
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Authorization page = new Authorization();
            page.Show();
        }
        private void Update()
        {
            dbContext = new RealtorsFirmContext();

            EmployeesDataGrid.ItemsSource = null;
            ComboBoxFilter.ItemsSource = null;

            // загрузка таблиц 
            dbContext.Employees.Include(e => e.IdRoleNavigation).Load();

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
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                selectedEmployee = (Employee)EmployeesDataGrid.SelectedItem;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee is Employee employee && employee.IdEmployee == _employee.IdEmployee)
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
                    dbContext.Remove(selectedEmployee);
                    dbContext.SaveChanges();

                    Update();
                    selectedEmployee = null;
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee is Employee)
            {
                EditEmployee page = new EditEmployee(selectedEmployee as Employee, _employee);
                page.ShowDialog();

                Update();
                selectedEmployee = null;
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

            var employees = dbContext.Employees.Where(r => r.Firstname.ToLower().StartsWith(searchBox)
                || r.Name.ToLower().StartsWith(searchBox) || r.Patronymic.ToLower().StartsWith(searchBox)).ToList();
            EmployeesDataGrid.ItemsSource = employees;
        }

        private void AddButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee page = new AddEmployee();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = _employee.FullName;
            Update();
        }
    }
}
