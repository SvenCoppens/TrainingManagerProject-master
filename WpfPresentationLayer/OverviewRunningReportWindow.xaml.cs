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
    /// Interaction logic for OverviewRunningReportWindow.xaml
    /// </summary>
    public partial class OverviewRunningReportWindow : Window
    {
        private string _databaseString;
        public OverviewRunningReportWindow(string databaseString, int year, int month)
        {
            InitializeComponent();
            _databaseString = databaseString;
            _databaseString = databaseString;
            ShowResults(year, month);
        }

        public void ShowResults(int year, int month)
        {
            TrainingManager trainingManager = new TrainingManager(new UnitOfWork(new TrainingContext(_databaseString)));
            Report report = trainingManager.GenerateMonthlyRunningReport(year, month);

            dataGridShowResults.ItemsSource = report.TimeLine;
            dataGridShowFastest.ItemsSource = new List<object>() { report.MaxSpeedSessionRunning };
            dataGridShowLongest.ItemsSource = new List<object>() { report.MaxDistanceSessionRunning };
            startDateReport.Text = $"{report.StartDate.Year}/{report.StartDate.Month:00}/{report.StartDate.Day:00}";
            endDateReport.Text = $"{report.EndDate.Year}/{report.EndDate.Month:00}/{report.EndDate.Day:00}";

            AmountOfRunningSessions.Text = report.RunningSessions.ToString();
            TotalRunningTime.Text = report.TotalRunningTrainingTime.ToString();

            TotalRunningDistance.Text = report.TotalRunningDistance.ToString() + " km";
        }
    }
}
