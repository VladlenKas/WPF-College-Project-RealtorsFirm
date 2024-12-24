using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.UserControls.Cards;
using RealtorsFirm_3cursEO.Windows.WindowsActions.Estates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для PricesAdmin.xaml
    /// </summary>
    public partial class EstatesAdmin : Page
    {
        // Класс для фильтрации и сортировки
        private DataFilterEstates DataFilterEstates;

        // Работа с бд
        private Employee _employeeAuth;
        private RealtorsFirmContext dbContext;

        private string _fileAdmin = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/AdminIconImage.png";
        private string _fileRealtor = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/RealtorIconImage.png";
        public EstatesAdmin(Employee employee)

        {
            InitializeComponent();
            _employeeAuth = employee;
        }

        private void UpdateDataEstates()
        {
            dbContext = new RealtorsFirmContext();

            DataFilterEstates = new DataFilterEstates(SearchTextBox, ComboBoxSort, SortCheckBox, ComboBoxFilter);
            var estates = dbContext.Estates
                .Include(r => r.IdClientNavigation)
                .Include(r => r.IdTypeNavigation)
                .ToList();

            estates = DataFilterEstates.ApplyFilter(estates);
            estates = DataFilterEstates.ApplySorter(estates);
            estates = DataFilterEstates.ApplySearch(estates);

            ItemsControlItems.Items.Clear();
            foreach (var estate in estates)
            {
                var estateCard = new EstateUCEdit(estate); // Инициализируем карточку с недвижимостью
                estateCard.RemoveEstateRequested += Estate_RemoveEstateRequested; // Добавляем событие для удаления
                ItemsControlItems.Items.Add(estateCard); // добавляем в ItemsControl
            }
        }

        // Обработчик события для удаления/архивирования недвижимости
        private void Estate_RemoveEstateRequested(object sender, EstateEventArgs e)
        {
            UpdateDataEstates(); // Обновляем список 
        }

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

            // ComboBoxes load
            var sorterList = UploadDataFilter.SorterEstates();
            var filterList = UploadDataFilter.FilterEstates();
            ComboBoxSort.ItemsSource = sorterList;
            ComboBoxFilter.ItemsSource = filterList;

            UpdateDataEstates();
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsControlItems != null)
            {
                SortCheckBox.IsChecked = false;
                ComboBoxFilter.SelectedIndex = 0;
                ComboBoxSort.SelectedIndex = 0;
                SearchTextBox.Text = "";
                UpdateDataEstates();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ItemsControlItems != null)
            {
                UpdateDataEstates();
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsControlItems != null)
            {
                UpdateDataEstates();
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsControlItems != null)
            {
                UpdateDataEstates();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsControlItems != null)
            {
                UpdateDataEstates();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEstate addEstate = new AddEstate();
            addEstate.ShowDialog();

            UpdateDataEstates();
        }
    }
}
