using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.UserControls.Cards;
using RealtorsFirm_3cursEO.Windows.WindowsActions.Estates;
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

namespace RealtorsFirm_3cursEO.Pages.PagesClient
{
    /// <summary>
    /// Логика взаимодействия для EstatesClient.xaml
    /// </summary>
    public partial class EstatesClient : Page
    {
        // Класс для фильтрации и сортировки
        private DataFilterEstates DataFilterEstates;

        // Работа с бд
        private RealtorsFirmContext dbContext;
        private Client _client;

        public EstatesClient(Client client)

        {
            InitializeComponent();
            _client = client;

            UserFio.Text = client.FullName;
        }

        private void UpdateDataEstates()
        {
            dbContext = new RealtorsFirmContext();

            DataFilterEstates = new DataFilterEstates(SearchTextBox, ComboBoxSort, SortCheckBox, ComboBoxFilter);
            var estates = dbContext.Estates
                .Include(r => r.IdClientNavigation)
                .Include(r => r.IdTypeNavigation)
                .Where(r => r.IdClient == _client.IdClient)
                .ToList();

            if (estates.Count == 0)
            {
                textFound.Text = $"У вас нет недвижимостей";
            }

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
            var estates = dbContext.Estates.Where(r => r.IdClient == _client.IdClient);
            if (estates.Count() < 5)
            {
                AddEstateClient addEstate = new AddEstateClient(_client);
                addEstate.ShowDialog();

                UpdateDataEstates();
            }
            else
            {
                MessageBox.Show("Вы имеете более чем 5 недвижимостей. Удалите или" +
                    "архивируйте одну или несколько из них, чтобы добавить новый объект.",
                    "Доступ ограничен.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
