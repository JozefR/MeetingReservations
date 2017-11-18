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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ObservableCollection<MeetingCentreModel> _centres = new ObservableCollection<MeetingCentreModel>();
        private static readonly List<MeetingRoomModel> _rooms = new List<MeetingRoomModel>();

        public MainWindow()
        {
            InitializeComponent();
            showEntitiesInListBox();
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
                MessageBox.Show("I don't have any file to show data. Sorry :(");
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
            foreach (var centre in _centres)
            {
                if (centre.Code == centreRoomCode)
                {
                    _rooms.Add(new MeetingRoomModel(name, code, description, capacity, video, centre));
                }
            }
        }

        private void handleCentresData(string[] splitLine)
        {
            string name = splitLine[0];
            string code = splitLine[1];
            string description = splitLine[2];

            _centres.Add(new MeetingCentreModel(name, code, description));
        }

        // 2. Make Menu nav with Import data, save, exit buttons

        //2.1 Import data button

        private void importDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ImportDataUI importDataUI = new ImportDataUI();
            importDataUI.Show();

        }

        // 3. Show Centre and Rooms entities in listBox. 

        private void showEntitiesInListBox()
        {
            meetingCentresListBox.ItemsSource = _centres;
        }


        private void listMeetingCentre_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("test");
        }


        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editMeegingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newRoomsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editRoomsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
