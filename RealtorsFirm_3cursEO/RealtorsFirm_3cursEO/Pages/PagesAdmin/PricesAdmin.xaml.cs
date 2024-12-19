using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Edits;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.WindowsActions.Price;
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

namespace RealtorsFirm_3cursEO.Pages.PagesAdmin
{
    /// <summary>
    /// Логика взаимодействия для PricesAdmin.xaml
    /// </summary>
    public partial class PricesAdmin : Page
    {
        #region Свойства_и_поля
        // Класс для фильтрации и сортировки
        private DataFilterPrices DataFilterPrices;

        // Работа с бд
        private RealtorsFirmContext dbContext;
        private Employee _employeeAuth;

        // выбранный пользователь
        private Price _selectedPrice;
        #endregion

        public PricesAdmin(Employee employee)
        {
            InitializeComponent();
            this._employeeAuth = employee;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Передаем ФИО в текстблок с инфо
            UserFio.Text = _employeeAuth.FullName;

            // Загрузка комбобоксов
            var sorterList = UploadDataFilter.SorterPrices();
            ComboBoxSort.ItemsSource = sorterList;

            ComboBoxSort.SelectedIndex = 0;

            // Загрузка датагрид
            UpdateDataEmployees();
        }

        private void UpdateDataEmployees()
        {
            dbContext = new RealtorsFirmContext();

            // Инициализация класса для фильтрации данных
            DataFilterPrices = new DataFilterPrices(SearchTextBox, ComboBoxSort, SortCheckBox);
            var pricesList = dbContext.Prices.ToList();

            pricesList = DataFilterPrices.ApplySorter(pricesList);
            pricesList = DataFilterPrices.ApplySearch(pricesList);

            PricesDataGrid.ItemsSource = null;
            PricesDataGrid.ItemsSource = pricesList;

            if (pricesList.Count == 0)
            {
                textFound.Visibility = Visibility.Visible;
            }
            else
            {
                textFound.Visibility = Visibility.Hidden;
            }
        }

        #region Обработчики_событий

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данную услугу?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.DeletePrice(_selectedPrice);
                _selectedPrice = null;

                UpdateDataEmployees();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            EditPrice window = new EditPrice(_selectedPrice);
            window.ShowDialog();

            _selectedPrice = null;
            UpdateDataEmployees();
        }

        private void AddButtonPrice_Click(object sender, RoutedEventArgs e)
        {
            AddPrice window = new AddPrice();
            window.ShowDialog();

            UpdateDataEmployees();
        }

        private void ButtonArchive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите архивировать данного сотрудника?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.ArchivePrice(_selectedPrice);
                _selectedPrice = null;

                UpdateDataEmployees();
            }
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PricesDataGrid.SelectedItem != null)
            {
                _selectedPrice = (Price)PricesDataGrid.SelectedItem;
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

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SortCheckBox.IsChecked = false;
                ComboBoxSort.SelectedIndex = 0;
                SearchTextBox.Text = "";
                UpdateDataEmployees();
            }
        }
        #endregion
    }
}
