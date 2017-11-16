using System.Collections.Generic;

namespace MeetingLibrary
{
    public class MeetingCentreModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<MeetingRoomModel> MeetingRooms { get; set; }

        public MeetingCentreModel()
        {
            MeetingRooms = new List<MeetingRoomModel>();
        }
    }

    public class MeetingRoomModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public bool VideoConference { get; set; }
        public MeetingCentreModel MeetingCentre { get; set; }
    }
}
