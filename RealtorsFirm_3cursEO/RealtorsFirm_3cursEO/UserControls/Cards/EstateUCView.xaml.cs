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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO.UserControls.Cards
{
    /// <summary>
    /// Логика взаимодействия для EstateUCView.xaml
    /// </summary>
    public partial class EstateUCView : UserControl
    {
        public EstateUCView(Estate estate)
        {
            InitializeComponent();

            var thisEstate = App.Context.Estates
                .Include(r => r.IdTypeNavigation)
                .Include(r => r.IdClientNavigation)
                .Single(r => r.IdEstate == estate.IdEstate);

            DataContext = thisEstate;

            if (thisEstate.Photo == null)
            {
                string file = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/NotImage.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }
    }
}
