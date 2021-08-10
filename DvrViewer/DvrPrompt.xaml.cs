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
using DvrViewer.Data;
using DvrViewer.Shared;

namespace DvrViewer
{
    /// <summary>
    /// Interaction logic for DvrPrompt.xaml
    /// </summary>
    public partial class DvrPrompt : Window
    {
        public DvrInformation Configuration { get; set; }

        public DvrPrompt()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Network.IsValidIpAddress(Configuration.DvrHost))
            {
                MessageBox.Show("The entered IP address is not valid. Please check the input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                TextBoxHost.Focus();
                return;
            }

            if (!Network.CanPingIpAddress(Configuration.DvrHost))
            {
                MessageBox.Show("The entered IP address could not be reached. Please check to ensure that the device can be reached from this computer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                TextBoxHost.Focus();
                return;
            }
            
            if (!Network.IsTcpPortOpen(Configuration.DvrHost, 85))
            {
                MessageBox.Show("Could not connect to the default ports. Please check to ensure that the device can be reached from this computer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TextBoxHost.Focus();
                Configuration.DvrPort = 0;
                return;
            }

            Configuration.DvrPort = 8000;

            Device.DvrDevice = Configuration;

            if (!Device.AuthenticateUser())
            {
                MessageBox.Show("Could not connect to the device using the entered username and password. Please check the input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TextBoxUsername.Focus();
                return;
            }

            DialogResult = true;
        }
    }
}
