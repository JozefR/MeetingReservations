namespace Models
{
    public class MeetingRoomModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string VideoConference { get; set; }
        public MeetingCentreModel MeetingCentre { get; set; }

        public MeetingRoomModel(string name, string code, string description, int capacity, string video, MeetingCentreModel meetingCentreModel)
        {
            Name = name;
            Code = code;
            Description = description;
            Capacity = capacity;
            VideoConference = video;
            MeetingCentre = meetingCentreModel;
        }

        public string getName
        {
            get
            {
                return string.Format("{0} {1}", Name, Code);
            }
        }
    }
}
