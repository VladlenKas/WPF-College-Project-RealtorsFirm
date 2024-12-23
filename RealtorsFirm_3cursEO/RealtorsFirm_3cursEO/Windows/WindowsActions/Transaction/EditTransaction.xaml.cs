using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Classes.DataOperations;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace RealtorsFirm_3cursEO.Windows.WindowsActions.Transactions
{
    /// <summary>
    /// Логика взаимодействия для EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        private Transaction _transaction;
        private RealtorsFirmContext dbContext;
        public EditTransaction(Transaction transaction)
        {
            InitializeComponent();

            _transaction = transaction;
            DataContext = transaction;

            dbContext = new RealtorsFirmContext();
            StatusComboBox.ItemsSource = new List<string>(dbContext.StatusTransactions.Select(r => r.Name).ToList());
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string statusName = StatusComboBox.SelectedValue.ToString();
            if (_transaction.IdStatusNavigation.Name != statusName)
            {

                var result = MessageBox.Show("Вы уверены, что хотите изменить статус транзакции?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    StatusTransaction status = dbContext.StatusTransactions.Single(r => r.Name == statusName);
                    Transaction transaction = dbContext.Transactions.Single(r => r.IdTransaction == _transaction.IdTransaction);

                    MessageBox.Show($"Транзакция с недвижимостью по адресу \"{_transaction.IdEstateNavigation.Address}\" " +
                        $"клиента {_transaction.IdEstateNavigation.IdClientNavigation.FullName} успешна отредактирована!",
                        "Успешно",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    transaction.IdStatus = status.IdStatus;
                    if (status.Name == "Завершено" || status.Name == "Отменено")
                    {
                        transaction.DateFinish = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Данные не изменились. Внесите изменения для сохранения",
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
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
    }
}
