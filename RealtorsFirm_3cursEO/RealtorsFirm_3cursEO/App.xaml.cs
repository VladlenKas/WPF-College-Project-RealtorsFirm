using RealtorsFirm_3cursEO.Model;
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
        public static RealtorsFirmContext Context { get; } = new RealtorsFirmContext();
    }
}
