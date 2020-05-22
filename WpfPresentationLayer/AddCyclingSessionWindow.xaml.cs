using DataLayer;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddCyclingSessionWindow : Window
    {
        private string _OptionalMessage = "(Optional)";
        public AddCyclingSessionWindow()
        {
            InitializeComponent();
            var trainingTypeValues = Enum.GetValues(typeof(TrainingType));
            trainingsTypeList.ItemsSource = trainingTypeValues;

            var bikeTypeValues = Enum.GetValues(typeof(BikeType));
            bikeTypeList.ItemsSource = bikeTypeValues;

            distanceTextBox.Text = _OptionalMessage;
            kmPerHoursTextBox.Text = _OptionalMessage;
            wattTextBox.Text = _OptionalMessage;


        }

        private void ShowWarningTextBox(string message)
        {
            MessageBox.Show(message, "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //focus zetten na elke error message
            //invoer controleren op getal tussen de juiste waarden
            bool mandatoryWasFilledIn = true;
            DateTime startTime;
            if (trainingDatePicker.SelectedDate != null)
            {
                startTime = (DateTime)trainingDatePicker.SelectedDate;

                int startHours;
                int startMinutes;

                bool correctStartTimeNumbersInput = int.TryParse(startTimeHours.Text, out startHours);
                correctStartTimeNumbersInput = int.TryParse(startTimeMinutes.Text, out startMinutes) && correctStartTimeNumbersInput;

                if (correctStartTimeNumbersInput)
                {
                    startTime.AddHours(startHours);
                    startTime.AddMinutes(startMinutes);

                    int durationHours;
                    int durationMinutes;

                    bool correctDurationTimeNumbersInput = int.TryParse(durationHoursTextBox.Text, out durationHours);
                    correctStartTimeNumbersInput = int.TryParse(durationMinutesTextBox.Text, out durationMinutes) && correctStartTimeNumbersInput;

                    if (correctDurationTimeNumbersInput)
                    {
                        TimeSpan duration = new TimeSpan(durationHours, durationMinutes, 0);

                        if (trainingsTypeList.SelectedItem != null)
                        {
                            TrainingType trainingType = (TrainingType)trainingsTypeList.SelectedItem;

                            if (bikeTypeList.SelectedItem != null)
                            {
                                BikeType bikeType = (BikeType)bikeTypeList.SelectedItem;
                                TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContext("Production")));
                                string comment = commentTextBox.Text;
                                //hier begint het, vanaf hier opsplitsen om de null'en te bekomen
                                float distance=0;
                                bool correctDistance = false;
                                if (distanceTextBox.Text != _OptionalMessage && distanceTextBox.Text != null)
                                {
                                    correctDistance = float.TryParse(distanceTextBox.Text, out distance);
                                }
                                bool correctSpeed = false;
                                float averageSpeed = 0;
                                if (distanceTextBox.Text != _OptionalMessage && kmPerHoursTextBox.Text != null)
                                {
                                    correctSpeed = float.TryParse(distanceTextBox.Text, out averageSpeed);
                                }

                                if (correctSpeed || correctDistance)
                                {
                                    //make sure the numbers actually match instead of just recalculating maybe?
                                    if (correctDistance)
                                    {
                                        averageSpeed = distance / (durationHours + (durationMinutes/60));
                                    }
                                    else
                                    {
                                        distance = averageSpeed * (durationHours + (durationMinutes / 60));
                                    }

                                    if (wattTextBox.Text != _OptionalMessage && wattTextBox != null)
                                    {
                                        int watt;
                                        bool correctWattage = int.TryParse(wattTextBox.Text, out watt);
                                        if (correctWattage)
                                        {
                                            m.AddCyclingTraining(startTime, distance, duration, averageSpeed, watt, trainingType, comment, bikeType);
                                        }
                                        else
                                        {
                                            ShowWarningTextBox("Incorrect input in Wattage: needs to be a number");
                                        }
                                    }
                                    else
                                        m.AddCyclingTraining(startTime, distance, duration, averageSpeed, null, trainingType, comment, bikeType);
                                }
                                else if(wattTextBox.Text != _OptionalMessage && wattTextBox.Text!=null)
                                {
                                    int watt;
                                    bool correctWattage = int.TryParse(wattTextBox.Text, out watt);
                                    if (correctWattage)
                                    {
                                        m.AddCyclingTraining(startTime, null, duration, null, watt, trainingType, comment, bikeType);
                                    }
                                    else
                                    {
                                        ShowWarningTextBox("Incorrect input in Wattage: needs to be a number");
                                    }
                                }
                                else
                                    //sowieso in try catch plaatsen voor de DomainExceptions een messagebox tonen, anders gewoon laten crashen.
                                    m.AddCyclingTraining(startTime, null, duration, null, null, trainingType, comment, bikeType);
                            }
                            else
                            {
                                ShowWarningTextBox("No BikeType was selected");
                            }
                        }
                        else
                        {
                            ShowWarningTextBox("No Training Type was selected");
                        }

                    }
                    else
                    {
                        ShowWarningTextBox("Incorrect input in Duration: Hours needs to be a number between 0 and 24 and minutes between 0 and 60");
                    }

                }
                else
                {
                    ShowWarningTextBox("Incorrec6t input in Start Time: Hours needs to be a number between 0 and 24 and minutes between 0 and 60");
                }

            }
            else
            {
                ShowWarningTextBox("Date was not Selected");
            }


        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox.Text == _OptionalMessage)
            {
                textbox.Text = "";
            }
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox.Text == "")
            {
                textbox.Text = _OptionalMessage;
            }
        }
    }
}
