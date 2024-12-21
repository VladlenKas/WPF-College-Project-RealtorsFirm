using RealtorsFirm_3cursEO.Classes.DataOperations;
using RealtorsFirm_3cursEO.Classes;
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
using System.Windows.Shapes;
using System.Data;
using System.Collections;

namespace RealtorsFirm_3cursEO.Windows.WindowsActions.Estates
{
    /// <summary>
    /// Логика взаимодействия для EditEstate.xaml
    /// </summary>
    public partial class EditEstate : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private Client? Client => ClientAutoComplete.SelectedItem as Client; // Не должен бытть пустой
        private TypeEstate? TypeEstate => TypeEstateComboBox.SelectedItem as TypeEstate; // Не должен быть пустой
        private int Area
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AreaTextBox.Text))
                {
                    AreaTextBox.Text = "0";
                    return 0;
                }
                return int.Parse(AreaTextBox.Text);
            }
        }
        private int NumberRooms
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NumberRoomsTextBox.Text))
                {
                    NumberRoomsTextBox.Text = "0";
                    return 0;
                }
                return int.Parse(NumberRoomsTextBox.Text);
            }
        }
        private string Cost
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CostTextBox.Text))
                {
                    CostTextBox.Text = "0";
                    return null;
                }
                return CostTextBox.Text;
            }
        }
        private string Address => AddressTextBox.Text; // Не должен быть пустой 
        private byte[]? Photo => WindowHelper.ImageSourceToBytes(ImageBorder.Source);
        private bool _imageChanged = false;
        #endregion

        #region Поля со старыми данными
        private Client _originalClient;
        private TypeEstate _originalTypeEstate;
        private int _originalArea;
        private int _originalNumberRooms;
        private string _originalCost;
        private string _originalAddress;
        private byte[]? _originalPhoto;
        #endregion

        private RealtorsFirmContext dbContext;
        private Estate _estate;
        public EditEstate(Estate estate)
        {
            InitializeComponent();

            _estate = estate;
            dbContext = new RealtorsFirmContext();

            // Возвращает только тех пользователей, кто имеет не более 5 объектов
            // или владельца нынешнего объекта
            List<Client> clients = dbContext.Clients 
                .Where(r =>
                    (r.IsArchive != 1 &&
                    dbContext.Estates.Count(e => e.IdClient == r.IdClient && e.IsArchive != 1) < 5) ||
                    r.IdClient == estate.IdClient) 
                .ToList();

            // Заполняем листы значениями
            ClientAutoComplete.ItemsSource = clients; // Заполняем лист клиентами
            TypeEstateComboBox.ItemsSource = dbContext.TypeEstates.ToList(); // Заполняем лист типами недвижимости
        }

        private bool HasDataChanged()
        {
            // Сравниваем текущие значения с оригинальными
            return Client != _originalClient ||
                   TypeEstate != _originalTypeEstate ||
                   Area != _originalArea ||
                   NumberRooms != _originalNumberRooms ||
                   Cost != _originalCost ||
                   Address != _originalAddress ||
                   _imageChanged;
        }

        private void CreateNewEmployee()
        {
            // Проверка на изменения данных
            if (!HasDataChanged())
            {
                var resultChanged = MessageBox.Show("Данные не изменились. Внесите изменения для сохранения",
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите редактировать данные недвижимости?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.EditEstate(_estate, Client.IdClient, TypeEstate.IdType, Address, Area, NumberRooms, Cost, Photo);
                MessageBox.Show($"Недвижимость по адресу {Address} клиента {Client.FullName} успешна отредактирована!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorEstate(_estate, Client, TypeEstate, Address,
                Area, NumberRooms, Cost, Photo);

            if (fieldsIsValid)
            {
                CreateNewEmployee();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            bool closing = WindowHelper.WindowClose();
            if (closing)
            {
                this.Close();
            }
        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            ImageBorder.Source = null;
            if (_originalPhoto != null)
            {
                _imageChanged = true;
            }
            else
            {
                _imageChanged = false;
            }
        }

        private void EditImage_Click(object sender, RoutedEventArgs e)
        {
            _imageChanged = WindowHelper.OpenImage(ImageBorder);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputCyrillic(e);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteCyrillic(e);
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputNumbers(e);
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteNumbers(e);
        }

        private void DescriptionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputDescription(e);
        }

        private void DescriptionTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteDescriptionForAddress(e);
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Client originalClient = dbContext.Clients.Single(r => r.IdClient == _estate.IdClient);
            TypeEstate originalTypeEstate = dbContext.TypeEstates.Single(r => r.IdType == _estate.IdType);

            // Сохраняем старые данные для проверки на то, были ли изменения
            _originalClient = originalClient;
            _originalTypeEstate = originalTypeEstate;
            _originalArea = _estate.Area;
            _originalNumberRooms = _estate.NumberRooms;
            _originalCost = _estate.Cost;
            _originalAddress = _estate.Address;
            _originalPhoto = _estate.Photo;

            // Загружаем данные в текстбоксы для редактирования
            ClientAutoComplete.SelectedItem = _originalClient;
            TypeEstateComboBox.SelectedItem = _originalTypeEstate;
            AreaTextBox.Text = _originalArea.ToString();
            NumberRoomsTextBox.Text = _originalNumberRooms.ToString();
            CostTextBox.Text = _originalCost;
            AddressTextBox.Text = _originalAddress;

            ImageBorder.DataContext = _estate;
        }
    }
}
