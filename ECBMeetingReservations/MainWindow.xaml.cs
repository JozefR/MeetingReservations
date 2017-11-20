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

        // 1. Handle csv file and parse it to entities. 

        public void LoadMeetingDataLinq(string path)
        {
            var lines = new List<string>();

            if (path != null)
            {
                using (StreamReader reader = new StreamReader($"{path}"))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        lines.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }

            else
            {
                MessageBox.Show("I don't have any file to show data.");
            }

            //lines = File.ReadLines($@"{path}");
            parseData(lines);
        }

        private void parseData(IEnumerable<string> lines)
        {
            bool centres = false;
            bool rooms = false;

            foreach (var line in lines)
            {
                var splitLine = line.Split(',');

                if (splitLine[0] == "MEETING_CENTRES")
                {
                    centres = true;
                    continue;
                }

                if (splitLine[0] == "MEETING_ROOMS")
                {
                    rooms = true;
                    centres = false;
                    continue;
                }

                if (centres == true)
                {
                    handleCentresData(splitLine);
                }
                if (rooms == true)
                {
                    handleRoomsData(splitLine);
                }
            }
            assignRoomsToCentre();
        }

        private void handleRoomsData(string[] splitLine)
        {
            string name = splitLine[0];
            string code = splitLine[1];
            string description = splitLine[2];
            int capacity = int.Parse(splitLine[3]);
            string video = splitLine[4];
            //navigation property
            string centreRoomCode = splitLine[5];

            assignRoomToCentre(name, code, description, capacity, video, centreRoomCode);
        }

        private void assignRoomToCentre(string name, string code, string description, int capacity, string video, string centreRoomCode)
        {
            foreach (var centre in DataManager.Centres)
            {
                if (centre.Code == centreRoomCode)
                {
                    DataManager.Rooms.Add(new MeetingRoomModel(name, code, description, capacity, video, centre));
                }
            }
        }

        private void handleCentresData(string[] splitLine)
        {
            string name = splitLine[0];
            string code = splitLine[1];
            string description = splitLine[2];

            DataManager.Centres.Add(new MeetingCentreModel(name, code, description));
        }

        private void assignRoomsToCentre()
        {
            foreach (var centre in DataManager.Centres)
            {
                foreach (var room in DataManager.Rooms)
                {
                    if (centre == room.MeetingCentre)
                    {
                        centre.MeetingRooms.Add(room);
                    }
                }
            }
        }

        // 2. Make Menu nav with Import data, save, exit buttons

        //2.1 Import data button

        private void importDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ImportDataUI importDataUI = new ImportDataUI();
            importDataUI.Show();
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
