using RealtorsFirm_3cursEO.ModelsDB;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RealtorsFirm_3cursEO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RealtorsFirmContext context { get; } = new RealtorsFirmContext();
    }

}
