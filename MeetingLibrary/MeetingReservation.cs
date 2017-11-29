using Models;
using System;

namespace Models
{
    public class MeetingReservation
    {
        public MeetingRoom MeetingRoom { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int ExpectedPersonsCount { get; set; }
        public string Customer { get; set; }
        public bool VideoConference { get; set; }
        public string Note { get; set; }

        public string GetFullName
        {
            get
            {
                return string.Format("{0} {1}", Customer, TimeFrom);
            }
        }
    }
}
