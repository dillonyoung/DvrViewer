using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DvrViewer.Enum;
using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class VideoOutputInformation : Item
    {
        private ChannelInformation _channel;
        private DisplayChannel _displayChannel;
        private VideoOutputControl.VideoOutputControl _videoControl;
        private VideoStatuses _videoStatus;
        private HCNetSDK.NET_DVR_PREVIEWINFO _previewInfo;
        private int _videoStreamId;
        private int _row;
        private int _column;

        public ChannelInformation Channel
        {
            get => _channel;
            set
            {
                _channel = value;
                NotifyPropertyChanged();
            }
        }

        public DisplayChannel DisplayChannel
        {
            get => _displayChannel;
            set
            {
                _displayChannel = value;
                NotifyPropertyChanged();
            }
        }

        public VideoOutputControl.VideoOutputControl VideoControl
        {
            get => _videoControl;
            set
            {
                _videoControl = value;
                NotifyPropertyChanged();
            }
        }

        public VideoStatuses VideoStatus
        {
            get => _videoStatus;
            set
            {
                _videoStatus = value;
                NotifyPropertyChanged();
            }
        }

        public HCNetSDK.NET_DVR_PREVIEWINFO PreviewInfo
        {
            get => _previewInfo;
            set
            {
                _previewInfo = value;
                NotifyPropertyChanged();
            }
        }

        public int VideoStreamId
        {
            get => _videoStreamId;
            set
            {
                _videoStreamId = value;
                NotifyPropertyChanged();
            }
        }

        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                NotifyPropertyChanged();
            }
        }

        public int Column
        {
            get => _column;
            set
            {
                _column = value;
                NotifyPropertyChanged();
            }
        }
    }
}
