using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DataRepository
{
    public static class DataManager
    {
        public static ObservableCollection<MeetingCentreModel> Centres { get; set; } =
            new ObservableCollection<MeetingCentreModel>();

        public static ObservableCollection<MeetingRoomModel> Rooms { get; set; } = 
            new ObservableCollection<MeetingRoomModel>();


    }
}
