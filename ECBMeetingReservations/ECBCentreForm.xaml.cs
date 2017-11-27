using System.Windows;
using System;
using Models;
using DataRepository;
using System.Windows.Controls;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for ECBCentreForm.xaml
    /// </summary>
    public partial class ECBCentreForm : Window
    {
        private MeetingCentre _centreModel = null;
        private ListBox _refreshData;

        public ECBCentreForm()
        {
            InitializeComponent();
        }

        private void okCentreFormBtn_Click(object sender, RoutedEventArgs e)
        {
            if (centreFormValidation() == false) ;
            else
            {
                newInputForm();
                editInputForm();
                HandleState.ChangingData();
                _refreshData.Items.Refresh();
                
            }
        }

        private void stornoCentreFormBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Handle the input data from user
        private void newInputForm()
        {
            
            if (_centreModel == null)
            {
                _centreModel = new MeetingCentre(nameFormTextBox.Text, codeFormTextBox.Text, descriptionFormTextBox.Text);
                DataManager.Centres.Add(_centreModel);
                this.Close();
            }
        }

        // 1. Get data from main ui after clicking on edit btn.
        internal void centreFormEdit(MeetingCentre centreModel, ListBox refresh)
        {
            _centreModel = centreModel;
            _refreshData = refresh;
            showSelectedCentreInEdit();
        }

        // 2. Show selected in edit form.
        private void showSelectedCentreInEdit()
        {
            nameFormTextBox.Text = _centreModel.Name;
            codeFormTextBox.Text = _centreModel.Code; 
            descriptionFormTextBox.Text = _centreModel.Description;
        }

        // 3
        private void updateSelectedForNew()
        {
            string name = nameFormTextBox.Text;
            string code = codeFormTextBox.Text;
            string description = descriptionFormTextBox.Text;
            updateSelected(name, code, description);
        }

        // 3. Update selected with new edit form.


        private void updateSelected(string name, string code, string description)
        {
            foreach (var centre in DataManager.Centres)
            {
                if (centre == _centreModel)
                {
                    centre.Name = name;
                    centre.Code = code;
                    centre.Description = description;
                    _centreModel.Name = name;
                    _centreModel.Code = code;
                    _centreModel.Description = description;
                }
            }

        }

        // 4. Handle the edit data from user
        private void editInputForm()
        {
            if (_centreModel != null)
            {
                updateSelectedForNew();
                this.Close();
            }
        }

        // centre form validation
        private bool centreFormValidation()
        {
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
            else
            {
                return true;
            }
        }

        internal void refreshListCentres(ListBox refreshCentres)
        {
            _refreshData = refreshCentres;
        }
    }
}
