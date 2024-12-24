using RealtorsFirm_3cursEO.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO.Pages.PagesRealtor
{
    /// <summary>
    /// Логика взаимодействия для PricesRealtor.xaml
    /// </summary>
    public partial class PricesRealtor : Page
    {
        #region Свойства_и_поля
        // Класс для фильтрации и сортировки
        private DataFilterPrices DataFilterPrices;

        // Работа с бд
        private RealtorsFirmContext dbContext;

        // выбранный пользователь
        private Price _selectedPrice;

        private readonly string _fileRealtor = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/RealtorIconImage.png";
        private readonly string _fileClient = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/ClientIconImage.png";
        #endregion

        public PricesRealtor(Employee employee)
        {
            InitializeComponent();
            UserFio.Text = employee.FullName;
            UserRole.Text = "Риелтор";
            UserIcon.ImageSource = new BitmapImage(new Uri(_fileRealtor, UriKind.Absolute));
        }

        public PricesRealtor(Client client)
        {
            InitializeComponent();
            UserFio.Text = client.FullName;
            UserRole.Text = "Клиент";
            UserIcon.ImageSource = new BitmapImage(new Uri(_fileClient, UriKind.Absolute));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

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
        }

        #region Обработчики_событий
        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PricesDataGrid.SelectedItem != null)
            {
                _selectedPrice = (Price)PricesDataGrid.SelectedItem;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PricesDataGrid != null)
            {
                UpdateDataEmployees();
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PricesDataGrid != null)
            {
                UpdateDataEmployees();
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PricesDataGrid != null)
            {
                UpdateDataEmployees();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (PricesDataGrid != null)
            {
                UpdateDataEmployees();
            }
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (PricesDataGrid != null)
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
