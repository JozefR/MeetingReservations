using MeetingLibrary;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DataRepository
{
    public static class DataManager
    {
        public static ObservableCollection<MeetingCentre> Centres { get; set; } =
            new ObservableCollection<MeetingCentre>();

        public static ObservableCollection<MeetingRoom> Rooms { get; set; } =
            new ObservableCollection<MeetingRoom>();

        public static ObservableCollection<MeetingReservation> Reservation { get; set; } =
            new ObservableCollection<MeetingReservation>();

        public static void sort()
        {
            foreach (var room in Rooms)
            {
                room.MeetingReservations = room.MeetingReservations.OrderByDescending(r => r.GetTimeFromHour).Reverse().ToList();
            }
        }
    }
}