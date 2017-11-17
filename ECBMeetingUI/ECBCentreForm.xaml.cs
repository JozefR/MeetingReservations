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

namespace ECBMeetingUI
{
    /// <summary>
    /// Interaction logic for ECBForm.xaml
    /// </summary>
    public partial class ECBForm : Window
    {
        public ECBForm()
        {
            InitializeComponent();
        }

        public ECBForm(string test)
        {
            InitializeComponent();
            formLabel.Text = test;
        }
    }
}
