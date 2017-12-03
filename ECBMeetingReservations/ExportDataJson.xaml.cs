using DataRepository;
using Microsoft.Win32;
using System.Windows;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for ExportDataJson.xaml
    /// </summary>
    public partial class ExportDataJson : Window
    {
        private string _path;

        public ExportDataJson()
        {
            InitializeComponent();
        }

        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            _path = openFile();
        }

        private void exportFileBtn_Click(object sender, RoutedEventArgs e)
        {
            exportFile();
        }

        private string openFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".json";

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


        private void exportFile()
        {
            if (_path != null)
            {
                SaveDataJson.exportDataToJson(_path);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please first choose the file!");
            }
        }
    }
}
