using System.Windows;
using DvrViewer.Enum;
using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class Preferences : Item
    {
        public Preferences()
        {
            WindowState = WindowState.Maximized;
            ShowChannels = true;
            ShowToolBar = true;
            ViewLayout = ViewLayoutTypes.View3By3;
        }

        private WindowState _windowState;
        private bool _showChannels;
        private bool _showToolBar;
        private ViewLayoutTypes _viewLayout;
        private double _windowLeft;
        private double _windowTop;
        private double _windowWidth;
        private double _windowHeight;

        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                OnWindowStateChange?.Invoke();
                NotifyPropertyChanged();
            }
        }

        public bool ShowChannels
        {
            get => _showChannels;
            set
            {
                _showChannels = value;
                OnShowChannelsChange?.Invoke();
                NotifyPropertyChanged();
            }
        }

        public bool ShowToolBar
        {
            get => _showToolBar;
            set
            {
                _showToolBar = value;
                OnShowToolBarChange?.Invoke();
                NotifyPropertyChanged();
            }
        }

        public ViewLayoutTypes ViewLayout
        {
            get => _viewLayout;
            set
            {
                _viewLayout = value;
                OnViewLayoutChange?.Invoke();
                NotifyPropertyChanged();
            }
        }

        public double WindowLeft
        {
            get => _windowLeft;
            set
            {
                _windowLeft = value;
                NotifyPropertyChanged();
            }
        }

        public double WindowTop
        {
            get => _windowTop;
            set
            {
                _windowTop = value;
                NotifyPropertyChanged();
            }
        }

        public double WindowWidth
        {
            get => _windowWidth;
            set
            {
                _windowWidth = value;
                NotifyPropertyChanged();
            }
        }

        public double WindowHeight
        {
            get => _windowHeight;
            set
            {
                _windowHeight = value;
                NotifyPropertyChanged();
            }
        }

        public delegate void OnWindowStateChangeDelegate();

        public event OnWindowStateChangeDelegate OnWindowStateChange;

        public delegate void OnShowChannelsChangeDelegate();

        public event OnShowChannelsChangeDelegate OnShowChannelsChange;

        public delegate void OnShowToolBarChanageDelegate();

        public event OnShowToolBarChanageDelegate OnShowToolBarChange;

        public delegate void OnViewLayoutChangeDelegate();

        public event OnViewLayoutChangeDelegate OnViewLayoutChange;
    }
}
