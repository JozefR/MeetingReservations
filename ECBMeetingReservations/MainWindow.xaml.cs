﻿using DataRepository;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ECBMeetingReservations
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MeetingCentre _meetingCentreModel = null;
        private MeetingRoom _roomModel = null;
        private MeetingReservation _reservation = null;

        public ListBox RefreshCentres { get; set; }
        public ListBox RefreshRooms { get; set; }


        public MainWindow()
        {
            // Determine if the app is first time started and load saved data from previous use.
            if (HandleState.SecondLoading)
                return;
            else
            {
                LoadData.LoadMeetingDataFromFile(@"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservation\MeetingLibrary\ExportData.csv");
                LoadDataXML.LoadXML(@"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservation\DataRepository\Reservations.xml");
                HandleState.FirstLoading();
            }

            InitializeComponent();
            showCentresInListBox();
            RefreshCentres = meetingCentresListBox;
            RefreshRooms = meetingRoomsListBox;
        }



        // 2. Menu nav with Import data, save, exit buttons

        //2.1 Import data button

        private void importDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ImportDataUI importDataUI = new ImportDataUI();
            importDataUI.Show();
        }

        //2.2 Save button

        private void saveDataBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveData.CreateFileWithData();
            // Data was already saved... 
            HandleState.ChangingDataToFalse();
        }

        //2.3 exit button
        private void exitDataBtn_Click(object sender, RoutedEventArgs e)
        {
            // If data was already saved exit app if not pop up message.
            if (HandleState.DataChanged)
            {
                if (MessageBox.Show("Do you really want to save all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    SaveData.CreateFileWithData();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
 
        // 4. Show Centre rooms in listBox.

        private void meetingCentresListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            showRoomsInListBox();
        }

        private void showRoomsInListBox()
        {
            var item = meetingCentresListBox.SelectedItem;
            var rooms = new ObservableCollection<MeetingRoom>();

            foreach (var centre in DataManager.Centres)
            {
                if (centre == item)
                {
                    rooms = centre.MeetingRooms;
                }
            }

            meetingRoomsListBox.ItemsSource = rooms;
        }

        // 5. Create new Meeting centre 

        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            ECBCentreForm centreForm = new ECBCentreForm();
            centreForm.Show();
            centreForm.refreshListCentres(RefreshCentres);
        }

        // 6. Edit meeting centers

        private void editMeegingButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                meetingCentresListBox.Items.Refresh();
                _meetingCentreModel = meetingCentresListBox.SelectedItem as MeetingCentre;
                ECBCentreForm centreForm = new ECBCentreForm();
                centreForm.centreFormEdit(_meetingCentreModel, RefreshCentres);
                centreForm.Show();
            }
            
        }

        //7. create new room 

        private void newRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                ECBRoomForm ecbRoom = new ECBRoomForm();
                ecbRoom.Show();
                _meetingCentreModel = meetingCentresListBox.SelectedItem as MeetingCentre;
                ecbRoom.roomFormNew(_meetingCentreModel);
                ecbRoom.refreshListRooms(RefreshRooms);
            }
            else
            {
                MessageBox.Show("Please first select the center where you want to create your room.");
            }
        }

        //8. edit existing rooom

        private void editRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingRoomsListBox.SelectedItem != null)
            {
                meetingRoomsListBox.Items.Refresh();
                _roomModel = meetingRoomsListBox.SelectedItem as MeetingRoom;
                ECBRoomForm roomForm = new ECBRoomForm();
                roomForm.roomFormEdit(_roomModel, RefreshRooms);
                roomForm.Show();
                roomForm.refreshListRooms(RefreshRooms);
                showRoomsInListBox();
            }
        }

        // 9.1 delete center
        private void deleteMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really want to delete selected Centre?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                  == MessageBoxResult.Yes)
                {
                    DataManager.Centres.Remove(meetingCentresListBox.SelectedItem as MeetingCentre);
                    HandleState.ChangingData();
                }
            }
        }
        //9.2 delete room

        private void deleteRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingRoomsListBox.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really want to delete selected room?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                  == MessageBoxResult.Yes)
                {
                    foreach (var center in DataManager.Centres)
                    {
                        foreach (var room in center.MeetingRooms)
                        {
                            if(room == meetingRoomsListBox.SelectedItem)
                            {
                                center.MeetingRooms.Remove(meetingRoomsListBox.SelectedItem as MeetingRoom);
                                DataManager.Rooms.Remove(meetingRoomsListBox.SelectedItem as MeetingRoom);
                                break;
                            }
                        }
                    }
                }
            }
        }

        ////////////////////////////////////////////
        //////////////////////////////////druhy ukol
        ////////////////////////////////////////////

        /// <summary>
        /// Create new Meeting Reservation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewPlanningButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxDateValidation(MeetingRoomCombo,ReservationDatePicker))
            {
                PlanningForm planningForm = new PlanningForm();
                planningForm.Show();
                planningForm.TransferDataForReservation(MeetingRoomCombo, ReservationDatePicker);
            }
        }


        private void EditPlanningButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (ReservationsListBox.SelectedItem != null)
                {
                    _reservation = ReservationsListBox.SelectedItem as MeetingReservation;
                    PlanningForm planningForm = new PlanningForm();
                    planningForm.reservationForEdit(_reservation);
                    planningForm.TransferDataForReservation(MeetingRoomCombo, ReservationDatePicker);
                    planningForm.Show();
                }
                
        }


        /// <summary>
        /// Show all rooms in selected center.
        /// </summary>
        private void showRoomsInComboBox()
        {
            var item = MeetingCentreCombo.SelectedItem;

            var rooms = new ObservableCollection<MeetingRoom>();

            foreach (var centre in DataManager.Centres)
            {
                if (centre == item)
                {
                    rooms = centre.MeetingRooms;
                }
            }

            MeetingRoomCombo.ItemsSource = rooms;
        }

        private void MeetingCentreCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            showRoomsInComboBox();
        }

        /// <summary>
        /// Show centre entities in listbox and combobox.
        /// </summary>
        private void showCentresInListBox()
        {
            meetingCentresListBox.ItemsSource = DataManager.Centres;
            MeetingCentreCombo.ItemsSource = DataManager.Centres;

        }

        private void ReservationDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            showReservationsInListBox();
        }

        /// <summary>
        /// Show reservations in list box
        /// </summary>
        public void showReservationsInListBox()
        {
            DataManager.sort();
            var item = MeetingRoomCombo.SelectedItem;
            var reservations = new ObservableCollection<MeetingReservation>();

            foreach (var room in DataManager.Rooms)
            {
                foreach (var reservation in room.MeetingReservations)
                {
                    if (room == item)
                    {
                        if (ReservationDatePicker.SelectedDate == reservation.Date)
                            reservations.Add(reservation);
                    }
                }
            }
            ReservationsListBox.ItemsSource = reservations;
        }

        /// <summary>
        /// Validation for comboBox and DatePicker
        /// </summary>
        /// <param name="meetingCombo"></param>
        /// <param name="reservationDatePicker"></param>
        /// <returns></returns>
        private bool listBoxDateValidation(ComboBox meetingCombo, DatePicker reservationDatePicker)
        {
            if (meetingCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select room.");
                return false;
            }
            else if (reservationDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select some date.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ReservationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Save meetings to xml file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveDataReserBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveDataXML.WriteXML();
            HandleState.ChangingDataToFalse();
        }

        private void exitDataReserBtn_Click(object sender, RoutedEventArgs e)
        {
            // If data was already saved exit app if not pop up message.
            if (HandleState.DataChanged)
            {
                if (MessageBox.Show("Do you really want to save all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    SaveDataXML.WriteXML();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Delete Reservation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePlanningButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsListBox.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really want to delete selected reservation?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                  == MessageBoxResult.Yes)
                {
                    foreach (var room in DataManager.Rooms)
                    {
                        foreach (var reservation in room.MeetingReservations)
                        {
                            if (reservation == ReservationsListBox.SelectedItem)
                            {
                                room.MeetingReservations.Remove(reservation);
                                DataManager.Reservation.Remove(reservation);
                                HandleState.ChangingData();
                                showReservationsInListBox();
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportDataJson exportData = new ExportDataJson();
            exportData.Show();
        }
    }
}
