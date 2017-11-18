using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ECBMeetingUI
{
    /// <summary>
    /// Interaction logic for ImportDataUI.xaml
    /// </summary>
    public partial class ImportDataUI : Window
    {
        private string _path;

        public string GetPathFromFile()
        {
            return _path;
        }

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


        private void saveFile()
        {
            if (_path != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.LoadMeetingDataLinq(_path);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please first open the file!");
            }
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

            return open.FileName;
        }
    }
}
