using MeetingLibrary;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DataRepository
{
    public static class DataManager
    {
        public static ObservableCollection<MeetingCentre> Centres { get; set; } =
            new ObservableCollection<MeetingCentre>();

        public static ObservableCollection<MeetingRoom> Rooms { get; set; } =
            new ObservableCollection<MeetingRoom>();

        public static ObservableCollection<MeetingPlanning> Planning { get; set; } =
            new ObservableCollection<MeetingPlanning>();
    }
}