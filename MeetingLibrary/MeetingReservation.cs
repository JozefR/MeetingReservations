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

        public MeetingReservation()
        {

        }

        public string GetFullName
        {
            get
            {
                return string.Format("{0} - {1} {2}", TimeFrom, TimeTo, Customer);
            }
        }

        public string GetTimeFromHour
        {
            get
            {
                return string.Format("{0}", TimeFrom.Hours);
            }
        }

        public string GetTimeFromMinute
        {
            get
            {
                return string.Format("{0}", TimeFrom.Minutes);
            }
        }

        public string GetTimeToHour
        {
            get
            {
                return string.Format("{0}", TimeTo.Hours);
            }
        }

        public string GetTimeToMinute
        {
            get
            {
                return string.Format("{0}", TimeTo.Minutes);
            }
        }

    }
}
