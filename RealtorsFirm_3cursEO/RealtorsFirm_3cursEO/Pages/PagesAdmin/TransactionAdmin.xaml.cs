using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Pipes;
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
    /// Логика взаимодействия для TransactionAdmin.xaml
    /// </summary>
    public partial class TransactionAdmin : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// Приватное поле для хранения значения с общей стоимостью услуг
        /// </summary>
        private int _amountPrices;
        /// <summary>
        /// Свойство для доступа к полю с общей стоимостью услуг
        /// </summary>
        public int AmountPrices
        {
            get { return _amountPrices; }
            set
            {
                if (_amountPrices != value)
                {
                    _amountPrices = value;
                    OnPropertyChanged(nameof(AmountPrices));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с выбранным клиентом
        /// </summary>
        private Client? _selectedClient;
        /// <summary>
        /// Свойство для доступа к полю с выбранным клиентом. 
        /// Также управляет видимостью элементов и коллекцией EstateComboBox
        /// </summary>
        public Client? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;

                if (_selectedClient is Client client)
                {
                    List<Estate> estates = new (App.Context.Estates.Where(r => r.IdClient == client.IdClient).ToList());
                    if (estates.Count == 0)
                    {
                        var noDataItem = new[] { new { Address = "У данного клиента нет недвижимостей" } };
                        EstateComboBox.ItemsSource = noDataItem;
                        EstateComboBox.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(300);
                        EstateComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        EstateComboBox.ItemsSource = App.Context.Estates.Where(r => r.IdClient == client.IdClient).ToList();
                        EstateComboBox.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(400);
                    }
                }
                else
                {
                    var noDataItem = new[] { new { Address = "Сначала выберите клиента" } };
                    EstateComboBox.ItemsSource = noDataItem;
                    EstateComboBox.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(300);
                    EstateComboBox.SelectedIndex = 0;
                }
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        public TransactionAdmin(Employee employee)
        {
            InitializeComponent();

            amountPricesTextBlock.DataContext = this;
            UserFio.Text = $"{employee.FullName}";
        }

        /// <summary>
        /// Событие для выбора услуги
        /// </summary>
        /// <param name="selectedItem"></param>
        private void OnPriceSelected(object selectedItem)
        {
            if (selectedItem is Price price)
            {
                if (!selectedPricesListView.Items.Contains(price))
                {
                    selectedPricesListView.Items.Add(price);
                    AmountPrices += price.Cost;
                }
                PricesSearch.Text = string.Empty;
            }
        }

        /// <summary>
        /// Событие для выбора клиента
        /// </summary>
        /// <param name="selectedItem"></param>
        private void OnClientSelected(object selectedItem)
        {
            if (selectedItem is Client client)
            {
                SelectedClient = client;
                MessageBox.Show("Вы добавили клиента");
            }
            else if (selectedItem is null)
            {
                SelectedClient = null;
                MessageBox.Show("Вы удалили клиента");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PricesSearch.ItemSelected += OnPriceSelected;
            PricesSearch.ItemsSource = App.Context.Prices.ToList();

            CLientSearch.ItemSelected += OnClientSelected;
            CLientSearch.ItemsSource = App.Context.Clients.ToList();

            StatusComboBox.ItemsSource = App.Context.StatusTransactions.ToList();

            SelectedClient = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
