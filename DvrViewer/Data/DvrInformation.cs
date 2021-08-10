using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DvrViewer.Attributes;
using DvrViewer.Shared;

namespace DvrViewer.Data
{
    public class DvrInformation : Item
    {
        private string _dvrHost;
        private int _dvrPort;
        private int _dvrHttpPort;
        private string _dvrUsername;
        private string _dvrPassword;

        public string DvrHost
        {
            get => _dvrHost;
            set
            {
                _dvrHost = value;
                NotifyPropertyChanged("DvrHost");
            }
        }

        public int DvrPort
        {
            get => _dvrPort;
            set
            {
                _dvrPort = value;
                NotifyPropertyChanged("DvrPort");
            }
        }

        public int DvrHttpPort
        {
            get => _dvrHttpPort;
            set
            {
                _dvrHttpPort = value;
                NotifyPropertyChanged("DvrHttpPort");
            }
        }

        [EncryptedConfiguration]
        public string DvrUsername
        {
            get => _dvrUsername;
            set
            {
                _dvrUsername = value;
                NotifyPropertyChanged("DvrUsername");
            }
        }

        [EncryptedConfiguration]
        public string DvrPassword
        {
            get => _dvrPassword;
            set
            {
                _dvrPassword = value;
                NotifyPropertyChanged("DvrPassword");
            }
        }
    }
}
