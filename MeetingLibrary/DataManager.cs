using System.Collections.ObjectModel;

namespace MeetingLibrary
{
    public static class DataManager
    {
        public static ObservableCollection<MeetingCentreModel> Centres { get; set; } = new ObservableCollection<MeetingCentreModel>();
        public static ObservableCollection<MeetingRoomModel> Rooms { get; set; } = new ObservableCollection<MeetingRoomModel>();

    }
}
