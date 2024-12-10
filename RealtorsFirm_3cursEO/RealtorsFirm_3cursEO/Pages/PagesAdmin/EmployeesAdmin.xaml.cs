﻿using Microsoft.EntityFrameworkCore;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для EmployeesAdmin.xaml
    /// </summary>
    public partial class EmployeesAdmin : Page
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

        public EmployeesAdmin(Employee employee)
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
            UpdateDataEmployees();
        }

        private void UpdateDataEmployees()
        {
            dbContext = new RealtorsFirmContext();
            
            // Инициализация класса для фильтрации данных
            DataFilterEmployees = new DataFilterEmployees(SearchTextBox, ComboBoxFilter, ComboBoxSort, SortCheckBox);
            var employeesList = dbContext.Employees.Include(e => e.IdRoleNavigation).ToList();

            employeesList = DataFilterEmployees.ApplyFilter(employeesList);
            employeesList = DataFilterEmployees.ApplySorter(employeesList);
            employeesList = DataFilterEmployees.ApplySearch(employeesList);

            EmployeesDataGrid.ItemsSource = null;
            EmployeesDataGrid.ItemsSource = employeesList;

            if (employeesList.Count == 0)
            {
                textFound.Visibility = Visibility.Visible;
            }
            else
            {
                textFound.Visibility = Visibility.Hidden;
            }
        }

        #region Обработчики_событий
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Authorization window = new Authorization();
            window.Show();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee.IdEmployee == _employeeAuth.IdEmployee)
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
                    ModelActions.DeleteEmployee(_selectedEmployee);
                    _selectedEmployee = null;
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditEmployee window = new EditEmployee(_selectedEmployee, _employeeAuth);
            window.ShowDialog();

            UpdateDataEmployees();
            _selectedEmployee = null;
        }

        private void AddButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee window = new AddEmployee();
            window.ShowDialog();

            UpdateDataEmployees();
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                _selectedEmployee = (Employee)EmployeesDataGrid.SelectedItem;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataEmployees();
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataEmployees();
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataEmployees();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataEmployees();
            }
        }
        #endregion

        private void ButtonArcive_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
