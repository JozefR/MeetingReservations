using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataRepository
{
    public static class SaveDataJson
    {
        private static List<DateTime> _dates;

        public static void exportDataToJson(string path)
        {
            JSONToFile(ToJSONFormat(path));
        }

        private static string ToJSONFormat(string path)
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter jw = new JsonTextWriter(new StringWriter(sb));

            var reservations = DataManager.Reservation;

            jw.Formatting = Formatting.Indented;
            jw.WriteStartObject();
            jw.WritePropertyName("schema");
            jw.WriteValue("PLUS4U.EBC.MCS.MeetingRoom_Schedule_1.0");
            jw.WritePropertyName("uri");
            jw.WriteValue("ues:UCL-BT:UCL.INF/DEMO_REZERVACE:EBC.MCS.DEMO/MR001/SCHEDULE");
            jw.WritePropertyName("data");
            jw.WriteStartArray();

            foreach (var centre in DataManager.Centres)
            {
                jw.WriteStartObject();
                jw.WritePropertyName("meetingCentre:");
                jw.WriteValue(centre.GetFullName);

                foreach (var room in centre.MeetingRooms)
                {
                    jw.WritePropertyName("meetingRoom:");
                    jw.WriteValue(room.GetFullName);
                    jw.WritePropertyName("reservations");
                    jw.WriteStartObject();

                    foreach (var reser in room.MeetingReservations)
                    {

                        jw.WritePropertyName(string.Format("{0}", reser.Date.ToShortDateString()));

                        jw.WriteStartArray();

                        jw.WriteStartObject();

                        jw.WritePropertyName("from");
                        jw.WriteValue(reser.TimeFrom);
                        jw.WritePropertyName("to");
                        jw.WriteValue(reser.TimeTo);
                        jw.WritePropertyName("expectedPersonsCount");
                        jw.WriteValue(reser.ExpectedPersonsCount);
                        jw.WritePropertyName("customer");
                        jw.WriteValue(reser.Customer);
                        jw.WritePropertyName("videoConference");
                        jw.WriteValue(reser.VideoConference);
                        jw.WritePropertyName("note");
                        jw.WriteValue(reser.Note);

                        jw.WriteEndObject();

                        jw.WriteEndArray();
                    }

                    jw.WriteEndObject();
                }

                jw.WriteEndObject();
            }


            jw.WriteEndArray();
            jw.WriteEndObject();

            return sb.ToString();
        }

        private static void JSONToFile(string format)
        {
            File.WriteAllText(@"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservation\DataRepository\exportData.json", format);
        }
        /*
         *                             jw.WritePropertyName("from");
                            jw.WriteValue(room.Name);
                            jw.WritePropertyName("to");
                            jw.WriteValue(room.VideoConference);

                            /*
                            jw.WritePropertyName("from");
                            jw.WriteValue(reser.TimeFrom);
                            jw.WritePropertyName("to");
                            jw.WriteValue(reser.TimeTo);
                            jw.WritePropertyName("expectedPersonsCount");
                            jw.WriteValue(reser.ExpectedPersonsCount);
                            jw.WritePropertyName("customer");
                            jw.WriteValue(reser.Customer);
                            jw.WritePropertyName("videoConference");
                            jw.WriteValue(reser.VideoConference);
                            jw.WritePropertyName("note");
                            jw.WriteValue(reser.Note);
                            */
    }
}
