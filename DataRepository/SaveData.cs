using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public static class SaveData
    {
        //5. Create file with data

        public static void CreateFileWithData()
        {
            string path = @"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservation\MeetingLibrary\ExportData.csv";

            try
            {
                using (StreamWriter fs = File.CreateText(path))
                {
                    writeDataToFile(fs);
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.ToString());
            }
        }

        private static void writeDataToFile(StreamWriter fs)
        {
            foreach (var center in DataManager.Centres)
            {
                fs.WriteLine("MEETING_CENTRES");
                fs.WriteLine(string.Format("{0}, {1}, {2}", center.Name, center.Code, center.Description));

                fs.WriteLine("MEETING_ROOMS");

                foreach (var room in center.MeetingRooms)
                {
                    fs.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                        room.Name,
                        room.Code,
                        room.Description,
                        room.Capacity,
                        room.VideoConference,
                        center.Code));
                }
            }
        }
    }
}
