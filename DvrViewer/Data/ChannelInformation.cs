using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvrViewer.Data
{
    public class ChannelInformation
    {
        public int ChannelNumber { get; set; }

        public string ChannelName { get; set; }

        public bool ShowChannelName { get; set; }

        public bool VideoAvailable { get; set; }

        public string VideoFormat { get; set; }

        public string VideoResolution { get; set; }
    }
}
