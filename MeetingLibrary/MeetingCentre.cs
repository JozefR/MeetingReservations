using MeetingLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Models
{
    public class MeetingCentre : MeetingTower
    {
        public ObservableCollection<MeetingRoom> MeetingRooms { get; set; }

        public MeetingCentre(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
            MeetingRooms = new ObservableCollection<MeetingRoom>();
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
