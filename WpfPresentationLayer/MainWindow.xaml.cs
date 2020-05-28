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
        private string _databaseString;
        public MainWindow()
        {
            InitializeComponent();
            _databaseString = "Production";
            List<int> years = new List<int>();
            List<int> months = new List<int>();
            for(int i = DateTime.Now.Year; i >=2000; i--)
            {
                years.Add(i);
            }
            for(int i = 0; i < 12; i++)
            {
                months.Add(i + 1);
            }
            overviewYears.ItemsSource = years;
            overviewMonths.ItemsSource = months;
            overviewYears.SelectedIndex = 0;
            overviewMonths.SelectedIndex = 0;
        }

        private void AddingButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)RadioAddCycling.IsChecked)
            {
                Window cyclingWindow = new AddCyclingSessionWindow(_databaseString);
                cyclingWindow.Show();
            }
            else
            {
                Window RunningWindow = new AddRunningSessionWindow(_databaseString);
                RunningWindow.Show();
            }
        }

        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            bool cyclingChecked = (bool)CyclingOverviewCheckbox.IsChecked;
            bool runningChecked = (bool)RunningOverviewCheckbox.IsChecked;
            int year = (int)overviewYears.SelectedItem;
            int month = (int)overviewMonths.SelectedItem;
            //add the if here
            if(cyclingChecked && runningChecked)
            {
                Window overViewWindow = new OverviewTrainingReportWindow(_databaseString,year,month);
                overViewWindow.Show();
            }
            else if (cyclingChecked)
            {
                Window overviewWindow = new OverViewCyclingReportWindow(_databaseString, year, month);
                overviewWindow.Show();
            }
            else if(runningChecked)
            {
                Window overviewWindow = new OverviewRunningReportWindow(_databaseString, year, month);
                overviewWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select at least one type of training","No Training Selected",MessageBoxButton.OK);
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
                Window showLatestWindow = new ShowLatestWindow(_databaseString, amount,sT);
                showLatestWindow.Show();
            }
            else
            {
                MessageBox.Show("The amount given needs to be a number! ", "Invalid Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountToShow.Focus();
                AmountToShow.SelectAll();
            }
        }

        private void OpenRemoveIdsWindow(object sender, RoutedEventArgs e)
        {
            Window removeWindow = new DeletingWindow(_databaseString);
            removeWindow.Show();
        }
    }
}
