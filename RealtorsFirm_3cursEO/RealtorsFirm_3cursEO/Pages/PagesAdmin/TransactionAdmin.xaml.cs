using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Classes.DataOperations;
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
                _selectedClient = value;

                if (_selectedClient is Client client)
                {
                    List<Estate> estates = new(dbContext.Estates.Where(r => r.IdClient == client.IdClient && r.IsArchive != 1).ToList());
                    if (estates.Count == 0)
                    {
                        var noDataItem = new[] { new { Address = "У данного клиента нет недвижимостей" } };
                        EstateComboBox.ItemsSource = noDataItem;
                        EstateComboBox.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(300);
                        EstateComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        EstateComboBox.ItemsSource = dbContext.Estates
                            .Where(r => r.IdClient == client.IdClient && r.IsArchive != 1)
                            .ToList();
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
                WriteOffButton.IsChecked = true;

            }
        }

        /// <summary>
        /// Приватное поле для хранения значения с выбранным клиентом
        /// </summary>
        private Employee? _selectedRieltor;
        /// <summary>
        /// Свойство для доступа к полю с выбранным клиентом. 
        /// Также управляет видимостью элементов и коллекцией EstateComboBox
        /// </summary>
        public Employee? SelectedRieltor
        {
            get { return _selectedRieltor; }
            set
            {
                if (_selectedRieltor != value)
                {
                    _selectedRieltor = value;
                    OnPropertyChanged(nameof(SelectedRieltor));
                }
            }
        }

        /// <summary>
        /// Свойсвто для хранения и доступа к значению с выбранной датой
        /// </summary>
        public DateTime DateTransaction => dateTimeControl.Text;

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
            dbContext.Transactions
                .Include(r => r.IdEmployeeNavigation)
                .Load();

            Employee = employee;
            UserFio.Text = $"{employee.FullName}";
        }

        /// <summary>
        /// Отвечает за отображение кнопки для создания транзакции
        /// </summary>
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

        /// <summary>
        /// Отвечает за проверку на пустоту для всех полей и списков
        /// </summary>
        /// <returns></returns>
        private bool CheckFields()
        {
            bool allFieldsFilled = false;

            if (WriteOffButton.IsChecked == true)
            {
                allFieldsFilled = SelectedClient != null &&
                    SelectedEstate != null &&
                    SelectedRieltor != null &&
                    SelectedPrices.Count != 0; 
            }
            else if (ChargeBonusesButton.IsChecked == true)
            {
                allFieldsFilled = SelectedClient != null &&
                    SelectedEstate != null &&
                    SelectedRieltor != null &&
                    SelectedPrices.Count != 0 &&
                    amountBonusesMinusTextBox.Text != string.Empty;
            }

            return allFieldsFilled;
        }

        /// <summary>
        /// Создает новую транзакцию
        /// </summary>
        private void CreateNewTransaction()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите осуществить транзакцию?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool IsAccrualBonuses = WriteOffButton.IsChecked == true;
                ModelActions.CreateTransaction(SelectedRieltor.IdEmployee, SelectedClient.IdClient, SelectedEstate.IdEstate,
                    DateTransaction, AmountDiscard, AmountTotal, SelectedPrices, CountBonuses,
                    BonusesDiscardSelectedClient, IsAccrualBonuses);
                MessageBox.Show($"Транзакция на клиента {SelectedClient.FullName} успешно создана!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ClearFields();
                DataUpdate();
            }
        }

        /// <summary>
        /// Очищает все поля и списки
        /// </summary>
        private void ClearFields()
        {
            SelectedClient = null;
            SelectedEstate = null;
            SelectedRieltor = null;
            EstateComboBox.SelectedIndex = 0;
            AmountDiscard = 0;
            AmountTotal = 0;
            SelectedPrices = new List<Price>();
            CountBonuses = 0;
            BonusesDiscardSelectedClient = 0;
            selectedPricesListView.Items.Clear();
            CLientSearch.Text = string.Empty;
            EmployeesSearch.Text = string.Empty;
            dateTimeControl.Clear(); 
            WriteOffButton.IsChecked = true;
        }

        /// <summary>
        /// Загружает все данные, тем самым динамически их обновляет 
        /// </summary>
        private void DataUpdate()
        {
            PricesSearch.ItemSelected -= OnPriceSelected;
            CLientSearch.ItemSelected -= OnClientSelected;
            EmployeesSearch.ItemSelected -= OnEmployeeSelected;

            PricesSearch.ItemSelected += OnPriceSelected;
            PricesSearch.ItemsSource = dbContext.Prices.Where(r => r.IsArchive != 1).ToList();

            CLientSearch.ItemSelected += OnClientSelected;
            var clientsList = dbContext.Clients.Where(r => 
                r.IsArchive != 1)
                .ToList();

            EmployeesSearch.ItemSelected += OnEmployeeSelected;
            var emloyeesList = dbContext.Employees.Where(r =>
                r.IsArchive != 1 &&
                r.IdRole == 2)
                .ToList();

            CLientSearch.ItemsSource = clientsList;
            EmployeesSearch.ItemsSource = emloyeesList;

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

        /// <summary>
        /// Событие для выбора сотрудника
        /// </summary>
        /// <param name="selectedItem"></param>
        private void OnEmployeeSelected(object selectedItem)
        {
            if (selectedItem is Employee employee)
            {
                SelectedRieltor = employee;
            }
            else if (selectedItem is null)
            {
                SelectedRieltor = null;
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

                    MessageBox.Show("Сначала выберите клиента!",
                        "Предупреждение",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else if (SelectedClient != null && SelectedClient.Bonuses == 0)
                {
                    WriteOffButton.IsChecked = true;

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
            bool validData = DataLimitators.LimitatorTransaction(DateTransaction, SelectedClient, SelectedRieltor);
            if (validData)
            {
                CreateNewTransaction();
            }
        }

        private void EstateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedEstate = comboBox.SelectedItem as Estate;
        }

        private void SetNowDate_Click(object sender, RoutedEventArgs e)
        {
            dateTimeControl.SetNow();
        }
    }
}
