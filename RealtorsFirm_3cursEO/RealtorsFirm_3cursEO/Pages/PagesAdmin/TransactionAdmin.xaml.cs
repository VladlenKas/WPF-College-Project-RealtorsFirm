using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
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
    public partial class TransactionAdmin : Page
    {
        public TransactionAdmin(Employee employee)
        {
            InitializeComponent();
            UserFio.Text = $"{employee.FullName}";
        }

        private void OnPriceSelected(object selectedItem)
        {
            if (selectedItem is Price price)
            {
                if (!selectedPricesListView.Items.Contains(price))
                {
                    selectedPricesListView.Items.Add(price);
                }
                PricesSearch.Text = string.Empty;
            }
        }

        private void OnClientSelected(object selectedItem)
        {
            if (selectedItem is Client client)
            {
                MessageBox.Show(client.FullName);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PricesSearch.ItemSelected += OnPriceSelected;
            PricesSearch.ItemsSource = App.Context.Prices.ToList();

            CLientSearch.ItemSelected += OnClientSelected;
            CLientSearch.ItemsSource = App.Context.Clients.ToList();

            EstateComboBox.ItemsSource = App.Context.Estates.ToList();
            StatusComboBox.ItemsSource = App.Context.StatusTransactions.ToList();
        }
    }
}
