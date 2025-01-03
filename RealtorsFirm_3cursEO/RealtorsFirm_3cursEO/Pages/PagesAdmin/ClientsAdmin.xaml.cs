﻿using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Edits;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.WindowsActions.ClientAct;
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

        // Выбранный пользователь
        private Client _selectedClient;
        #endregion

        private string _fileAdmin = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/AdminIconImage.png";
        private string _fileRealtor = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/RealtorIconImage.png";
        public ClientsAdmin(Employee employee)
        {
            InitializeComponent();
            _employeeAuth = employee;
        }

        private void UpdateDataClients()
        {
            dbContext = new RealtorsFirmContext();

            // Инициализация класса для фильтрации данных
            DataFilterClients = new DataFilterClients(SearchTextBox, ComboBoxSort, SortCheckBox);
            var employeesList = dbContext.Clients.ToList();

            employeesList = DataFilterClients.ApplySorter(employeesList);
            employeesList = DataFilterClients.ApplySearch(employeesList);

            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = employeesList;
        }

        #region Обработчики_событий
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Передаем ФИО в текстблок с инфо
            UserRole.Text = _employeeAuth.IdRoleNavigation.Name;
            UserFio.Text = _employeeAuth.FullName;

            if (_employeeAuth.IdRoleNavigation.Name == "Администратор")
            {
                UserIcon.ImageSource = new BitmapImage(new Uri(_fileAdmin, UriKind.Absolute));
            }
            else if (_employeeAuth.IdRoleNavigation.Name == "Риелтор")
            {
                UserIcon.ImageSource = new BitmapImage(new Uri(_fileRealtor, UriKind.Absolute));
            }

            // Загрузка комбобоксов
            var sorterList = UploadDataFilter.SorterClients();
            ComboBoxSort.ItemsSource = sorterList;

            ComboBoxSort.SelectedIndex = 0;

            // Загрузка датагрид
            UpdateDataClients();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного клиента? " +
                "При удалении так же удалятся все привязанные недвижимости.",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.DeleteClient(_selectedClient);
                _selectedClient = null;

                MessageBox.Show("Клиент удален.",
                "Успех",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                UpdateDataClients();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditClient window = new EditClient(_selectedClient, _employeeAuth);
            window.ShowDialog();

            _selectedClient = null;
            UpdateDataClients();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddClient window = new AddClient();
            window.ShowDialog();

            UpdateDataClients();
        }

        private void ButtonArchive_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Вы точно хотите архивировать данного клиента? " +
                "При архивации так же архивируются все привязанные недвижимости.",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.ArchiveClient(_selectedClient);
                _selectedClient = null;

                MessageBox.Show("Клиент архивирован.",
                "Успех",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                UpdateDataClients();
            }
        }

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
                UpdateDataClients();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataClients();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                UpdateDataClients();
            }
        } 
        
        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SortCheckBox.IsChecked = false;
                ComboBoxSort.SelectedIndex = 0;
                SearchTextBox.Text = "";
                UpdateDataClients();
            }
        } 
        #endregion
    }
}
