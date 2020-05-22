using DataLayer;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for ShowLatestWindow.xaml
    /// </summary>
    public partial class ShowLatestWindow : Window
    {
        public ShowLatestWindow(int amount,SessionType sessionType)
        {
            InitializeComponent();
            ShowResults(amount,sessionType);
        }
        public void ShowResults(int amount, SessionType sessionType)
        {
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));
            if (sessionType == SessionType.Cycling)
            {
                dataGridShowResults.ItemsSource = m.GetPreviousCyclingSessions(amount);
            }
            else
            {
                dataGridShowResults.ItemsSource = m.GetPreviousRunningSessions(amount);
            }
        }
        private void Window_Closing(object sender,System.ComponentModel.CancelEventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
