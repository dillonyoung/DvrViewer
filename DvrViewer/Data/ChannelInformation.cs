using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DvrViewer.Enum;
using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class ChannelInformation : Item
    {
        private int _channelNumber;
        private string _channelName;
        private bool _showChannelName;
        private bool _videoAvailable;
        private string _videoFormat;
        private string _videoResolution;
        private ChannelIconTypes _channelIconType;

        public int ChannelNumber
        {
            get => _channelNumber;
            set
            {
                _channelNumber = value;
                NotifyPropertyChanged();
            }
        }

        public string ChannelName
        {
            get => _channelName;
            set
            {
                _channelName = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowChannelName
        {
            get => _showChannelName;
            set
            {
                _showChannelName = value;
                NotifyPropertyChanged();
            }
        }

        public bool VideoAvailable
        {
            get => _videoAvailable;
            set
            {
                _videoAvailable = value;
                NotifyPropertyChanged();
            }
        }

        public string VideoFormat
        {
            get => _videoFormat;
            set
            {
                _videoFormat = value;
                NotifyPropertyChanged();
            }
        }

        public string VideoResolution
        {
            get => _videoResolution;
            set
            {
                _videoResolution = value;
                NotifyPropertyChanged();
            }
        }

        public ChannelIconTypes ChannelIconType
        {
            get => _channelIconType;
            set
            {
                _channelIconType = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Icon");
            }
        }

        public BitmapImage Icon
        {
            get
            {
                string path = "pack://application:,,,/Images/{0}.ico";
                string name;

                switch (_channelIconType)
                {
                    case ChannelIconTypes.Playing:
                        name = "CameraPlay";
                        break;
                    case ChannelIconTypes.Stopped:
                        name = "Stop";
                        break;
                    default:
                        name = "Camera";
                        break;
                }

                if (!_videoAvailable)
                {
                    name = $"{name}Disabled";
                }

                string location = string.Format(path, name);

                return new BitmapImage(new Uri(location));
            }
        }
    }
}
