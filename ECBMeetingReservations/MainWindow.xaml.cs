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
            var item = meetingCentresListBox.SelectedItem;

            var selectedRooms = new ObservableCollection<MeetingRoomModel>();

            foreach (var centre in DataManager.Centres)
            {
                if (centre == item)
                {
                    selectedRooms = centre.MeetingRooms;
                }
            }

            meetingRoomsListBox.ItemsSource = selectedRooms;
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
        }



        private void newRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            ECBRoomForm ecbRoom = new ECBRoomForm();
            ecbRoom.Show();
        }

        private void editRoomsButton_Click(object sender, RoutedEventArgs e)
        {

        }



        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            meetingCentresListBox.Items.Refresh();
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

        }
    }
}
