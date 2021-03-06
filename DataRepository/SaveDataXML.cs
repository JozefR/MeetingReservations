﻿using System.IO;
using System.Xml;

namespace DataRepository
{
    public static class SaveDataXML
    {
        /// <summary>
        /// Write reservations data to xml file
        /// </summary>
        public static void WriteXML()
        {
            using (XmlWriter writer = XmlWriter.Create(@"C:\Users\randj\Dropbox\NET\Projects\Meeting-Centres\MeetingReservation\DataRepository\Reservations.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Table");

                foreach (var reservation in DataManager.Reservation)
                {
                    writer.WriteStartElement("Reservation");
                    writer.WriteElementString("MeetingRoomName", reservation.MeetingRoom.Name);
                    writer.WriteElementString("Customer", reservation.Customer);
                    writer.WriteElementString("Date", reservation.Date.ToString());
                    writer.WriteElementString("TimeFrom", reservation.TimeFrom.ToString());
                    writer.WriteElementString("TimeTo", reservation.TimeTo.ToString());
                    writer.WriteElementString("ExpectedPersons", reservation.ExpectedPersonsCount.ToString());
                    writer.WriteElementString("VideoConference", reservation.VideoConference.ToString());
                    writer.WriteElementString("Note", reservation.Note);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
    }
}
