using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Models
{
    public class MeetingCentreModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ObservableCollection<MeetingRoomModel> MeetingRooms { get; set; }

        public MeetingCentreModel()
        {

        }

        public MeetingCentreModel(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
            MeetingRooms = new ObservableCollection<MeetingRoomModel>();
        }

        public string getFullName
        {
            get
            {
                return string.Format("{0} {1}", Name, Code);
            }
        }
    }
}
