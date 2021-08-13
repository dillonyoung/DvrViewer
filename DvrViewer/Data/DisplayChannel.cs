using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class DisplayChannel : Item
    {
        private object _hostControl;
        private int _row;
        private int _column;
        private bool _visible;

        public object HostControl
        {
            get => _hostControl;
            set
            {
                _hostControl = value;
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

        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                NotifyPropertyChanged();
            }
        }
    }
}
