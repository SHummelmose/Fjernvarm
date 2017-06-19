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
    /// Interaction logic for UC_Statistics.xaml
    /// </summary>
    public partial class UC_Statistics : UserControl
    {
        public UC_Statistics()
        {
            InitializeComponent();

            tBoxYear.Text = DateTime.Now.Year + "";

            cbBoxYears.ItemsSource = Service.GetYearsWithReadings();
        }

        private void tBoxYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            string year = tBoxYear.Text.Trim();
            if (year != null)
            {
                int y = -1;
                if (Int32.TryParse(year, out y))
                {
                    bool isActive = Service.CheckActiveYear(year);

                    btnActivate.IsEnabled = !isActive;
                    btnDeactivate.IsEnabled = isActive;
                    if (isActive)
                    {
                        lblActive.Content = "Aktiv";
                    }
                    else
                    {
                        lblActive.Content = "Inaktiv";
                    }
                }
                else
                {
                    btnActivate.IsEnabled = false;
                    btnDeactivate.IsEnabled = false;
                    lblActive.Content = "Ikke et gyldigt årstal";
                }
            }
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            string year = tBoxYear.Text.Trim();

            MessageBoxResult answer = MessageBox.Show("Er du sikker på at du vil aktivere aflæsning for år " + year + "?\nDette vil DEAKTIVERE ALLE eventuelt aktive år!", "Aktivér aflæsning?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                Service.ActivateYear(year);
                btnActivate.IsEnabled = false;
                btnDeactivate.IsEnabled = true;
                lblActive.Content = "Aktiv";
            }
        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            string year = tBoxYear.Text.Trim();

            MessageBoxResult answer = MessageBox.Show("Er du sikker på at du vil deaktivere aflæsning for år " + year + "?", "Deaktivér aflæsning?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                Service.DeactivateYear(year);
                btnActivate.IsEnabled = true;
                btnDeactivate.IsEnabled = false;
                lblActive.Content = "Inaktiv";
            }
        }

        private void cbBoxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string year = (string)cbBoxYears.SelectedItem;
            if (year != null)
            {
                double[] averages = Service.AverageReadingsOnYear(year);
                tBoxCooling.Text = averages[0] + "";
                tBoxPrice.Text = averages[1] + "";
                tBoxHours.Text = averages[2] + "";
            }
        }
    }
}
