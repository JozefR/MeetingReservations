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
        private ComboBox _meetingCombo;
        private DatePicker _reservationDatePicker;
        private MainWindow _transferMain;

        public PlanningForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            _meetingReservation = new MeetingReservation();

            //transfered properties
            _meetingReservation.Date = (DateTime)_reservationDatePicker.SelectedDate;
            _meetingReservation.MeetingRoom = (MeetingRoom)_meetingCombo.SelectedItem;

            if (PlanningFormValidation())
            {
                createNewReservation();
                HandleState.ChangingData();
                this.Close();
            }
        }

        private void StornoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Transfer comboBox and DatePicker data from MainWindow.
        /// </summary>
        internal void TransferDataForReservation(ComboBox meeetingCombo, DatePicker reservationDatePicker)
        {
            _meetingCombo = meeetingCombo;
            _reservationDatePicker = reservationDatePicker;
        }

        /// <summary>
        /// Create new reservation and add it to Datamanger.Reservations
        /// </summary>
        private void createNewReservation()
        {
            TimeSpan timeFrom = new TimeSpan(int.Parse(FromPlanHour.Text), int.Parse(FromPlanMinute.Text), 0);
            TimeSpan timeTo = new TimeSpan(int.Parse(ToPlanHour.Text), int.Parse(ToPlanMinute.Text), 0);

 
            _meetingReservation.TimeFrom = timeFrom;
            _meetingReservation.TimeTo = timeTo;
            _meetingReservation.ExpectedPersonsCount = int.Parse(ExpectedPersonsTextBox.Text);
            _meetingReservation.Customer = CustomerTextBox.Text;
            _meetingReservation.VideoConference = (bool)VideoCheckBox.IsChecked;
            _meetingReservation.Note = NoteTextBox.Text;
            DataManager.Reservation.Add(_meetingReservation);

            assignReservationToCorrectRoom(_meetingReservation);
        }

        /// <summary>
        /// Check if meetingReservation belong to correct room
        /// </summary>
        /// <param name="meetingReservation"></param>
        private void assignReservationToCorrectRoom(MeetingReservation meetingReservation)
        {

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
