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
    public partial class EstateUCEdit : UserControl
    {
        #region Свойства
        public Estate Estate { get; private set; } // для доступа в usercontrol

        public event EventHandler<EstateEventArgs> RemoveEstateRequested; // Событие для удаления недвижимости

        private Estate _estate; // этот элемент 
        private RealtorsFirmContext dbContext; // доступ к бд
        #endregion

        public EstateUCEdit(Estate estate)
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить недвижимость по адресу {_estate.Address}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.DeleteEstate(_estate);

                MessageBox.Show("Недвижимость удалена!",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                // Передаем информацию о действии
                RemoveEstateRequested?.Invoke(this, new EstateEventArgs { Estate = this.Estate });
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditEstate edit = new(_estate);
            edit.ShowDialog();

            // Передаем информацию о действии
            RemoveEstateRequested?.Invoke(this, new EstateEventArgs { Estate = this.Estate });
            DataLoad();
        }

        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы точно хотите архивировать недвижимость по адресу {_estate.Address}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ModelActions.ArchiveEstate(_estate);

                MessageBox.Show("Недвижимость архивирована!",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                // Передаем информацию о действии
                RemoveEstateRequested?.Invoke(this, new EstateEventArgs { Estate = this.Estate });
            }
        }
    }

    public class EstateEventArgs : EventArgs
    {
        public Estate Estate { get; set; }
    }
}
