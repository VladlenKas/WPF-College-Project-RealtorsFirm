﻿using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Windows.Messages;
using RealtorsFirm_3cursEO.Windows.WindowsActions.Transactions;
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

namespace RealtorsFirm_3cursEO.Windows.WindowsInterface
{
    /// <summary>
    /// Логика взаимодействия для StatiscticsTransactionsRealtor.xaml
    /// </summary>
    public partial class StatisticsTransactionsRealtor : Page
    {
        #region Свойства_и_поля
        // Класс для фильтрации и сортировки
        private DataFilterTransactions DataFilterTransactions;

        // Работа с бд
        private RealtorsFirmContext dbContext;
        private Employee _employeeAuth;

        // выбранная транзакция
        private Transaction _selectedTransaction;
        #endregion

        public StatisticsTransactionsRealtor(Employee employee)
        {
            InitializeComponent();
            this._employeeAuth = employee;

            dbContext = new RealtorsFirmContext();
            dbContext.Transactions
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdStatusNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(e => e.IdClientNavigation)
                .Load();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Передаем ФИО в текстблок с инфо
            UserFio.Text = _employeeAuth.FullName;

            // Загрузка комбобоксов
            var filterList = UploadDataFilter.FilterTransactions();
            var sorterList = UploadDataFilter.SorterTransactions();
            ComboBoxFilter.ItemsSource = filterList;
            ComboBoxSort.ItemsSource = sorterList;

            SortCheckBox.IsChecked = true;
            ComboBoxFilter.SelectedIndex = 0;
            ComboBoxSort.SelectedIndex = 0;

            // Загрузка датагрид
            UpdateDataTransactions();
        }

        private void UpdateDataTransactions()
        {
            var transactionsList = dbContext.Transactions.Where(r => r.IdEmployee == _employeeAuth.IdEmployee).ToList();
            // Инициализация класса для фильтрации данных
            DataFilterTransactions = new DataFilterTransactions(SearchTextBox, ComboBoxSort, SortCheckBox, ComboBoxFilter);

            transactionsList = DataFilterTransactions.ApplyFilter(transactionsList);
            transactionsList = DataFilterTransactions.ApplySorter(transactionsList);
            transactionsList = DataFilterTransactions.ApplySearch(transactionsList);

            TransactionsDataGrid.ItemsSource = transactionsList;
        }

        #region Обработчики_событий

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditTransaction window = new EditTransaction(_selectedTransaction);
            window.ShowDialog();

            _selectedTransaction = null;

            dbContext = new RealtorsFirmContext();
            dbContext.Transactions
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdStatusNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(e => e.IdClientNavigation)
                .Load();
            UpdateDataTransactions();
        }

        private void GetAllStatistics_Click(object sender, RoutedEventArgs e)
        {
            // Затемняем окно
            App.MenuWindow.Opacity = 0.5;
            this.Opacity = 0.5;

            GetAllStatisticsMessage window = new GetAllStatisticsMessage(_employeeAuth);
            window.ShowDialog();

            // Снова проявляем окно
            App.MenuWindow.Opacity = 1;
            this.Opacity = 1;
        }

        private void TransactionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionsDataGrid.SelectedItem != null)
            {
                _selectedTransaction = (Transaction)TransactionsDataGrid.SelectedItem;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TransactionsDataGrid.ItemsSource != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionsDataGrid.ItemsSource != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionsDataGrid.ItemsSource != null)
            {
                UpdateDataTransactions();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsDataGrid.ItemsSource != null)
            {
                UpdateDataTransactions();
            }
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsDataGrid.ItemsSource != null)
            {
                SortCheckBox.IsChecked = true;
                ComboBoxFilter.SelectedIndex = 0;
                ComboBoxSort.SelectedIndex = 0;
                SearchTextBox.Text = "";
                UpdateDataTransactions();
            }
        }
        #endregion

        private void GetStatistic_Click(object sender, RoutedEventArgs e)
        {
            // Затемняем окно
            App.MenuWindow.Opacity = 0.5;
            this.Opacity = 0.5;

            StatisticsMessage message = new(_selectedTransaction);
            message.ShowDialog();

            // Снова проявляем окно
            App.MenuWindow.Opacity = 1;
            this.Opacity = 1;
        }
    }
}