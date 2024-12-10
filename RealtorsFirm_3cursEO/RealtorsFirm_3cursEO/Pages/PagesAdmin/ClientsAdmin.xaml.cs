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

namespace RealtorsFirm_3cursEO.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для ClientsAdmin.xaml
    /// </summary>
    public partial class ClientsAdmin : Page
    {
        #region Свойства_и_поля
        // Класс для фильтрации и сортировки
        private DataFilterClients DataFilterClients;

        // Работа с бд
        private RealtorsFirmContext dbContext;
        private Employee _employeeAuth;

        // выбранный пользователь
        private Client _selectedClient;
        #endregion

        public ClientsAdmin(Employee employee)
        {
            InitializeComponent();
            _employeeAuth = employee;
        }

        private void UpdateDataEmployees()
        {
            dbContext = new RealtorsFirmContext();

            // Инициализация класса для фильтрации данных
            DataFilterClients = new DataFilterClients(SearchTextBox, ComboBoxSort, SortCheckBox);
            var employeesList = dbContext.Employees.Include(e => e.IdRoleNavigation).ToList();

            employeesList = DataFilterClients.ApplySorter(employeesList);
            employeesList = DataFilterClients.ApplySearch(employeesList);

            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = employeesList;

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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Передаем ФИО в текстблок с инфо
            UserFio.Text = _employeeAuth.FullName;

            // Загрузка комбобоксов
            var sorterList = UploadDataFilter.SorterClients();
            ComboBoxSort.ItemsSource = sorterList;

            ComboBoxSort.SelectedIndex = 0;

            // Загрузка датагрид
            UpdateDataEmployees();
        }
        /*
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного пользователя?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.DeleteClient(_selectedClient);
                _selectedClient = null;
            }
        }

        private void AddButtonClient_Click(object sender, RoutedEventArgs e)
        {

        }*/

        private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem != null)
            {
                _selectedClient = (Client)ClientsDataGrid.SelectedItem;
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataEmployees();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
    }
}
