﻿using System.Collections.Generic;

namespace MeetingLibrary
{
    public class MeetingCentreModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<MeetingRoomModel> MeetingRooms { get; set; }

        public MeetingCentreModel(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
            MeetingRooms = new List<MeetingRoomModel>();
        }
    }
}
