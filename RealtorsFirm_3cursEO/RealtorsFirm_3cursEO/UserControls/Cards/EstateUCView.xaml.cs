using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.Windows.WindowsActions.Estates;
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
        #region Свойства
        private Estate _estate; // этот элемент 
        private RealtorsFirmContext dbContext; // доступ к бд
        #endregion

        public EstateUCView(Estate estate)
        {
            InitializeComponent();

            _estate = estate;
            DataLoad();
        }

        private void DataLoad()
        {
            dbContext = new();

            var thisEstate = dbContext.Estates
                           .Include(r => r.IdTypeNavigation)
                           .Include(r => r.IdClientNavigation)
                           .Single(r => r.IdEstate == _estate.IdEstate);

            _estate = thisEstate;
            DataContext = thisEstate;

            if (thisEstate.Photo == null)
            {
                string file = "pack://application:,,,/RealtorsFirm_3cursEO;component/Images/NotImage.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }
    }
}
