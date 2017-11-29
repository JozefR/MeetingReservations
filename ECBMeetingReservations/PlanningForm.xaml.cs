using System.Windows;
using Models;
using MeetingLibrary;
using DataRepository;
using System;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for PlanningForm.xaml
    /// </summary>
    public partial class PlanningForm : Window
    {
        private MeetingReservation _meetingReservation = null;

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
            if (_meetingReservation == null)
            {
                _meetingReservation = new MeetingReservation();
                _meetingReservation.Date = (DateTime)reservationDatePicker.SelectedDate;
                _meetingReservation.MeetingRoom = (MeetingRoom)meeetingCombo.SelectedItem;
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

            _meetingReservation.TimeFrom = timeFrom;
            _meetingReservation.TimeTo = timeTo;
            _meetingReservation.ExpectedPersonsCount = int.Parse(ExpectedPersonsTextBox.Text);
            _meetingReservation.Customer = CustomerTextBox.Text;
            _meetingReservation.VideoConference = (bool)VideoCheckBox.IsChecked;
            _meetingReservation.Note = NoteTextBox.Text;
            DataManager.Reservation.Add(_meetingReservation);
            foreach (var room in DataManager.Rooms)
            {
                if (room == _meetingReservation.MeetingRoom)
                {
                    room.MeetingReservations.Add(_meetingReservation);
                }
            }
        }

        /// <summary>
        /// Validation for Planning form.
        /// </summary>
        /// <returns></returns>
        private bool PlanningFormValidation()
        {


            if (!int.TryParse(FromPlanHour.Text, out int TimeFromHour))
            {
                MessageBox.Show("Input time must be a number");
                return false;
            }
            if (!int.TryParse(FromPlanMinute.Text, out int timeFromMinute))
            {
                MessageBox.Show("time must be a number");
                return false;
            }
            if (!int.TryParse(ToPlanHour.Text, out int timeTohour))
            {
                MessageBox.Show("time must be a number");
                return false;
            }
            if (!int.TryParse(ToPlanMinute.Text, out int timeToMinute))
            {
                MessageBox.Show("time must be a number");
                return false;
            }

            // validate hours and minutes

            //string pattern = @"([0-9]{0,2}\:[0-9]{0,2})";

            /*
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
            */

            string format = "HH:mm";

            string dateFrom = string.Format("{0}:{1}", TimeFromHour, timeFromMinute);

            if (!DateTime.TryParseExact(dateFrom, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeFrom))
            {
                MessageBox.Show("Please put correct hour and minute!");
                return false;
            }

            string dateTo = string.Format("{0}:{1}", timeTohour, timeToMinute);

            if (!DateTime.TryParseExact(dateTo, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeTo))
            {
                MessageBox.Show("Please put correct hour and minute!");
                return false;
            }

            else if (!int.TryParse(ExpectedPersonsTextBox.Text, out int persons))
            {
                MessageBox.Show("Expected persons must be a number!");
                return false;
            }
            else if (persons > _meetingReservation.MeetingRoom.Capacity)
            {
                MessageBox.Show($"Too many persons! Maximum {_meetingReservation.MeetingRoom.Capacity.ToString()}");
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
