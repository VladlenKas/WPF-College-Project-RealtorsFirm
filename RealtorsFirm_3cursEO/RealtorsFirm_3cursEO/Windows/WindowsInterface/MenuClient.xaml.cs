using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Pages.PagesAdmin;
using RealtorsFirm_3cursEO.Pages.PagesClient;
using RealtorsFirm_3cursEO.Pages.PagesRealtor;
using RealtorsFirm_3cursEO.PagesAdmin;
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

namespace RealtorsFirm_3cursEO.Windows.WindowsInterface
{
    /// <summary>
    /// Логика взаимодействия для MenuClient.xaml
    /// </summary>
    public partial class MenuClient : Window
    {
        private Client _client;

        public MenuClient(Client client)
        {
            InitializeComponent();
            _client = client;

            App.MenuWindow = this;
            ChoosePage(2);
        }

        public void ChoosePage(int numberPage)
        {
            switch (numberPage)
            {
                case 2:
                    ContentFrame.Navigate(new PricesRealtor(_client));
                    break;
                case 3:
                    ContentFrame.Navigate(new EstatesClient(_client)); // Готово
                    break;
                case 4:
                    //ContentFrame.Navigate(new TransactionRealtor(_client));
                    break;
                case 5:
                    //ContentFrame.Navigate(new StatiscticsTransactionsRealtor(_client));
                    break;
                case 6:
                    break;
            }
        }

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"Меню для клиента: {_client.FullName}";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            bool closing = WindowHelper.WindowClose();
            if (closing)
            {
                Authorization auth = new Authorization();
                auth.Show();
                this.Close();
            }
        }

        private void ButtonPrices_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(2);
        }

        private void ButtonEstates_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(3);
        }

        private void ButtonTransaction_Click(object sender, RoutedEventArgs e)
        {
            RealtorsFirmContext dbContext = new RealtorsFirmContext();
            bool hasTransction = dbContext.Transactions.Any(t => t.IdClient == _client.IdClient && (t.IdStatus == 1 || t.IdStatus == 4));
            if (hasTransction)
            {
                MessageBox.Show("У вас есть незавершенная транзакция. Дождитесь ее завершения, чтобы создать новую.",
                    "Доступ ограничен.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                ChoosePage(4);
            }
        }

        private void ButtonStatistics_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(5);
        }
    }
}
