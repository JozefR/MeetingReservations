using MeetingLibrary;
using Microsoft.Win32;
using System.Windows;

namespace ECBMeetingReservations
{

    /// <summary>
    /// Interaction logic for ImportDataUI.xaml
    /// </summary>
    public partial class ImportDataUI : Window
    {
        private string _path;


        public ImportDataUI()
        {
            InitializeComponent();
        }


        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            _path = openFile();
        }

        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }

        private string openFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt/.csv";

            if (open.ShowDialog() == true)
            {
                string fileName = open.FileName;
                inputFileTextBox.Text = fileName;
            }
            else
            {
                MessageBox.Show("Something wrong with the file.");
            }

            return open.FileName;
        }


        private void saveFile()
        {
            if (_path != null)
            {
                DataManager.LoadMeetingDataFromFile(_path);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please first open the file!");
            }
        }
    }
}
