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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RunningRepository rp = new RunningRepository(new TrainingContext());
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));
            m.AddCyclingTraining(DateTime.Now, 25, new TimeSpan(25), 10, 20, TrainingType.Endurance, "none", BikeType.CityBike);
        }
    }
}
