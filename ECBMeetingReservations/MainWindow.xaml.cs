using DataRepository;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ECBMeetingReservations
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MeetingCentre _meetingCentreModel = null;
        private MeetingRoom _roomModel = null;
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
 
        /// <summary>
        /// Show centre entities in listbox and combobox.
        /// </summary>
        private void showCentresInListBox()
        {
            meetingCentresListBox.ItemsSource = DataManager.Centres;
            MeetingCentreCombo.ItemsSource = DataManager.Centres;

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

        //////////////Druha cast ukolu

       
    }
}
