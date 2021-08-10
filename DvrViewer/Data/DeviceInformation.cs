using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvrViewer.Data
{
    public class DeviceInformation
    {
        public string DeviceName { get; set; }

        public string DeviceType { get; set; }

        public string DeviceSerialNumber { get; set; }

        public string DeviceVersion { get; set; }

        public int AnalogChannelCount { get; set; }

        public int IpChannelCount { get; set; }

        public int NetworkPortCount { get; set; }

        public int AlarmInCount { get; set; }

        public int AlarmOutCount { get; set; }
    }
}
