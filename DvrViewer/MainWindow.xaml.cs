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
using DvrViewer.Data;
using DvrViewer.Shared;

namespace DvrViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void OnShowChannelsChangeDelegate();

        public static event OnShowChannelsChangeDelegate OnShowChannelsChange;

        private static bool _showChannels;

        public static bool ShowChannels
        {
            get => _showChannels;
            set
            {
                _showChannels = value;
                OnShowChannelsChange?.Invoke();
            }
        }

        private DvrInformation DvrConfiguration { get; set; }

        private DeviceInformation DeviceInfo { get; set; }

        private NetworkInformation NetworkInfo { get; set; }

        private List<ChannelInformation> Channels { get; set; } = new List<ChannelInformation>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            HCNetSDK.NET_DVR_Init();

            DvrConfiguration = Configuration.Configuration.LoadConfiguration<DvrInformation>();

            if (string.IsNullOrEmpty(DvrConfiguration.DvrHost))
            {
                MessageBox.Show("The application needs to be configured. Click OK to begin the configuration process.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                DvrPrompt dvrPrompt = new DvrPrompt();
                dvrPrompt.Configuration = DvrConfiguration;

                bool? result = dvrPrompt.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    DvrConfiguration = dvrPrompt.Configuration;
                    Configuration.Configuration.SaveConfiguration(DvrConfiguration);
                    DvrConfiguration = Configuration.Configuration.LoadConfiguration<DvrInformation>();
                }
                else
                {
                    Close();
                    return;
                }
            }
            
            Device.DvrDevice = DvrConfiguration;

            if (!Device.AuthenticateUser())
            {
                if (MessageBox.Show("Could not connect to the device. Would you like to verify the configuration?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    DvrPrompt dvrPrompt = new DvrPrompt();
                    dvrPrompt.Configuration = DvrConfiguration;

                    bool? result = dvrPrompt.ShowDialog();

                    if (result.HasValue && result.Value)
                    {
                        DvrConfiguration = dvrPrompt.Configuration;
                        Configuration.Configuration.SaveConfiguration(DvrConfiguration);
                    }
                    else
                    {
                        Close();
                        return;
                    }
                }
            }

            DeviceInfo = Device.GetDeviceInformation();
            NetworkInfo = Device.GetNetworkInformation();

            if (DeviceInfo == null)
            {
                MessageBox.Show("An error occurred while attempting to get the device information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Close();
                return;
            }

            if (NetworkInfo == null)
            {
                MessageBox.Show("An error occurred while attempting to get the device network information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Close();
                return;
            }

            for (int i = 1; i <= DeviceInfo.AnalogChannelCount; i++)
            {
                ChannelInformation channelInformation = Device.GetChannelInformation(i);

                if (channelInformation != null)
                {
                    Channels.Add(channelInformation);
                }
            }

            ListViewChannels.ItemsSource = Channels;

            OnShowChannelsChange += MainWindow_OnShowChannelsChange;
            ShowChannels = false;

            TextBlockStatus.Text = $"Connected to {DeviceInfo.DeviceName} {Device.DvrDevice.DvrHost}";
        }

        private void MainWindow_OnShowChannelsChange()
        {
            ColumnChannels.Width = ShowChannels ? GridLength.Auto : new GridLength(0);
        }

        private void MenuItemFileQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            HCNetSDK.NET_DVR_Cleanup();
        }
    }
}
