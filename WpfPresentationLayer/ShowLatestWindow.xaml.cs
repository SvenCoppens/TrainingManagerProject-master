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
        private int _amountToShow;
        private SessionType _sessionType;
        private string _databaseString;
        public ShowLatestWindow(string databaseString,int amount,SessionType sessionType)
        {
            InitializeComponent();
            _databaseString = databaseString;
            _amountToShow = amount;
            _sessionType = sessionType;
            ShowResults(_amountToShow,_sessionType);
            
        }
        public void ShowResults(int amount, SessionType sessionType)
        {
            TrainingManager _tM = new TrainingManager(new UnitOfWork(new TrainingContext(_databaseString)));
            if (sessionType == SessionType.Cycling)
            {
                dataGridShowResults.ItemsSource = _tM.GetPreviousCyclingSessions(amount);
            }
            else
            {
                dataGridShowResults.ItemsSource = _tM.GetPreviousRunningSessions(amount);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            TrainingManager _tM = new TrainingManager(new UnitOfWork(new TrainingContext(_databaseString)));
            var selectedItems = dataGridShowResults.SelectedItems;
            List<int> cyclingIds = new List<int>();
            List<int> runningIds = new List<int>();
            for (int i = 0; i < selectedItems.Count; i++)
            {
                if(_sessionType == SessionType.Cycling)
                {
                    cyclingIds.Add((selectedItems[i] as CyclingSession).Id);
                }
                else
                {
                    runningIds.Add((selectedItems[i] as RunningSession).Id);
                }
            }

            _tM.RemoveTrainings(cyclingIds, runningIds);
            ShowResults(_amountToShow, _sessionType);
        }
    }
}
