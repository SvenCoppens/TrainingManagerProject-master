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
    /// Interaction logic for OverViewCyclingReportWindow.xaml
    /// </summary>
    public partial class OverViewCyclingReportWindow : Window
    {
        private string _databaseString;
        public OverViewCyclingReportWindow(string databaseString,int year, int month)
        {
            InitializeComponent();
            _databaseString = databaseString;
            ShowResults(year, month);
        }

        public void ShowResults(int year, int month)
        {
            TrainingManager trainingManager = new TrainingManager(new UnitOfWork(new TrainingContext(_databaseString)));
            Report report = trainingManager.GenerateMonthlyCyclingReport(year, month);

            dataGridShowResults.ItemsSource = report.TimeLine;
            dataGridShowFastest.ItemsSource = new List<object>() { report.MaxSpeedSessionCycling};
            dataGridShowLongest.ItemsSource = new List<object>() { report.MaxDistanceSessionCycling};
            dataGridShowWattest.ItemsSource = new List<CyclingSession>() { report.MaxWattSessionCycling };
            startDateReport.Text = $"{report.StartDate.Year}/{report.StartDate.Month:00}/{report.StartDate.Day:00}";
            endDateReport.Text = $"{report.EndDate.Year}/{report.EndDate.Month:00}/{report.EndDate.Day:00}";

            AmountOfCyclingSessions.Text = report.CyclingSessions.ToString();
            TotalCyclingTime.Text = report.TotalCyclingTrainingTime.ToString();

            TotalCyclingDistance.Text = report.TotalCyclingDistance.ToString() + " km";
        }
    }
}
