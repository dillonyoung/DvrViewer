using System.Collections.ObjectModel;
using DvrViewer.Enum;
using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class ViewLayout : Item
    {
        private ViewLayoutTypes _layoutType;
        private bool _isChecked;
        private string _title;
        private int _rowCount;
        private int _columnCount;

        public ViewLayoutTypes LayoutType
        {
            get => _layoutType;
            set
            {
                _layoutType = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public int RowCount
        {
            get => _rowCount;
            set
            {
                _rowCount = value;
                NotifyPropertyChanged();
            }
        }

        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                _columnCount = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DisplayChannel> Channels { get; set; } = new ObservableCollection<DisplayChannel>();
    }
}
