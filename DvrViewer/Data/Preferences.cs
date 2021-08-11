using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ViewLayout = ViewLayoutTypes.View3By3;
        }

        private WindowState _windowState;
        private bool _showChannels;
        private ViewLayoutTypes _viewLayout;

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

        public delegate void OnWindowStateChangeDelegate();

        public event OnWindowStateChangeDelegate OnWindowStateChange;

        public delegate void OnShowChannelsChangeDelegate();

        public event OnShowChannelsChangeDelegate OnShowChannelsChange;

        public delegate void OnViewLayoutChangeDelegate();

        public event OnViewLayoutChangeDelegate OnViewLayoutChange;
    }
}
