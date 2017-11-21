using MeetingLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ECBMeetingReservations
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private MeetingCentreModel _meetingCentreModel = null;
        private MeetingRoomModel _roomModel = null;


        public MainWindow()
        {
            InitializeComponent();
            showCentresInListBox();
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

        }

        // 3. Show Centre entities in listBox. 

        private void showCentresInListBox()
        {
            meetingCentresListBox.ItemsSource = DataManager.Centres;
        }

        // 4. Show Centre rooms in listBox.

        private void meetingCentresListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            showRoomsInListBox();
        }

        private void showRoomsInListBox()
        {
            var item = meetingCentresListBox.SelectedItem;

            var rooms = new ObservableCollection<MeetingRoomModel>();

            foreach (var centre in DataManager.Centres)
            {
                if (centre == item)
                {
                    rooms = centre.MeetingRooms;
                }
            }

            meetingRoomsListBox.ItemsSource = rooms;
        }

        private void listMeetingCentre_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("test");
        }

        // 5. Create new Meeting centre 

        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            ECBCentreForm centreForm = new ECBCentreForm();
            centreForm.Show();
        }

        // 6. Edit meeting centers

        private void editMeegingButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                meetingCentresListBox.Items.Refresh();
                _meetingCentreModel = meetingCentresListBox.SelectedItem as MeetingCentreModel;
                ECBCentreForm centreForm = new ECBCentreForm();
                centreForm.centreFormEdit(nameCentresTextBox.Text, codeCentresTextBox.Text, descriptionCentresTextBox.Text, _meetingCentreModel);
                centreForm.Show();   
            }
            refreshData();
        }



        private void newRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                ECBRoomForm ecbRoom = new ECBRoomForm();
                ecbRoom.Show();
                _meetingCentreModel = meetingCentresListBox.SelectedItem as MeetingCentreModel;
                ecbRoom.roomFormNew(_meetingCentreModel);
            }
            else
            {
                MessageBox.Show("Please first select the center where you want to create your room.");
            }
        }

        private void editRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingRoomsListBox.SelectedItem != null)
            {
                meetingRoomsListBox.Items.Refresh();
                _roomModel = meetingRoomsListBox.SelectedItem as MeetingRoomModel;
                ECBRoomForm roomForm = new ECBRoomForm();
                roomForm.roomFormEdit(nameRoomsTextBox.Text, codeRoomsTextBox.Text, descriptionRoomsTextBox.Text,int.Parse(capacityRoomsTextBox.Text),videoRoomsTextBox.Text, _roomModel);
                roomForm.Show();
                showRoomsInListBox();
            }
        }



        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void deleteMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                if (MessageBox.Show("Do you really want to delete selected Centre?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)
                  == MessageBoxResult.Yes)
                {
                    DataManager.Centres.Remove(meetingCentresListBox.SelectedItem as MeetingCentreModel);
                }
            }
        }

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
                                center.MeetingRooms.Remove(meetingRoomsListBox.SelectedItem as MeetingRoomModel);
                                DataManager.Rooms.Remove(meetingRoomsListBox.SelectedItem as MeetingRoomModel);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void refreshData()
        {
            meetingCentresListBox.Items.Refresh();
            meetingRoomsListBox.Items.Refresh();
        }


    }
}
