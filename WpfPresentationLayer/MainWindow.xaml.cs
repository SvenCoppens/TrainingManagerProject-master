using DataLayer;
using DataLayer.Repositories;
using DomainLibrary.Domain;
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

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddingButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)RadioAddCycling.IsChecked)
            {
                Window cyclingWindow = new AddCyclingSessionWindow();
                cyclingWindow.Show();
            }
            else
            {
                Window RunningWindow = new RunningWindow();
                RunningWindow.Show();
            }
        }

        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            bool cycling = (bool)CyclingOverviewCheckbox.IsChecked;
            bool running = (bool)RunninggOverviewCheckbox.IsChecked;
            if (cycling&&running)
            {

            }
            else if (running)
            {

            }
            else
            {

            }
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            bool isNumber = int.TryParse(AmountToShow.Text,out amount);
            if (isNumber)
            {
                SessionType  sT;
                if ((bool)RadioShowCycling.IsChecked)
                {
                    sT = SessionType.Cycling;
                }
                else
                {
                    sT = SessionType.Running;
                }
                Window showLatestWindow = new ShowLatestWindow(amount,sT);
                showLatestWindow.Show();
                //not sure if i should keep this
                this.Close();
            }
            else
            {
                MessageBox.Show("The amount given needs to be a number! ", "Invalid Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountToShow.Focus();
                AmountToShow.SelectAll();
            }
        }
    }
}
