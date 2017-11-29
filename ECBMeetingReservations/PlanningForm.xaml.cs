using System.Windows;
using Models;
using MeetingLibrary;
using DataRepository;
using System;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for PlanningForm.xaml
    /// </summary>
    public partial class PlanningForm : Window
    {
        private MeetingPlanning _meetingPlanning = null;

        public PlanningForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlanningFormValidation())
            {
                NewInputFormOk();
                this.Close();
            }
        }

        private void StornoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Data for planning input form from main window
        /// </summary>
        internal void NewInputForm(ComboBox meeetingCombo, DatePicker reservationDatePicker)
        {
            if (_meetingPlanning == null)
            {
                _meetingPlanning = new MeetingPlanning();
                _meetingPlanning.Date = (DateTime)reservationDatePicker.SelectedDate;
                _meetingPlanning.MeetingRoom = (MeetingRoom)meeetingCombo.SelectedItem;
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Create new planning after click on ok button
        /// </summary>
        /// <returns></returns>
        private void NewInputFormOk()
        {
            TimeSpan timeFrom = new TimeSpan(int.Parse(FromPlanHour.Text), int.Parse(FromPlanMinute.Text),0);
            TimeSpan timeTo = new TimeSpan(int.Parse(ToPlanHour.Text), int.Parse(ToPlanMinute.Text), 0);

            _meetingPlanning.TimeFrom = timeFrom;
            _meetingPlanning.TimeTo = timeTo;
            _meetingPlanning.ExpectedPersonsCount = int.Parse(ExpectedPersonsTextBox.Text);
            _meetingPlanning.Customer = CustomerTextBox.Text;
            _meetingPlanning.VideoConference = (bool)VideoCheckBox.IsChecked;
            _meetingPlanning.Note = NoteTextBox.Text;
            DataManager.Planning.Add(_meetingPlanning);
        }

        /// <summary>
        /// Validation for Planning form.
        /// </summary>
        /// <returns></returns>
        private bool PlanningFormValidation()
        {
            string pattern = @"([0-9]{0,2}\:[0-9]{0,2})";

            if (!int.TryParse(FromPlanHour.Text, out int timeFrom))
            {
                MessageBox.Show("Input time must be a number");
                return false;
            }
            else if (!int.TryParse(FromPlanMinute.Text, out int timeFromMinute))
            {
                MessageBox.Show("selected time is not correct>");
                return false;
            }
            else if (!int.TryParse(ToPlanHour.Text, out int timeTohour))
            {
                MessageBox.Show("selected time is not correct>");
                return false;
            }
            else if (!int.TryParse(ToPlanMinute.Text, out int timeToMinute))
            {
                MessageBox.Show("selected time is not correct>");
                return false;
            }
            else if (!Regex.IsMatch(string.Format("{0}:{1}", timeFrom, timeFromMinute), pattern))
            {
                MessageBox.Show("selected time is not correct>");
                return false;
            }
            else if (!Regex.IsMatch(string.Format("{0}:{1}", timeTohour, timeToMinute), pattern))
            {
                MessageBox.Show("selected time is not correct>");
                return false;
            }
            else if (!int.TryParse(ExpectedPersonsTextBox.Text, out int persons))
            {
                MessageBox.Show("Expected persons must be a number!");
                return false;
            }
            else if (persons > _meetingPlanning.MeetingRoom.Capacity)
            {
                MessageBox.Show($"Too many persons! Maximum {_meetingPlanning.MeetingRoom.Capacity.ToString()}");
                return false;
            }
            else if (CustomerTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the customer name.");
                return false;
            }
            else if (NoteTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the description.");
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
