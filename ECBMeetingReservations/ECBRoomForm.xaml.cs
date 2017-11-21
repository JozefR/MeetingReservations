﻿using System.Windows;
using System;
using DataRepository;
using Models;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for ECBRoomForm.xaml
    /// </summary>
    public partial class ECBRoomForm : Window
    {
        private MeetingCentreModel _centreModel = null;
        private MeetingRoomModel _meetingRoom = null;

        public ECBRoomForm()
        {
            InitializeComponent();
        }


        // Handle the input data from user


        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (centreFormValidation() == false) ;
            else
            {
                newInputForm();
                editInputForm();
            }
        }

        private void stornoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void roomFormNew(MeetingCentreModel meetingCentreModel)
        {
            _centreModel = meetingCentreModel;
        }

        // 1. New room in selected MeetingCenter.
        private void newInputForm()
        {
            if (_meetingRoom == null)
            {
                _meetingRoom = new MeetingRoomModel
                    (nameFormTextBox.Text,
                    codeFormTextBox.Text,
                    descriptionFormTextBox.Text,
                    int.Parse(capacityFormTextBox.Text),
                    videoFormTextBox.Text,
                    _centreModel);

                DataManager.Rooms.Add(_meetingRoom);

                foreach (var centre in DataManager.Centres)
                {
                    if (centre == _meetingRoom.MeetingCentre)
                    {
                        centre.MeetingRooms.Add(_meetingRoom);
                    }
                }
                this.Close();
            }
        }


        // 2. Show selected room for edit
        internal void roomFormEdit(string text1, string text2, string text3, int v, string text4, MeetingRoomModel roomModel)
        {
            _meetingRoom = roomModel;
            showCelectedRoomInEdit();

        }

        private void showCelectedRoomInEdit()
        {
            nameFormTextBox.Text = _meetingRoom.Name;
            codeFormTextBox.Text = _meetingRoom.Code;
            descriptionFormTextBox.Text = _meetingRoom.Description;
            capacityFormTextBox.Text = _meetingRoom.Capacity.ToString();
            videoFormTextBox.Text = _meetingRoom.VideoConference;
        }

        // 3. update selected room

        private void updateSelectedForNew()
        {
            string name = nameFormTextBox.Text;
            string code = codeFormTextBox.Text;
            string description = descriptionFormTextBox.Text;
            int capacity = int.Parse(capacityFormTextBox.Text);
            string video = videoFormTextBox.Text;
            updateSelected(name, code, description, capacity, video);
        }

        // 4. Update selected with new edit form.

        private void updateSelected(string name, string code, string description, int capacity, string video)
        {
            foreach (var room in DataManager.Rooms)
            {
                if (room == _meetingRoom)
                {
                    room.Name = name;
                    room.Code = code;
                    room.Description = description;
                    room.Capacity = capacity;
                    room.VideoConference = video;
                    _meetingRoom.Name = name;
                    _meetingRoom.Code = code;
                    _meetingRoom.Description = description;
                    _meetingRoom.VideoConference = video;
                    _meetingRoom.Capacity = capacity;
                }
            }
        }

        // 5. Handle the edit data from user
        private void editInputForm()
        {
            if (_meetingRoom != null)
            {
                updateSelectedForNew();
                this.Close();
            }
        }

        // centre form validation
        private bool centreFormValidation()
        {
            int a = 0;
            if (nameFormTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the name.");
                return false;
            }
            else if (codeFormTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the code.");
                return false;
            }
            else if (descriptionFormTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the description.");
                return false;
            }
            else if (videoFormTextBox.Text != "YES" && videoFormTextBox.Text != "NO")
            {
                MessageBox.Show("Please select YES or NO for video.");
                return false;
            }
            else if (!int.TryParse(capacityFormTextBox.Text, out a))
            {
                MessageBox.Show("Please enter some number in capacity.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
