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
using Microsoft.EntityFrameworkCore;

namespace RealtorsFirm_3cursEO.Windows.Messages
{
    /// <summary>
    /// Логика взаимодействия для CreateTypeEstate.xaml
    /// </summary>
    public partial class CreateTypeEstate : Window
    {
        #region Свойства для хранения значений из текстбоксов
        private string Name => NameTextBox.Text; // Не должен быть пустой
        #endregion

        private RealtorsFirmContext dbContext;
        public CreateTypeEstate()
        {
            InitializeComponent();
            dbContext = new();
        }

        private void CreateNewEmployee()
        {
            var result = MessageBox.Show("Вы уверены, что заполнили поле с наименованием верно и хотите" +
                " добавить новый тип недвижимости?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TypeEstate typeEstate = new TypeEstate()
                {
                    Name = this.Name
                };
                dbContext.TypeEstates.Add(typeEstate);
                dbContext.SaveChanges();

                MessageBox.Show($"Новая тип недвижимости \"{Name}\" успешно добавлен!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        #region Обработчики событий
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Заполните поле с наименованием",
                "Ошибка.",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            else if (dbContext.TypeEstates.Any(r => r.Name == Name))
            {
                MessageBox.Show("Введенное наименование для типа недвижимости уже существует",
                "Ошибка.",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            else
            {
                CreateNewEmployee();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        #endregion
    }
}
