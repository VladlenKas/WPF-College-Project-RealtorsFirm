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

namespace RealtorsFirm_3cursEO.Windows.WindowsActions.Estates
{
    /// <summary>
    /// Логика взаимодействия для AddEstate.xaml
    /// </summary>
    public partial class AddEstate : Window
    {
        #region Свойства для хранения значений из текстбоксов
        public Client? Client => ClientAutoComplete.SelectedItem as Client; // Не должен бытть пустой
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

        #endregion

        private RealtorsFirmContext dbContext;

        public AddEstate()
        {
            InitializeComponent();

            dbContext = new RealtorsFirmContext();

            // Возвращает только тех пользователей, кто имеет не более 5 объектов
            // или владельца нынешнего объекта
            List<Client> clients = dbContext.Clients
                .Where(r =>
                    (r.IsArchive != 1 &&
                    dbContext.Estates.Count(e => e.IdClient == r.IdClient && e.IsArchive != 1) < 5))
                .ToList();

            // Заполняем листы значениями
            ClientAutoComplete.ItemsSource = clients; // Заполняем лист клиентами
            TypeEstateComboBox.ItemsSource = dbContext.TypeEstates.ToList(); // Заполняем лист типами недвижимости
        }

        private void CreateNewEmployee()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите добавить новую недвижимость?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.AddEstate(Client.IdClient, TypeEstate.IdType, Address, Area, NumberRooms, Cost, Photo);
                MessageBox.Show($"Новая недвижимость по адресу {Address} клиента {Client.FullName} успешна добавлена!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorEstate(null, Client, TypeEstate, Address,
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
        }

        private void EditImage_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.OpenImage(ImageBorder);
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
    }
}
