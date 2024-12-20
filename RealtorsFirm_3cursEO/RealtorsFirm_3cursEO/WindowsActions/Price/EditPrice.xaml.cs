using RealtorsFirm_3cursEO.Classes.DataOperations;
using RealtorsFirm_3cursEO.Classes;
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
using RealtorsFirm_3cursEO.Model;

namespace RealtorsFirm_3cursEO.WindowsActions.Price
{
    /// <summary>
    /// Логика взаимодействия для EditPrice.xaml
    /// </summary>
    public partial class EditPrice : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private string Name => NameTextBox.Text; // Не должен быть пустой

        private int Cost // Не должен быть пустой + не должен повторяться + правильный формат
        {
            get
            {
                if (CostTextBox.Text == "")
                {
                    return 0;
                }

                return int.Parse(CostTextBox.Text);
            }
        }

        private string _originalName;
        private int _originalCost;
        #endregion

        private Model.Price _selectPrice;

        public EditPrice(Model.Price price)
        {
            InitializeComponent();
            _selectPrice = price;
        }

        private void DataEditPrice()
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

            var result = MessageBox.Show("Вы уверены, что заполнили все поля верно и хотите отредактировать услугу?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.EditPrices(_selectPrice, Name, Cost);
                MessageBox.Show($"Услуга {Name} успешно отредактирована!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        private bool HasDataChanged()
        {
            // Сравниваем текущие значения с оригинальными
            return Name != _originalName ||
                   Cost != _originalCost;
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            bool fieldsIsValid = DataLimitators.LimitatorPrice(_selectPrice, Name, Cost);

            if (fieldsIsValid)
            {
                DataEditPrice();
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

        private void TextBoxNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputNumbers(e);
        }

        private void TextBoxNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteNumbers(e);
        }

        private void PasswordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputPassword(e);
        }

        private void PasswordTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePastePassword(e);
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _originalName = _selectPrice.Name;
            _originalCost = _selectPrice.Cost;

            NameTextBox.Text = _originalName;
            CostTextBox.Text = _originalCost.ToString();
        }
    }
}
