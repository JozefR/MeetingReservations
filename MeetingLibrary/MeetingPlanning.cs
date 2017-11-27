using Models;
using System;

namespace MeetingLibrary
{
    class MeetingPlanning
    {
        public MeetingRoom MeetingRoom { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int ExpectedPersonsCount { get; set; }
        public string Customer { get; set; }
        public bool VideoConference { get; set; }
        public string Note { get; set; }
    }
}
