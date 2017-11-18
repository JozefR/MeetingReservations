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
using System.Windows.Shapes;

namespace ECBMeetingReservations
{
    /// <summary>
    /// Interaction logic for ECBCentreForm.xaml
    /// </summary>
    public partial class ECBCentreForm : Window
    {
        public ECBCentreForm()
        {
            InitializeComponent();
        }

        public ECBCentreForm(string test)
        {
            InitializeComponent();
            formLabel.Text = test;
        }
    }
}
