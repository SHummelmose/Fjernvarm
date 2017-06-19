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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for UC_Consumers.xaml
    /// </summary>
    public partial class UC_Consumers : UserControl
    {
        public UC_Consumers()
        {
            InitializeComponent();
            lBoxConsumers.ItemsSource = Service.GetConsumers();
            lBoxConsumers.DisplayMemberPath = "Name";
        }

        private void tBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Not implemented
        }

        private void lBoxConsumers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Consumer c = (Consumer)lBoxConsumers.SelectedItem;

            if (c != null)
            {
                tBoxConsumerID.Text = c.ID + "";
                tBoxConsumerName.Text = c.Name;
                tBoxConsumerAddress.Text = c.Address;
                tBoxConsumerZip.Text = c.ZipCode;
                tBoxConsumerCity.Text = c.City;

                lBoxGauges.ItemsSource = c.Gauges;
                lBoxGauges.DisplayMemberPath = "Description";

                btnEditConsumer.IsEnabled = true;
                btnDeleteConsumer.IsEnabled = Service.CheckConsumerDeleteable(c);

                cbBoxYears.ItemsSource = "";
                cbBoxYears.IsEnabled = false;
                tBoxCooling.Clear();
                tBoxPrice.Clear();
            }
        }

        private void btnCreateConsumer_Click(object sender, RoutedEventArgs e)
        {
            CreateConsumerWindow c = new CreateConsumerWindow();
            c.ShowDialog();
            lBoxConsumers.ItemsSource = Service.GetConsumers();
        }

        private void btnEditConsumer_Click(object sender, RoutedEventArgs e)
        {
            Consumer c = (Consumer)lBoxConsumers.SelectedItem;
            if(c!= null)
            {
                string name = tBoxConsumerName.Text.Trim();
                string address = tBoxConsumerAddress.Text.Trim();
                string zip = tBoxConsumerZip.Text.Trim();
                string city = tBoxConsumerCity.Text.Trim();
                Service.UpdateConsumer(c, name, address, zip, city);

                lBoxConsumers.ItemsSource = Service.GetConsumers();
            }
        }

        private void btnDeleteConsumer_Click(object sender, RoutedEventArgs e)
        {
            Consumer c = (Consumer)lBoxConsumers.SelectedItem;
            if (c != null)
            {
                Service.DeleteConsumer(c);
                lBoxConsumers.ItemsSource = Service.GetConsumers();
                ClearGUI();
                btnEditConsumer.IsEnabled = false;
                btnDeleteConsumer.IsEnabled = false;
                cbBoxYears.IsEnabled = false;
            }
        }

        private void lBoxGauges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gauge g = (Gauge)lBoxGauges.SelectedItem;
            if(g != null)
            {
                cbBoxYears.ItemsSource = "";
                cbBoxYears.IsEnabled = true;
                cbBoxYears.ItemsSource = Service.GetYearsOnGauge(g);
                tBoxCooling.Clear();
                tBoxPrice.Clear();
            }
        }

        private void btnCreateGauge_Click(object sender, RoutedEventArgs e)
        {
            Consumer c = (Consumer)lBoxConsumers.SelectedItem;
            if (c != null)
            {
                lblError.Content = "";
                CreateGaugeWindow win = new CreateGaugeWindow(c);
                win.ShowDialog();

                lBoxGauges.ItemsSource = c.Gauges.ToList();
            }
            else
            {
                lblError.Content = "Vælg forbruger";
            }
        }

        private void cbBoxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gauge g = (Gauge)lBoxGauges.SelectedItem;
            string year = (string)cbBoxYears.SelectedItem;

            if(g != null)
            {
                tBoxCooling.Text =  Service.CalculateCoolingOnYear(g, year)+"";
                tBoxPrice.Text = Service.CalculatePriceOnYear(g, year) + "";
            }
        }

        /**
         * Empties GUI elements
         **/
        private void ClearGUI()
        {
            tBoxConsumerID.Clear();
            tBoxConsumerName.Clear();
            tBoxConsumerAddress.Clear();
            tBoxConsumerZip.Clear();
            tBoxConsumerCity.Clear();
            lBoxGauges.ItemsSource = null;
            cbBoxYears.ItemsSource = null;
            tBoxCooling.Clear();
            tBoxPrice.Clear();
        }
    }
}
