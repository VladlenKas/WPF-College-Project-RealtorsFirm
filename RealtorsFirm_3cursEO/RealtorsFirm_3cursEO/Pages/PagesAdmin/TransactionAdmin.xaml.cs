using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            VisibilityButtonAdd();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Свойства и поля
        /// <summary>
        /// Приватное поле для хранения значения с общей стоимостью услуг
        /// </summary>
        private int _amountTotal;
        /// <summary>
        /// Свойство для доступа к полю с общей стоимостью услуг
        /// </summary>
        public int AmountTotal
        {
            get { return _amountTotal; }
            set
            {
                if (_amountTotal != value)
                {
                    _amountTotal = value;
                    CountBonuses = _amountTotal / 10;
                    OnPropertyChanged(nameof(AmountTotal));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с общей стоимостью услуг со скидкой
        /// </summary>
        private int _amountDiscard;
        /// <summary>
        /// Свойство для доступа к полю с общей стоимостью услуг со скидкой
        /// </summary>
        public int AmountDiscard
        {
            get { return _amountDiscard; }
            set
            {
                if (_amountDiscard != value)
                {
                    _amountDiscard = value;
                    OnPropertyChanged(nameof(AmountDiscard));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с количеством начисляемых бонусов
        /// </summary>
        private int _countBonuses;
        /// <summary>
        /// Свойство для доступа к полю с количеством начисляемых бонусов
        /// </summary>
        public int CountBonuses
        {
            get { return _countBonuses; }
            set
            {
                if (_countBonuses != value)
                {
                    _countBonuses = value;
                    OnPropertyChanged(nameof(CountBonuses));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с количеством списываемых бонусов у выбранного клиента
        /// </summary>
        private int _bonusesDiscardSelectedClient;
        /// <summary>
        /// Свойство для доступа к полю с количеством списываемых бонусов у выбранного клиента
        /// </summary>
        public int BonusesDiscardSelectedClient
        {
            get { return _bonusesDiscardSelectedClient; }
            set
            {
                if (_bonusesDiscardSelectedClient != value)
                {
                    _bonusesDiscardSelectedClient = value;
                    OnPropertyChanged(nameof(BonusesDiscardSelectedClient));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с количеством бонусов у выбранного клиента
        /// </summary>
        private int _bonusesSelectedClient;
        /// <summary>
        /// Свойство для доступа к полю с количеством бонусов у выбранного клиента
        /// </summary>
        public int BonusesSelectedClient
        {
            get { return _bonusesSelectedClient; }
            set
            {
                if (_bonusesSelectedClient != value)
                {
                    _bonusesSelectedClient = value;
                    OnPropertyChanged(nameof(BonusesSelectedClient));
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
                dbContext = new();
                _selectedClient = value;

                if (_selectedClient is Client client)
                {
                    List<Estate> estates = new(dbContext.Estates.Where(r => r.IdClient == client.IdClient).ToList());
                    if (estates.Count == 0)
                    {
                        var noDataItem = new[] { new { Address = "У данного клиента нет недвижимостей" } };
                        EstateComboBox.ItemsSource = noDataItem;
                        EstateComboBox.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(300);
                        EstateComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        EstateComboBox.ItemsSource = dbContext.Estates.Where(r => r.IdClient == client.IdClient).ToList();
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

        /// <summary>
        /// Приватное поле для хранения значения с выбранным статусом транзакции
        /// </summary>
        private StatusTransaction _selectedStatusTransaction;
        /// <summary>
        /// Свойство для доступа к полю с выбранным статусом транзакции
        /// </summary>
        public StatusTransaction SelectedStatusTransaction
        {
            get { return _selectedStatusTransaction; }
            set
            {
                if (_selectedStatusTransaction != value)
                {
                    _selectedStatusTransaction = value;
                    OnPropertyChanged(nameof(SelectedStatusTransaction));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с выбранной недвижимостью
        /// </summary>
        private Estate _selectedEstate;
        /// <summary>
        /// Свойство для доступа к полю с выбранной недвижимостью
        /// </summary>
        public Estate SelectedEstate
        {
            get { return _selectedEstate; }
            set
            {
                if (_selectedEstate != value)
                {
                    _selectedEstate = value;
                    OnPropertyChanged(nameof(SelectedEstate));
                }
            }
        }

        /// <summary>
        /// Приватное поле для хранения выбранных услуг
        /// </summary>
        private List<Price> _selectedPrices = new List<Price>();
        /// <summary>
        /// Свойство для доступа к полю с выбранными услугами
        /// </summary>
        public List<Price> SelectedPrices
        {
            get { return _selectedPrices; }
            set
            {
                if (_selectedPrices != value)
                {
                    _selectedPrices = value;
                    OnPropertyChanged(nameof(SelectedPrices));
                }
            }
        }

        private Employee Employee;
        private RealtorsFirmContext dbContext;
        #endregion

        public TransactionAdmin(Employee employee)
        {
            InitializeComponent();

            // Устанавливаем контекст привязки, чтобы
            // все элементы обновлялись автоматически
            DataContext = this;

            dbContext = new();
            Employee = employee;
            UserFio.Text = $"{employee.FullName}";
        }

        private void VisibilityButtonAdd()
        {
            // Удаляем событие для того, чтобы кнопка были недоступна до проверки
            CreateNewTransatcionButton.Click -= CreateNewTransatcion_Click;
            // Делаем кнопку полупрозрачной для демонстрации некликабельности
            CreateNewTransatcionButton.Opacity = 0.5;

            // Проверяем, заполнены ли все необходимые поля
            bool allFieldsFilled = CheckFields();

            if (allFieldsFilled)
            {
                // Подключаем обработчик, если все поля заполнены
                CreateNewTransatcionButton.Click += CreateNewTransatcion_Click;
                CreateNewTransatcionButton.Opacity = 1;
            }
        }

        private bool CheckFields()
        {
            bool allFieldsFilled = false;

            if (WriteOffButton.IsChecked == true)
            {
                allFieldsFilled = SelectedClient != null &&
                                           SelectedEstate != null &&
                                           SelectedStatusTransaction != null &&
                                           SelectedPrices.Count != 0; 
            }
            else if (ChargeBonusesButton.IsChecked == true)
            {
                allFieldsFilled = SelectedClient != null &&
                                           SelectedEstate != null &&
                                           SelectedStatusTransaction != null &&
                                           SelectedPrices.Count != 0 &&
                                           amountBonusesMinusTextBox.Text != string.Empty;
            }

            return allFieldsFilled;
        }

        private void CreateNewTransaction()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите осуществить транзакцию?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool IsAccrualBonuses = WriteOffButton.IsChecked == true;
                ModelActions.CreateTransaction(Employee.IdEmployee, SelectedClient.IdClient, SelectedEstate.IdEstate,
                    SelectedStatusTransaction.IdStatus, AmountDiscard, AmountTotal, SelectedPrices, CountBonuses,
                    BonusesDiscardSelectedClient, IsAccrualBonuses);
                MessageBox.Show($"Транзакция на клиента {SelectedClient.FullName} успешно создана!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ClearFields();
                DataUpdate();
            }
        }

        private void ClearFields()
        {
            SelectedClient = null;
            SelectedEstate = null;
            EstateComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = -1;
            AmountDiscard = 0;
            AmountTotal = 0;
            SelectedPrices = new List<Price>();
            CountBonuses = 0;
            BonusesDiscardSelectedClient = 0;
            selectedPricesListView.Items.Clear();
            CLientSearch.Text = string.Empty;

            WriteOffButton.IsChecked = true;
        }

        private void DataUpdate()
        {
            PricesSearch.ItemSelected -= OnPriceSelected;
            CLientSearch.ItemSelected -= OnClientSelected;

            dbContext = new();
            PricesSearch.ItemSelected += OnPriceSelected;
            PricesSearch.ItemsSource = dbContext.Prices.ToList();

            CLientSearch.ItemSelected += OnClientSelected;
            CLientSearch.ItemsSource = dbContext.Clients.ToList();

            StatusComboBox.ItemsSource = dbContext.StatusTransactions.ToList();

            SelectedClient = null;
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
                    AmountTotal += price.Cost;
                    AmountDiscard += price.Cost;

                    var lisrPrices = new List<Price>();
                    foreach (var selectedPrice in selectedPricesListView.Items)
                    {
                        lisrPrices.Add(selectedPrice as Price);
                    }
                    SelectedPrices = new List<Price>(lisrPrices);
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
            }
            else if (selectedItem is null)
            {
                SelectedClient = null;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataUpdate();
        }

        private void AmountBonusesMinusTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputNumbers(e);
        }

        private void AmountBonusesMinusTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteNumbers(e);
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;

            if (button.Content.ToString() == "Списать")
            {
                if (SelectedClient == null)
                {
                    WriteOffButton.IsChecked = true;
                    ChargeBonusesButton.IsChecked = false;

                    MessageBox.Show("Сначала выберите клиента!",
                        "Предупреждение",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else if (SelectedClient != null && SelectedClient.Bonuses == 0)
                {
                    WriteOffButton.IsChecked = true;
                    ChargeBonusesButton.IsChecked = false;

                    MessageBox.Show("У данного клиента нет бонусов.",
                        "Предупреждение",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else
                {
                    int bonuses = Convert.ToInt32(SelectedClient.Bonuses);
                    BonusesSelectedClient = bonuses;
                    AmountTotal = AmountDiscard - BonusesDiscardSelectedClient;
                }
            }
            else
            {
                AmountTotal = AmountDiscard;
            }
            OnPropertyChanged("");
        }

        private void ClearPricesListView_Click(object sender, RoutedEventArgs e)
        {
            selectedPricesListView.Items.Clear();
            SelectedPrices = new List<Price>();
            AmountTotal = 0;
        }

        private void AmountBonusesMinusTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text != string.Empty)
            {
                int minusBonuses = int.Parse(textBox.Text);

                if (BonusesSelectedClient >= minusBonuses && minusBonuses <= 2000)
                {
                    AmountTotal = AmountDiscard;

                    BonusesDiscardSelectedClient = minusBonuses;
                    AmountTotal = AmountDiscard - BonusesDiscardSelectedClient;

                    return;
                }
                else if (minusBonuses > 2000)
                {
                    MessageBox.Show("Вы не можете списать больше 2000 бонусов.",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Вы не можете списать бонусов больше, чем есть у клиента.",
                            "Предупреждение",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }

                amountBonusesMinusTextBox.TextChanged -= AmountBonusesMinusTextBox_TextChanged;
                textBox.Text = BonusesDiscardSelectedClient.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                amountBonusesMinusTextBox.TextChanged += AmountBonusesMinusTextBox_TextChanged;
            }
            else
            {
                AmountTotal = AmountDiscard;
            }
        }

        private void AllBonusesSelect_Click(object sender, RoutedEventArgs e)
        {
            int minusBonuses = Convert.ToInt32(SelectedClient.Bonuses);
            if (minusBonuses > 2000)
            {
                amountBonusesMinusTextBox.Text = "2000";
            }
            else
            {
                amountBonusesMinusTextBox.Text = SelectedClient.Bonuses.ToString();
            }
        }

        private void CreateNewTransatcion_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTransaction();
        }

        private void EstateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedEstate = comboBox.SelectedItem as Estate;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedStatusTransaction = comboBox.SelectedItem as StatusTransaction;
        }
    }
}
