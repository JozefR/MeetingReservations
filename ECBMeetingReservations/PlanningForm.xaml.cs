using System.Windows;
using Models;
using DataRepository;
using System;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for PlanningForm.xaml
    /// </summary>
    public partial class PlanningForm : Window
    {
        private MeetingReservation _meetingReservation = null;
        private MeetingReservation _meetingReservationTransfer;
        private ComboBox _meetingCombo;
        private DatePicker _reservationDatePicker;

        private int _a1;
        private int _a2;
        private int _b1;
        private int _b2;

        public PlanningForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlanningFormValidation() && validateTime())
            {
                if (_meetingReservation == null)
                {
                    _meetingReservation = new MeetingReservation();

                    //transfered properties
                    _meetingReservation.Date = (DateTime)_reservationDatePicker.SelectedDate;
                    _meetingReservation.MeetingRoom = (MeetingRoom)_meetingCombo.SelectedItem;

                    if (transformDataFromUI())
                    {
                        HandleState.ChangingData();
                        DataManager.sort();
                        ((MainWindow)Application.Current.MainWindow).showReservationsInListBox();
                        this.Close();
                    }
                }
                else
                {
                    updateSelectedForNew();
                    ((MainWindow)Application.Current.MainWindow).showReservationsInListBox();
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Update selected reservation to new.
        /// </summary>
        private void updateSelectedForNew()
        {
            foreach (var room in DataManager.Rooms)
            {
                foreach (var reservation in room.MeetingReservations)
                {
                    if (reservation == _meetingReservation)
                    {
                        TimeSpan timeFrom = new TimeSpan(int.Parse(FromPlanHour.Text), int.Parse(FromPlanMinute.Text), 0);
                        TimeSpan timeTo = new TimeSpan(int.Parse(ToPlanHour.Text), int.Parse(ToPlanMinute.Text), 0);

                        reservation.TimeFrom = timeFrom;
                        reservation.TimeTo = timeTo;
                        reservation.ExpectedPersonsCount = int.Parse(ExpectedPersonsTextBox.Text);
                        reservation.Customer = CustomerTextBox.Text;
                        reservation.VideoConference = (bool)VideoCheckBox.IsChecked;
                        reservation.Note = NoteTextBox.Text;
                    }
                }
            }
        }

        /// <summary>
        /// Transfer existing reservation to edit
        /// </summary>
        /// <param name="reservation"></param>
        internal void reservationForEdit(MeetingReservation reservation)
        {
            _meetingReservation = reservation;
            showCelectedRoomInEdit();
        }

        /// <summary>
        /// Show selected reservations in edit form.
        /// </summary>
        private void showCelectedRoomInEdit()
        {
            FromPlanHour.Text = _meetingReservation.TimeFrom.Hours.ToString();
            FromPlanMinute.Text = _meetingReservation.TimeFrom.Minutes.ToString();
            ToPlanHour.Text = _meetingReservation.TimeTo.Hours.ToString();
            ToPlanMinute.Text = _meetingReservation.TimeTo.Minutes.ToString();
            CustomerTextBox.Text = _meetingReservation.Customer;
            VideoCheckBox.IsChecked = _meetingReservation.VideoConference;
            ExpectedPersonsTextBox.Text = _meetingReservation.ExpectedPersonsCount.ToString();
            NoteTextBox.Text = _meetingReservation.Note;
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
            _meetingReservationTransfer = new MeetingReservation();
            _meetingReservationTransfer.MeetingRoom = (MeetingRoom)meeetingCombo.SelectedItem;
            _meetingCombo = meeetingCombo;
            _reservationDatePicker = reservationDatePicker;
        }

        /// <summary>
        /// Create new reservation and add it to Datamanger.Reservations
        /// </summary>
        private bool transformDataFromUI()
        {
            _a1 = int.Parse(FromPlanHour.Text) * 60 + int.Parse(FromPlanMinute.Text);
            _a2 = int.Parse(ToPlanHour.Text) * 60 + int.Parse(ToPlanMinute.Text);
            
            TimeSpan timeFrom = new TimeSpan(int.Parse(FromPlanHour.Text), int.Parse(FromPlanMinute.Text), 0);
            TimeSpan timeTo = new TimeSpan(int.Parse(ToPlanHour.Text), int.Parse(ToPlanMinute.Text), 0);

            _meetingReservation.TimeFrom = timeFrom;
            _meetingReservation.TimeTo = timeTo;
            _meetingReservation.ExpectedPersonsCount = int.Parse(ExpectedPersonsTextBox.Text);
            _meetingReservation.Customer = CustomerTextBox.Text;
            _meetingReservation.VideoConference = (bool)VideoCheckBox.IsChecked;
            _meetingReservation.Note = NoteTextBox.Text;

            if (createNewReservation())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Handle new reservation
        /// </summary>
        /// <returns></returns>
        private bool createNewReservation()
        {
            if (validateExistingReservations(_a1, _a2))
            {
                DataManager.Reservation.Add(_meetingReservation);
                assignReservationToCorrectRoom(_meetingReservation);
                return true;
            }
            else
            {
                MessageBox.Show("There is already reservation on that time!");
                return false;
            }
        }

        /// <summary>
        /// Validate time of existing reservations.
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        private bool validateExistingReservations(int a1, int a2)
        {
            foreach (var reservation in DataManager.Reservation)
            {
                if (_meetingReservation.Date == reservation.Date)
                {
                    _b1 = int.Parse(reservation.GetTimeFromHour) * 60 + int.Parse(reservation.GetTimeFromMinute);
                    _b2 = int.Parse(reservation.GetTimeToHour) * 60 + int.Parse(reservation.GetTimeToMinute);

                    if (((_a1 <= _b1) && (_b1 <= _a2)) ||
                            (_a1 <= _b2) && (_b2 <= _a1) ||
                            (_b1 <= _a1) && (_b2 >= _a2))
                    {
                        return false;
                    }
                }
            }
            return true;
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
            if (!int.TryParse(ExpectedPersonsTextBox.Text, out int persons))
            {
                MessageBox.Show("Expected persons must be a number!");
                return false;
            }
            else if (persons > _meetingReservationTransfer.MeetingRoom.Capacity)
            {
                MessageBox.Show($"Too many persons! Maximum {_meetingReservationTransfer.MeetingRoom.Capacity.ToString()}");
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

        /// <summary>
        /// validate input time
        /// </summary>
        /// <returns></returns>
        private bool validateTime()
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

            if (TimeFromHour > 23)
            {
                MessageBox.Show("Invalid Hour Error");
                return false;
            }

            // Minutes should be less than 60
            if (timeFromMinute > 59)
            {
                MessageBox.Show("Invalid Minute Error");
                return false;
            }

            if (timeTohour > 23)
            {
                MessageBox.Show("Invalid Hour Error");
                return false;
            }

            if (timeTohour < TimeFromHour)
            {
                MessageBox.Show("Invalid Hour Error");
                return false;
            }

            if (timeTohour == TimeFromHour)
                if (timeFromMinute >= timeToMinute)
            {
                MessageBox.Show("From time cannot be smaller as To time!");
                return false;
            }

            // Minutes should be less than 60
            if (timeToMinute > 59)
            {
                MessageBox.Show("Invalid Minute Error");
                return false;
            }

            string regex = @"\d{1,2}";

            if (!Regex.IsMatch(FromPlanHour.Text,regex))
            {
                MessageBox.Show("Only two digits allowed");
                return false;
            }

            if (!Regex.IsMatch(FromPlanMinute.Text, regex))
            {
                MessageBox.Show("Only two digits allowed");
                return false;
            }

            if (!Regex.IsMatch(ToPlanHour.Text, regex))
            {
                MessageBox.Show("Only two digits allowed");
                return false;
            }

            if (!Regex.IsMatch(ToPlanMinute.Text, regex))
            {
                MessageBox.Show("Only two digits allowed");
                return false;
            }

            return true;
        }


    }
}
