using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MeetingLibrary
{
    public static class DataManager
    {
        public static ObservableCollection<MeetingCentreModel> Centres { get; set; } =
            new ObservableCollection<MeetingCentreModel>();

        public static ObservableCollection<MeetingRoomModel> Rooms { get; set; } = 
            new ObservableCollection<MeetingRoomModel>();

        // 1. Handle csv file and parse it to entities. 

        public static void LoadMeetingDataFromFile(string path)
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

            parseData(lines);
        }

        // 2. Validate the csv file. 
        private static void parseData(IEnumerable<string> lines)
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

        // 3. Assign room the correct centre
        private static void handleRoomsData(string[] splitLine)
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

        private static void assignRoomToCentre(string name,
            string code,
            string description,
            int capacity, 
            string video,
            string centreRoomCode)
        {
            foreach (var centre in DataManager.Centres)
            {
                if (centre.Code == centreRoomCode)
                {
                    DataManager.Rooms.Add(new MeetingRoomModel(name, code, description, capacity, video, centre));
                }
            }
        }

        //4. List all rooms and assign each room to correct centre
        private static void handleCentresData(string[] splitLine)
        {
            string name = splitLine[0];
            string code = splitLine[1];
            string description = splitLine[2];

            DataManager.Centres.Add(new MeetingCentreModel(name, code, description));
        }

        private static void assignRoomsToCentre()
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

    }
}
