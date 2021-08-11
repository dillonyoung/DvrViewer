using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvrViewer.Shared
{
    public class CheckedItem : Item
    {
        private bool _isChecked;
        private string _title;
        private dynamic _value;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (value == _isChecked)
                {
                    return;
                }

                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public dynamic Value
        {
            get => _value;
            set
            {
                if (value == _value)
                {
                    return;
                }

                _value = value;
                NotifyPropertyChanged("Value");
            }
        }
    }
}
