using DataLayer;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for DeletingWindow.xaml
    /// </summary>
    public partial class DeletingWindow : Window
    {
        private string _databaseString;
        private List<int> _cyclingIds;
        private List<int> _runningIds;
        public DeletingWindow(string databaseString)
        {
            InitializeComponent();
            _databaseString = databaseString;
            _cyclingIds = new List<int>();
            _runningIds = new List<int>();
            cyclingIdsListView.ItemsSource = _cyclingIds;
            runningIdsListView.ItemsSource = _runningIds;
        }

        private void AddCyclingIdButtonClick(object sender, RoutedEventArgs e)
        {
            int newId = int.Parse(newCyclingIdTextBox.Text);
            _cyclingIds.Add(newId);
            _cyclingIds.Sort();
            cyclingIdsListView.ItemsSource = null;
            cyclingIdsListView.ItemsSource = _cyclingIds;
            newCyclingIdTextBox.Text = "";
        }

        private void AddRunningIdButtonClick(object sender, RoutedEventArgs e)
        {
            int newId = int.Parse(newRunningIdTextBox.Text);
            _runningIds.Add(newId);
            _runningIds.Sort();
            runningIdsListView.ItemsSource = null;
            runningIdsListView.ItemsSource = _runningIds;
            newRunningIdTextBox.Text = "";
        }

        private void DeleteGivenIds(object sender, RoutedEventArgs e)
        {
            try
            {
                TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext(_databaseString)));
                m.RemoveTrainings(_cyclingIds, _runningIds);
                _cyclingIds = new List<int>();
                _runningIds = new List<int>();

                MessageBox.Show("Id's succesfully deleted", "succesfull deletion", MessageBoxButton.OK);
                cyclingIdsListView.ItemsSource = null;
                cyclingIdsListView.ItemsSource = _cyclingIds;
                runningIdsListView.ItemsSource = null;
                runningIdsListView.ItemsSource = _runningIds;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveRunningIdsFromListButton(object sender, RoutedEventArgs e)
        {
            var toDeleteIds = runningIdsListView.SelectedItems;
            foreach(var id in toDeleteIds)
            {
                _runningIds.Remove((int)id);
            }
            runningIdsListView.ItemsSource = null;
            runningIdsListView.ItemsSource = _runningIds;
        }

        private void RemoveCyclingIdsFromListButton(object sender, RoutedEventArgs e)
        {
            var toDeleteIds = cyclingIdsListView.SelectedItems;
            foreach (var id in toDeleteIds)
            {
                _cyclingIds.Remove((int)id);
            }
            cyclingIdsListView.ItemsSource = null;
            cyclingIdsListView.ItemsSource = _cyclingIds;
        }
    }
}
