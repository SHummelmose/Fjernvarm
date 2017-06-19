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
    /// Interaction logic for CreateGaugeWindow.xaml
    /// </summary>
    public partial class CreateGaugeWindow : Window
    {
        static Consumer c = null;

        public CreateGaugeWindow(Consumer consumer)
        {
            InitializeComponent();
            c = consumer;
            tBoxName.Text= c.Name;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string description = tBoxDescription.Text.Trim();

            if(description != "")
            {
                Service.CreateGauge(c.ID, description);
                this.Close();
            }
        }
    }
}
