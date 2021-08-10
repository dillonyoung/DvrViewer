using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvrViewer.Data
{
    public class NetworkInformation
    {
        public string IpAddress { get; set; }

        public string SubnetMask { get; set; }

        public string Gateway { get; set; }

        public List<string> Dns { get; set; } = new List<string>();

        public string MacAddress { get; set; }

        public int Mtu { get; set; }

        public int HttpPort { get; set; }

        public int DvrPort { get; set; }

        public bool DhcpEnabled { get; set; }

        public bool PppoeEnabled { get; set; }

        public string PppoeUsername { get; set; }

        public string PppoePassword { get; set; }
    }
}
