using MeetingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Models
{
    public class MeetingRoom : MeetingTower
    { 
        public int Capacity { get; set; }
        public string VideoConference { get; set; }
        public MeetingCentre MeetingCentre { get; set; }
        public List<MeetingReservation> MeetingReservations { get; set; }

        public MeetingRoom()
        {

        }

        public MeetingRoom(string name, string code, string description, int capacity, string video, MeetingCentre meetingCentreModel)
        {
            Name = name;
            Code = code;
            Description = description;
            Capacity = capacity;
            VideoConference = video;
            MeetingCentre = meetingCentreModel;
            MeetingReservations = new List<MeetingReservation>();
        }

        public override string GetFullName
        {
            get
            {
                return string.Format("{0} {1}", Name, Code);
            }
        }
    }
}
