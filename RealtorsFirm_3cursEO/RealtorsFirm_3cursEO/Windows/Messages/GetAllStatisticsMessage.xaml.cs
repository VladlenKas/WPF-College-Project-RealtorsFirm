using Microsoft.EntityFrameworkCore;
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

namespace RealtorsFirm_3cursEO.Windows.Messages
{
    /// <summary>
    /// Логика взаимодействия для GetAllStatisticsMessage.xaml
    /// </summary>
    public partial class GetAllStatisticsMessage : Window
    {
        public GetAllStatisticsMessage()
        {
            InitializeComponent();
            CloseButton.Focus();

            RealtorsFirmContext dbContext = new RealtorsFirmContext();
            dbContext.Transactions
                .Include(r => r.TransactionPriceRelations)
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdStatusNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(e => e.IdClientNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(r => r.IdTypeNavigation)
                .Load();

            var viewModel = new GetAllStatisticsViewModel(dbContext);
            DataContext = viewModel;
            selectedPricesListView.ItemsSource = viewModel.CalculateEstateTypeStatistics();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
