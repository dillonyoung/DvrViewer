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

        public static ObservableCollection<CheckedItem> ViewLayoutTypesProvider { get; set; }

        private DvrInformation DvrConfiguration { get; set; }

        private DeviceInformation DeviceInfo { get; set; }

        private NetworkInformation NetworkInfo { get; set; }

        private List<ChannelInformation> Channels { get; set; } = new List<ChannelInformation>();

        public MainWindow()
        {
            ViewLayoutTypesProvider = new ObservableCollection<CheckedItem>();

            foreach (ViewLayoutTypes viewLayoutType in System.Enum.GetValues(typeof(ViewLayoutTypes)))
            {
                DescriptionAttribute descriptionAttribute = viewLayoutType.GetType().GetField(System.Enum.GetName(viewLayoutType.GetType(), viewLayoutType)).GetCustomAttribute<DescriptionAttribute>();

                CheckedItem checkedItem = new CheckedItem();
                checkedItem.IsChecked = false;
                checkedItem.Value = viewLayoutType;
                checkedItem.Title = descriptionAttribute == null ? viewLayoutType.ToString() : descriptionAttribute.Description;

                ViewLayoutTypesProvider.Add(checkedItem);
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

            CurrentPreferences.OnShowChannelsChange += MainWindow_OnShowChannelsChange;
            CurrentPreferences.OnViewLayoutChange += MainWindow_OnViewLayoutChange;

            TextBlockStatus.Text = $"Connected to {DeviceInfo.DeviceName} {Device.DvrDevice.DvrHost}";
        }

        private void MainWindow_OnViewLayoutChange()
        {
            foreach (CheckedItem viewLayout in MenuItemViewOutputArea.Items)
            {
                viewLayout.IsChecked = viewLayout.Value == CurrentPreferences.ViewLayout;
            }

            switch (CurrentPreferences.ViewLayout)
            {
                case ViewLayoutTypes.View1By1:
                    LabelView1.Visibility = Visibility.Visible;
                    LabelView2.Visibility = Visibility.Hidden;
                    LabelView3.Visibility = Visibility.Hidden;
                    LabelView4.Visibility = Visibility.Hidden;
                    LabelView5.Visibility = Visibility.Hidden;
                    LabelView6.Visibility = Visibility.Hidden;
                    LabelView7.Visibility = Visibility.Hidden;
                    LabelView8.Visibility = Visibility.Hidden;
                    LabelView9.Visibility = Visibility.Hidden;

                    GridViewLayout.ColumnDefinitions.Clear();
                    for (int i = 0; i < 1; i++)
                    {
                        GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    GridViewLayout.RowDefinitions.Clear();
                    for (int i = 0; i < 1; i++)
                    {
                        GridViewLayout.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetColumn(LabelView1, 0);
                    Grid.SetRow(LabelView1, 0);

                    break;
                case ViewLayoutTypes.View1By2:
                    LabelView1.Visibility = Visibility.Visible;
                    LabelView2.Visibility = Visibility.Visible;
                    LabelView3.Visibility = Visibility.Hidden;
                    LabelView4.Visibility = Visibility.Hidden;
                    LabelView5.Visibility = Visibility.Hidden;
                    LabelView6.Visibility = Visibility.Hidden;
                    LabelView7.Visibility = Visibility.Hidden;
                    LabelView8.Visibility = Visibility.Hidden;
                    LabelView9.Visibility = Visibility.Hidden;

                    GridViewLayout.ColumnDefinitions.Clear();
                    for (int i = 0; i < 2; i++)
                    {
                        GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    GridViewLayout.RowDefinitions.Clear();
                    for (int i = 0; i < 1; i++)
                    {
                        GridViewLayout.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetColumn(LabelView1, 0);
                    Grid.SetRow(LabelView1, 0);
                    Grid.SetColumn(LabelView2, 1);
                    Grid.SetRow(LabelView2, 0);
                    break;
                case ViewLayoutTypes.View2By1:
                    LabelView1.Visibility = Visibility.Visible;
                    LabelView2.Visibility = Visibility.Visible;
                    LabelView3.Visibility = Visibility.Hidden;
                    LabelView4.Visibility = Visibility.Hidden;
                    LabelView5.Visibility = Visibility.Hidden;
                    LabelView6.Visibility = Visibility.Hidden;
                    LabelView7.Visibility = Visibility.Hidden;
                    LabelView8.Visibility = Visibility.Hidden;
                    LabelView9.Visibility = Visibility.Hidden;

                    GridViewLayout.ColumnDefinitions.Clear();
                    for (int i = 0; i < 1; i++)
                    {
                        GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    GridViewLayout.RowDefinitions.Clear();
                    for (int i = 0; i < 2; i++)
                    {
                        GridViewLayout.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetColumn(LabelView1, 0);
                    Grid.SetRow(LabelView1, 0);
                    Grid.SetColumn(LabelView2, 0);
                    Grid.SetRow(LabelView2, 1);
                    break;
                case ViewLayoutTypes.View2By2:
                    LabelView1.Visibility = Visibility.Visible;
                    LabelView2.Visibility = Visibility.Visible;
                    LabelView3.Visibility = Visibility.Visible;
                    LabelView4.Visibility = Visibility.Visible;
                    LabelView5.Visibility = Visibility.Hidden;
                    LabelView6.Visibility = Visibility.Hidden;
                    LabelView7.Visibility = Visibility.Hidden;
                    LabelView8.Visibility = Visibility.Hidden;
                    LabelView9.Visibility = Visibility.Hidden;

                    GridViewLayout.ColumnDefinitions.Clear();
                    for (int i = 0; i < 2; i++)
                    {
                        GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    GridViewLayout.RowDefinitions.Clear();
                    for (int i = 0; i < 2; i++)
                    {
                        GridViewLayout.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetColumn(LabelView1, 0);
                    Grid.SetRow(LabelView1, 0);
                    Grid.SetColumn(LabelView2, 1);
                    Grid.SetRow(LabelView2, 0);
                    Grid.SetColumn(LabelView3, 0);
                    Grid.SetRow(LabelView3, 1);
                    Grid.SetColumn(LabelView4, 1);
                    Grid.SetRow(LabelView4, 1);
                    break;
                case ViewLayoutTypes.View3By3:
                    LabelView1.Visibility = Visibility.Visible;
                    LabelView2.Visibility = Visibility.Visible;
                    LabelView3.Visibility = Visibility.Visible;
                    LabelView4.Visibility = Visibility.Visible;
                    LabelView5.Visibility = Visibility.Visible;
                    LabelView6.Visibility = Visibility.Visible;
                    LabelView7.Visibility = Visibility.Visible;
                    LabelView8.Visibility = Visibility.Visible;
                    LabelView9.Visibility = Visibility.Visible;

                    GridViewLayout.ColumnDefinitions.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        GridViewLayout.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    GridViewLayout.RowDefinitions.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        GridViewLayout.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetColumn(LabelView1, 0);
                    Grid.SetRow(LabelView1, 0);
                    Grid.SetColumn(LabelView2, 1);
                    Grid.SetRow(LabelView2, 0);
                    Grid.SetColumn(LabelView3, 2);
                    Grid.SetRow(LabelView3, 0);

                    Grid.SetColumn(LabelView4, 0);
                    Grid.SetRow(LabelView4, 1);
                    Grid.SetColumn(LabelView5, 1);
                    Grid.SetRow(LabelView5, 1);
                    Grid.SetColumn(LabelView6, 2);
                    Grid.SetRow(LabelView6, 1);

                    Grid.SetColumn(LabelView7, 0);
                    Grid.SetRow(LabelView7, 2);
                    Grid.SetColumn(LabelView8, 1);
                    Grid.SetRow(LabelView8, 2);
                    Grid.SetColumn(LabelView9, 2);
                    Grid.SetRow(LabelView9, 2);
                    break;
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
            CheckedItem originalViewLayout = (CheckedItem)((MenuItem) e.OriginalSource).DataContext;

            CurrentPreferences.ViewLayout = originalViewLayout.Value;
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
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            CurrentPreferences.WindowState = WindowState;
        }
    }
}
