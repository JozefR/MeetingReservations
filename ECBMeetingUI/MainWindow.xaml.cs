using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECBMeetingUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Show meeting centres from csv file. 




        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            ECBForm eCBForm = new ECBForm("my first test");
            eCBForm.Show();
        }

        private void editMeegingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newRoomsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editRoomsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
