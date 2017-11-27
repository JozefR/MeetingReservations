using MeetingLibrary;

namespace Models
{
    public class MeetingRoom : MeetingTower
    { 
        public int Capacity { get; set; }
        public string VideoConference { get; set; }
        public MeetingCentre MeetingCentre { get; set; }

        public MeetingRoom(string name, string code, string description, int capacity, string video, MeetingCentre meetingCentreModel)
        {
            Name = name;
            Code = code;
            Description = description;
            Capacity = capacity;
            VideoConference = video;
            MeetingCentre = meetingCentreModel;
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
