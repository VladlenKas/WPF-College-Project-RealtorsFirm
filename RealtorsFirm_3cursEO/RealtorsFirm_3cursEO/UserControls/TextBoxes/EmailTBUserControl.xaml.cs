using RealtorsFirm_3cursEO.Classes;
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

namespace RealtorsFirm_3cursEO.UserControls.TextBoxes
{
    /// <summary>
    /// Логика взаимодействия для EmailTBUserControl.xaml
    /// </summary>
    public partial class EmailTBUserControl : UserControl
    {
        public string Text
        {
            get { return phoneTextBox.Text.Replace(" ", ""); }
            set { phoneTextBox.Text = value; }
        }

        public EmailTBUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Позволяет вводить только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataValidations.ValidateInputEmail(e);
        }

        private void phoneTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            DataValidations.ValidatePasteEmail(e);
        }
    }
}
