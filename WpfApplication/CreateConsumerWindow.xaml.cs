using ClassLibrary;
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

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for CreateConsumerWindow.xaml
    /// </summary>
    public partial class CreateConsumerWindow : Window
    {
        public CreateConsumerWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string name = tBoxName.Text.Trim();
            string address = tBoxAddress.Text.Trim();
            string zip = tBoxZip.Text.Trim();
            string city = tBoxCity.Text.Trim();

            //Check validity of zip
            int i = -1;
            if(!Int32.TryParse(zip, out i))
            {
                lblZipError.Content = "Ugyldigt!";
                return;
            }
           
            Service.CreateConsumer(name, address, zip, city);
            this.Close();
        }
    }
}
