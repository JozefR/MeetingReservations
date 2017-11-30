using Models;
using System;
using System.Xml;

namespace DataRepository
{
    public static class LoadDataXML
    {
        private static MeetingReservation _meetingReservation = new MeetingReservation();

        /// <summary>
        /// Load data from xml after starting the application
        /// </summary>
        /// <param name="path"></param>
        public static void LoadXML(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Reservation");

            foreach (XmlNode node in nodeList)
            {
                string roomName = node.SelectSingleNode("MeetingRoomName").InnerText;
                _meetingReservation.Customer = node.SelectSingleNode("Customer").InnerText;
                _meetingReservation.Date = DateTime.Parse(node.SelectSingleNode("Date").InnerText);
                _meetingReservation.TimeFrom = TimeSpan.Parse(node.SelectSingleNode("TimeFrom").InnerText);
                _meetingReservation.TimeTo = TimeSpan.Parse(node.SelectSingleNode("TimeTo").InnerText);
                _meetingReservation.ExpectedPersonsCount = int.Parse(node.SelectSingleNode("ExpectedPersons").InnerText);
                _meetingReservation.VideoConference = bool.Parse(node.SelectSingleNode("VideoConference").InnerText);
                _meetingReservation.Note = node.SelectSingleNode("Note").InnerText;

                assingEachReservationToCorrectRoom(_meetingReservation, roomName);
            }
        }

        /// <summary>
        /// Validate the correct room to add the reservation
        /// </summary>
        /// <param name="meetingReservation"></param>
        /// <param name="roomName"></param>
        private static void assingEachReservationToCorrectRoom(MeetingReservation meetingReservation, string roomName)
        {
            foreach (var room in DataManager.Rooms)
            {
                if (room.Name == roomName)
                {
                    _meetingReservation.MeetingRoom = room;
                    DataManager.Reservation.Add(_meetingReservation);
                    room.MeetingReservations.Add(_meetingReservation);
                }
            }
        }
    }
}
