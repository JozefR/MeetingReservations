using System.Windows;
using System.Collections.Generic;
using MeetingLibrary;
using System.IO;
using System;
using System.Linq;

namespace ECBMeetingUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MeetingCentreModel> _centres = new List<MeetingCentreModel>();
        private List<MeetingRoomModel> _rooms = new List<MeetingRoomModel>();

        public MainWindow()
        {
            InitializeComponent();
            LoadMeetingDataLinq();

        }

        // 1. Handle csv file and parse it to entities. 

        private void LoadMeetingDataLinq()
        {
            var lines = File.ReadLines
                (@"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservations\MeetingLibrary\ImportData.csv");

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

                if(splitLine[0] == "MEETING_ROOMS")
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

        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            ECBForm eCBForm = new ECBForm("my first test");
            //eCBForm.Show();
            foreach (var item in _rooms)
            {
                MessageBox.Show(item.MeetingCentre.Code);
            }
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
