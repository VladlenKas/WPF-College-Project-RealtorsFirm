using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
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

namespace RealtorsFirm_3cursEO.Pages.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для StatisticsTransactionAdmin.xaml
    /// </summary>
    public partial class StatisticsTransactionAdmin : Page
    {
        #region Свойства_и_поля
        // Класс для фильтрации и сортировки
        private DataFilterEmployees DataFilterEmployees;

        // Работа с бд
        private RealtorsFirmContext dbContext;
        private Employee _employeeAuth;

        // выбранный пользователь
        private Employee _selectedEmployee;
        #endregion

        public StatisticsTransactionAdmin(Employee employee)
        {
            InitializeComponent();
            this._employeeAuth = employee;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Передаем ФИО в текстблок с инфо
            UserFio.Text = _employeeAuth.FullName;

            // Загрузка комбобоксов
            var filterList = UploadDataFilter.FilterEmployees();
            var sorterList = UploadDataFilter.SorterEmployees();
            ComboBoxFilter.ItemsSource = filterList;
            ComboBoxSort.ItemsSource = sorterList;

            ComboBoxFilter.SelectedIndex = 0;
            ComboBoxSort.SelectedIndex = 0;

            // Загрузка датагрид
            UpdateDataTransactions();
        }

        private void UpdateDataTransactions()
        {
            dbContext = new RealtorsFirmContext();

            var transactionsList = dbContext.Transactions
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdStatusNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(e => e.IdClientNavigation)
                .ToList();

            /*// Инициализация класса для фильтрации данных
            DataFilterEmployees = new DataFilterEmployees(SearchTextBox, ComboBoxFilter, ComboBoxSort, SortCheckBox);
            var employeesList = dbContext.Employees.Include(e => e.IdRoleNavigation).ToList();

            employeesList = DataFilterEmployees.ApplyFilter(employeesList);
            employeesList = DataFilterEmployees.ApplySorter(employeesList);
            employeesList = DataFilterEmployees.ApplySearch(employeesList);*/

            TransactionsDataGrid.ItemsSource = null;
            TransactionsDataGrid.ItemsSource = transactionsList;
        }

        #region Обработчики_событий

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            /*EditEmployee window = new EditEmployee(_selectedEmployee, _employeeAuth);
            window.ShowDialog();

            _selectedEmployee = null;
            UpdateDataEmployees();*/
        }

        private void AddButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            /*AddEmployee window = new AddEmployee();
            window.ShowDialog();

            UpdateDataEmployees();*/
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (TransactionsDataGrid.SelectedItem != null)
            {
                _selectedEmployee = (Employee)TransactionsDataGrid.SelectedItem;
            }*/
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataTransactions();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SortCheckBox.IsChecked = false;
                ComboBoxFilter.SelectedIndex = 0;
                ComboBoxSort.SelectedIndex = 0;
                SearchTextBox.Text = "";
                UpdateDataTransactions();
            }
        }
        #endregion
    }
}
