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

            pricesSearch.ItemSelected += OnItemSelected;
            pricesSearch.ItemsSource = App.Context.Prices.ToList();
        }

        private void OnItemSelected(object selectedItem)
        {
            if (selectedItem is Price price)
            {
                MessageBox.Show($"Выбран: {price.Name}"); 
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
