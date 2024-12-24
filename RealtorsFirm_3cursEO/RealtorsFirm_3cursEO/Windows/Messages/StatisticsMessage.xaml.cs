using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

namespace RealtorsFirm_3cursEO.Windows
{
    /// <summary>
    /// Логика взаимодействия для Messeges.xaml
    /// </summary>
    public partial class StatisticsMessage : Window
    {
        public StatisticsMessage(Transaction transaction)
        {
            InitializeComponent();
            DataContext = transaction;

            RealtorsFirmContext dbContext = new RealtorsFirmContext();
            CloseButton.Focus();

            // Инициализируем лист со всеми транакциями для передачи данных к ListView
            var transactions = dbContext.TransactionPriceRelations
                .Include(r => r.IdPriceNavigation)
                .Where(r => r.IdTransaction == transaction.IdTransaction).ToList();

            // Инициализируем лист со всеми транакциями для привязки данных к ListView в Xaml
            List<DataTransaction> dataList = new List<DataTransaction>();

            // Передаем данные из листа в класс DataTransaction и добавляем в ListView
            int count = 1;
            foreach (var trans in transactions)
            {
                DataTransaction data = new DataTransaction(count++, trans.IdPriceNavigation.Name, trans.IdPriceNavigation.Cost);
                dataList.Add(data);
            }

            // Указываем ItemsSource для ListView
            PricesListView.ItemsSource = dataList;

            TotalCostTB.Text = dataList.Sum(r => r.Cost).ToString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    internal class DataTransaction
    {
        public DataTransaction(int count, string name, int cost)
        {
            Count = count;
            Cost = cost;
            Name = name;

        }
        public int Count { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
