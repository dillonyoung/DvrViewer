using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DvrViewer.Data;
using DvrViewer.Enum;
using DvrViewer.Shared;

namespace DvrViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Preferences CurrentPreferences { get; set; } = new Preferences();

        public static ObservableCollection<ViewLayout> ViewLayouts { get; set; }

        private DvrInformation DvrConfiguration { get; set; }

        private DeviceInformation DeviceInfo { get; set; }

        private NetworkInformation NetworkInfo { get; set; }

        private List<ChannelInformation> Channels { get; set; } = new List<ChannelInformation>();

        private VideoOutputControl.VideoOutputControl[] VideoOutputControls;

        private VideoOutputInformation SelectedDisplayOutput { get; set; }

        private ObservableCollection<VideoOutputInformation> VideoOutputs { get; set; } = new ObservableCollection<VideoOutputInformation>();
        
        public MainWindow()
        {
            ViewLayouts = new ObservableCollection<ViewLayout>();

            foreach (ViewLayoutTypes viewLayoutType in System.Enum.GetValues(typeof(ViewLayoutTypes)))
            {
                DescriptionAttribute descriptionAttribute = viewLayoutType.GetType().GetField(System.Enum.GetName(viewLayoutType.GetType(), viewLayoutType)).GetCustomAttribute<DescriptionAttribute>();

                ViewLayout checkedItem = new ViewLayout
                {
                    IsChecked = false,
                    LayoutType = viewLayoutType,
                    Title = descriptionAttribute == null ? viewLayoutType.ToString() : descriptionAttribute.Description
                };

                ViewLayouts.Add(checkedItem);
            }

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

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    VideoOutputInformation videoOutputInformation = new VideoOutputInformation();
                    videoOutputInformation.VideoStatus = VideoStatuses.Stopped;
                    videoOutputInformation.VideoStreamId = -1;
                    videoOutputInformation.Row = i;
                    videoOutputInformation.Column = j;
                    VideoOutputs.Add(videoOutputInformation);
                }
            }

            CurrentPreferences.OnShowChannelsChange += MainWindow_OnShowChannelsChange;
            CurrentPreferences.OnViewLayoutChange += MainWindow_OnViewLayoutChange;

            InitializeVideoEventHandling();
            InitializeViewList();

            TextBlockStatus.Text = $"Connected to {DeviceInfo.DeviceName} {Device.DvrDevice.DvrHost}";
        }

        private void MainWindow_OnViewLayoutChange()
        {
            foreach (ViewLayout viewLayout in MenuItemViewOutputArea.Items)
            {
                viewLayout.IsChecked = viewLayout.LayoutType == CurrentPreferences.ViewLayout;
            }

            WindowsFormsHost[] windowsFormsHosts =
            {
                WindowsFormsHost1, WindowsFormsHost2, WindowsFormsHost3, WindowsFormsHost4, WindowsFormsHost5, 
                WindowsFormsHost6, WindowsFormsHost7, WindowsFormsHost8, WindowsFormsHost9
            };
            
            ViewLayout currentViewLayout = ViewLayouts.First(x => x.LayoutType == CurrentPreferences.ViewLayout);

            GridViewLayout.ColumnDefinitions.Clear();
            for (int i = 0; i < (currentViewLayout.ColumnCount == 0 ? 1 : currentViewLayout.ColumnCount); i++)
            {
                GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
            }

            GridViewLayout.RowDefinitions.Clear();
            for (int i = 0; i < (currentViewLayout.RowCount == 0 ? 1 : currentViewLayout.RowCount); i++)
            {
                GridViewLayout.RowDefinitions.Add(new RowDefinition());
            }

            foreach (WindowsFormsHost windowsFormsHost in windowsFormsHosts)
            {
                DisplayChannel displayChannel = currentViewLayout.Channels.FirstOrDefault(x => ReferenceEquals(x.HostControl, windowsFormsHost));

                if (displayChannel == null)
                {
                    windowsFormsHost.Visibility = Visibility.Hidden;
                }
                else
                {
                    windowsFormsHost.Visibility = displayChannel.Visible ? Visibility.Visible : Visibility.Hidden;
                    Grid.SetColumn(windowsFormsHost, displayChannel.Column);
                    Grid.SetRow(windowsFormsHost, displayChannel.Row);
                }
            }
        }

        private void MainWindow_OnShowChannelsChange()
        {
            ColumnChannels.Width = CurrentPreferences.ShowChannels ? GridLength.Auto : new GridLength(0);
        }

        private void MenuItemFileQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            HCNetSDK.NET_DVR_Cleanup();
            
            Configuration.Configuration.SaveConfiguration(CurrentPreferences);
        }

        private void MenuItemViewOutputArea_OnClick(object sender, RoutedEventArgs e)
        {
            ViewLayout originalViewLayout = (ViewLayout)((MenuItem) e.OriginalSource).DataContext;

            CurrentPreferences.ViewLayout = originalViewLayout.LayoutType;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadPreferences();
        }

        private void LoadPreferences()
        {
            Preferences preferences = Configuration.Configuration.LoadConfiguration<Preferences>();

            preferences.CopyProperties(CurrentPreferences);
            WindowState = CurrentPreferences.WindowState;

            if (WindowState == WindowState.Normal && ((int)CurrentPreferences.WindowWidth != 0 || (int)CurrentPreferences.WindowHeight != 0))
            {
                Left = preferences.WindowLeft;
                Top = preferences.WindowTop;
                Width = preferences.WindowWidth;
                Height = preferences.WindowHeight;
            }
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            CurrentPreferences.WindowState = WindowState;
        }

        private void InitializeVideoEventHandling()
        {
            VideoOutputControls = new[]
            {
                VideoOutputControl1, VideoOutputControl2, VideoOutputControl3, VideoOutputControl4, VideoOutputControl5,
                VideoOutputControl6, VideoOutputControl7, VideoOutputControl8, VideoOutputControl9
            };

            foreach (VideoOutputControl.VideoOutputControl videoOutputControl in VideoOutputControls)
            {
                videoOutputControl.VideoClicked += VideoOutputControl_VideoClicked;
                videoOutputControl.VideoRightClicked += VideoOutputControl_VideoRightClicked;
            }
        }

        private void VideoOutputControl_VideoClicked(object sender, EventArgs e)
        {
            VideoOutputControl.VideoOutputControl sourceVideoOutputControl = (VideoOutputControl.VideoOutputControl) sender;

            foreach (VideoOutputControl.VideoOutputControl videoOutputControl in VideoOutputControls)
            {
                videoOutputControl.HighlightBorderVisible = videoOutputControl == sourceVideoOutputControl;
            }

            SelectedDisplayOutput = new VideoOutputInformation();
            SelectedDisplayOutput.VideoControl = sourceVideoOutputControl;
        }

        private void VideoOutputControl_VideoRightClicked(object sender, EventArgs e)
        {
            VideoOutputControl.VideoOutputControl sourceVideoOutputControl = (VideoOutputControl.VideoOutputControl)sender;

            foreach (VideoOutputControl.VideoOutputControl videoOutputControl in VideoOutputControls)
            {
                videoOutputControl.HighlightBorderVisible = false;
            }

            SelectedDisplayOutput = null;
        }

        private void InitializeViewList()
        {
            ViewLayout view1By1 = ViewLayouts.First(x => x.LayoutType == ViewLayoutTypes.View1By1);
            view1By1.RowCount = 1;
            view1By1.ColumnCount = 1;
            view1By1.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost1,
                Row = 0,
                Column = 0,
                Visible = true
            });

            ViewLayout view1By2 = ViewLayouts.First(x => x.LayoutType == ViewLayoutTypes.View1By2);
            view1By2.RowCount = 1;
            view1By2.ColumnCount = 2;
            view1By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost1,
                Row = 0,
                Column = 0,
                Visible = true
            });
            view1By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost2,
                Row = 0,
                Column = 1,
                Visible = true
            });

            ViewLayout view2By1 = ViewLayouts.First(x => x.LayoutType == ViewLayoutTypes.View2By1);
            view2By1.RowCount = 2;
            view2By1.ColumnCount = 1;
            view2By1.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost1,
                Row = 0,
                Column = 0,
                Visible = true
            });
            view2By1.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost2,
                Row = 1,
                Column = 0,
                Visible = true
            });

            ViewLayout view2By2 = ViewLayouts.First(x => x.LayoutType == ViewLayoutTypes.View2By2);
            view2By2.RowCount = 2;
            view2By2.ColumnCount = 2;
            view2By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost1,
                Row = 0,
                Column = 0,
                Visible = true
            });
            view2By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost2,
                Row = 0,
                Column = 1,
                Visible = true
            });
            view2By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost3,
                Row = 1,
                Column = 0,
                Visible = true
            });
            view2By2.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost4,
                Row = 1,
                Column = 1,
                Visible = true
            });

            ViewLayout view3By3 = ViewLayouts.First(x => x.LayoutType == ViewLayoutTypes.View3By3);
            view3By3.RowCount = 3;
            view3By3.ColumnCount = 3;
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost1,
                Row = 0,
                Column = 0,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost2,
                Row = 0,
                Column = 1,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost3,
                Row = 0,
                Column = 2,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost4,
                Row = 1,
                Column = 0,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost5,
                Row = 1,
                Column = 1,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost6,
                Row = 1,
                Column = 2,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost7,
                Row = 2,
                Column = 0,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost8,
                Row = 2,
                Column = 1,
                Visible = true
            });
            view3By3.Channels.Add(new DisplayChannel
            {
                HostControl = WindowsFormsHost9,
                Row = 2,
                Column = 2,
                Visible = true
            });
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                CurrentPreferences.WindowLeft = Left;
                CurrentPreferences.WindowTop = Top;
                CurrentPreferences.WindowWidth = Width;
                CurrentPreferences.WindowHeight = Height;
            }
        }

        private void ListViewChannels_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            if ((sender as ListViewItem) == null)
            {
                if (ListViewChannels.SelectedItem != null)
                {
                    ListViewChannels.SelectedItem = null;
                }
            }

            ListViewChannels.Focus();
        }

        private void ListViewChannels_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewChannels.SelectedItem == null)
            {
                return;
            }

            if (ListViewChannels.SelectedItem is ChannelInformation)
            {
                if (SelectedDisplayOutput == null)
                {
                    MessageBox.Show("You must first select a video output location before you can start the video.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Console.WriteLine(SelectedDisplayOutput);
                ViewLayout currentViewLayout = ViewLayouts.First(x => x.LayoutType == CurrentPreferences.ViewLayout);

                DisplayChannel displayChannel = currentViewLayout.Channels.FirstOrDefault(x => ReferenceEquals(((WindowsFormsHost)x.HostControl).Child, SelectedDisplayOutput.VideoControl));

                UpdateVideoOutput((ChannelInformation)ListViewChannels.SelectedItem, displayChannel);
                UpdateVideoList();
            }
        }

        private void UpdateVideoList()
        {
            foreach (ChannelInformation channelInformation in Channels)
            {
                VideoOutputInformation videoOutputInformation = VideoOutputs.FirstOrDefault(x => x.Channel?.ChannelNumber == channelInformation.ChannelNumber);

                if (videoOutputInformation == null)
                {
                    channelInformation.ChannelIconType = ChannelIconTypes.Camera;
                }
                else
                {
                    channelInformation.ChannelIconType = ChannelIconTypes.Playing;
                }
            }
        }

        private void UpdateVideoOutput(ChannelInformation channelInformation, DisplayChannel displayChannel)
        {
            foreach (VideoOutputInformation videoOutputInformation in VideoOutputs)
            {
                if (videoOutputInformation.Channel == null)
                {
                    continue;
                }

                if (videoOutputInformation.Channel.ChannelNumber == channelInformation.ChannelNumber)
                {
                    if (!StopVideoPlayback(videoOutputInformation))
                    {
                        Console.WriteLine($"Could not stop the video with the ID {videoOutputInformation.VideoStreamId}");
                    }
                }
            }

            VideoOutputInformation currentVideoOutputInformation = VideoOutputs.First(x => x.Row == displayChannel.Row && x.Column == displayChannel.Column);
            
            if (currentVideoOutputInformation.VideoStatus == VideoStatuses.Playing)
            {
                StopVideoPlayback(currentVideoOutputInformation);
            }

            if (currentVideoOutputInformation != null)
            {
                currentVideoOutputInformation.Channel = channelInformation;
                currentVideoOutputInformation.DisplayChannel = displayChannel;
                currentVideoOutputInformation.VideoControl = (VideoOutputControl.VideoOutputControl) ((WindowsFormsHost) displayChannel.HostControl).Child;


                currentVideoOutputInformation.VideoStreamId = Shared.Device.StartVideoPlayback(channelInformation.ChannelNumber, currentVideoOutputInformation.VideoControl.VideoHandle);

                if (currentVideoOutputInformation.VideoStreamId >= 0)
                {
                    currentVideoOutputInformation.VideoStatus = VideoStatuses.Playing;
                }
                else
                {
                    // TODO: Determine the error
                }
            }
        }

        private bool StopVideoPlayback(VideoOutputInformation videoOutputInformation)
        {
            bool result = true;

            if (videoOutputInformation.VideoStreamId >= 0)
            {
                if (!HCNetSDK.NET_DVR_StopRealPlay(videoOutputInformation.VideoStreamId))
                {
                    result = false;
                }
                else
                {
                    videoOutputInformation.VideoStatus = VideoStatuses.Stopped;
                    videoOutputInformation.VideoStreamId = -1;
                    videoOutputInformation.VideoControl.ClearOutput();
                }
            }

            return result;
        }
    }
}
