using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Animation;
using System.Xml;
using DvrViewer.Configuration;
using DvrViewer.Data;

namespace DvrViewer.Shared
{
    public static class Device
    {
        public static DvrInformation DvrDevice { get; set; }

        //private static string AuthenticationCredential { get; set; }

        private static int UserId { get; set; } 

        private static string AuthenticationString { get; set; }

        //public bool AuthenticateUser()
        //{
        //    string authentication = Base64.Encode($"{DvrDevice.DvrUsername}:{DvrDevice.DvrPassword}");
        //    bool valid = false;

        //    using (WebClient webClient = new WebClient())
        //    {
        //        webClient.Headers.Add("Authorization", $"Basic {authentication}");

        //        string data = webClient.DownloadString($@"http://{DvrDevice.DvrHost}:{DvrDevice.DvrPort}/ISAPI/Security/userCheck");

        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.LoadXml(data);

        //        if (xmlDocument.DocumentElement != null)
        //        {
        //            for (int i = 0; i < xmlDocument.DocumentElement.ChildNodes.Count; i++)
        //            {
        //                if (xmlDocument.DocumentElement.ChildNodes[i].Name == "statusValue")
        //                {
        //                    if (xmlDocument.DocumentElement.ChildNodes[i].InnerText == "200")
        //                    {
        //                        valid = true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    AuthenticationCredential = valid ? authentication : string.Empty;

        //    return valid;
        //}

        public static bool AuthenticateUser()
        {
            //HCNetSDK.NET_DVR_Init();

            HCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new HCNetSDK.NET_DVR_DEVICEINFO_V30();

            int result = HCNetSDK.NET_DVR_Login_V30(DvrDevice.DvrHost, DvrDevice.DvrPort, DvrDevice.DvrUsername, DvrDevice.DvrPassword, ref deviceInfo);

            if (result < 0)
            {
                uint error = HCNetSDK.NET_DVR_GetLastError();
                UserId = 0;

                return false;
            }

            UserId = result;

            return true;
        }

        public static DeviceInformation GetDeviceInformation()
        {
            DeviceInformation deviceInformation = new DeviceInformation();

            HCNetSDK.NET_DVR_DEVICECFG_V40 configuration = new HCNetSDK.NET_DVR_DEVICECFG_V40();
            UInt32 result = 0;
            Int32 size = Marshal.SizeOf(configuration);
            IntPtr deviceConfigPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(configuration, deviceConfigPointer, false);

            if (!HCNetSDK.NET_DVR_GetDVRConfig(UserId, HCNetSDK.NET_DVR_GET_DEVICECFG_V40, -1, deviceConfigPointer, (UInt32) size, ref result))
            {
                uint error = HCNetSDK.NET_DVR_GetLastError();

                Marshal.FreeHGlobal(deviceConfigPointer);

                return null;
            }

            configuration = (HCNetSDK.NET_DVR_DEVICECFG_V40) Marshal.PtrToStructure(deviceConfigPointer, typeof(HCNetSDK.NET_DVR_DEVICECFG_V40));

            deviceInformation.DeviceName = Encoding.GetEncoding("GBK").GetString(configuration.sDVRName);
            deviceInformation.DeviceType = Encoding.UTF8.GetString(configuration.byDevTypeName);
            deviceInformation.DeviceSerialNumber = Encoding.UTF8.GetString(configuration.sSerialNumber);
            deviceInformation.AnalogChannelCount = Convert.ToInt32(configuration.byChanNum);
            deviceInformation.IpChannelCount = Convert.ToInt32(configuration.byIPChanNum) + 256 * Convert.ToInt32(configuration.byHighIPChanNum);
            deviceInformation.AlarmInCount = Convert.ToInt32(configuration.byAlarmInPortNum);
            deviceInformation.AlarmOutCount = Convert.ToInt32(configuration.byAlarmOutPortNum);
            deviceInformation.NetworkPortCount = Convert.ToInt32(configuration.byNetworkPortNum);

            uint ver1 = (configuration.dwSoftwareVersion >> 24) & 0xFF;
            uint ver2 = (configuration.dwSoftwareVersion >> 16) & 0xFF;
            uint ver3 = configuration.dwSoftwareVersion & 0xFFFF;
            uint ver4 = (configuration.dwSoftwareBuildDate >> 16) & 0xFFFF;
            uint ver5 = (configuration.dwSoftwareBuildDate >> 8) & 0xFF;
            uint ver6 = configuration.dwSoftwareBuildDate & 0xFF;

            deviceInformation.DeviceVersion = $"v{ver1}.{ver2}.{ver3} Build {ver4:D2}{ver5:D2}{ver6:D2}";

            Marshal.FreeHGlobal(deviceConfigPointer);

            return deviceInformation;
        }

        public static NetworkInformation GetNetworkInformation()
        {
            NetworkInformation networkInformation = new NetworkInformation();

            HCNetSDK.NET_DVR_NETCFG_V30 configuration = new HCNetSDK.NET_DVR_NETCFG_V30();
            UInt32 result = 0;
            Int32 size = Marshal.SizeOf(configuration);
            IntPtr deviceConfigPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(configuration, deviceConfigPointer, false);

            if (!HCNetSDK.NET_DVR_GetDVRConfig(UserId, HCNetSDK.NET_DVR_GET_NETCFG_V30, -1, deviceConfigPointer, (UInt32) size, ref result))
            {
                uint error = HCNetSDK.NET_DVR_GetLastError();

                Marshal.FreeHGlobal(deviceConfigPointer);

                return null;
            }

            configuration = (HCNetSDK.NET_DVR_NETCFG_V30) Marshal.PtrToStructure(deviceConfigPointer, typeof(HCNetSDK.NET_DVR_NETCFG_V30));

            networkInformation.IpAddress = configuration.struEtherNet[0].struDVRIP.sIpV4;
            networkInformation.SubnetMask = configuration.struEtherNet[0].struDVRIPMask.sIpV4;
            networkInformation.Gateway = configuration.struGatewayIpAddr.sIpV4;

            if (!string.IsNullOrEmpty(configuration.struDnsServer1IpAddr.sIpV4))
            {
                networkInformation.Dns.Add(configuration.struDnsServer1IpAddr.sIpV4);
            }

            if (!string.IsNullOrEmpty(configuration.struDnsServer2IpAddr.sIpV4))
            {
                networkInformation.Dns.Add(configuration.struDnsServer2IpAddr.sIpV4);
            }

            networkInformation.HttpPort = Convert.ToInt32(configuration.wHttpPortNo);
            networkInformation.DvrPort = Convert.ToInt32(configuration.struEtherNet[0].wDVRPort);
            networkInformation.Mtu = Convert.ToInt32(configuration.struEtherNet[0].wMTU);

            networkInformation.DhcpEnabled = configuration.byUseDhcp == 1;
            networkInformation.PppoeEnabled = configuration.struPPPoE.dwPPPOE == 1;
            networkInformation.PppoeUsername = Encoding.UTF8.GetString(configuration.struPPPoE.sPPPoEUser);
            networkInformation.PppoePassword = configuration.struPPPoE.sPPPoEPassword;

            networkInformation.MacAddress = string.Join(":", configuration.struEtherNet[0].byMACAddr.Select(x => x.ToString("X2")));

            Marshal.FreeHGlobal(deviceConfigPointer);

            DvrDevice.DvrHttpPort = networkInformation.HttpPort;

            return networkInformation;
        }

        public static ChannelInformation GetChannelInformation(int channel)
        {
            ChannelInformation channelInformation = new ChannelInformation();

            HCNetSDK.NET_DVR_PICCFG_V30 configuration = new HCNetSDK.NET_DVR_PICCFG_V30();
            UInt32 result = 0;
            Int32 size = Marshal.SizeOf(configuration);
            IntPtr deviceConfigPointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(configuration, deviceConfigPointer, false);

            if (!HCNetSDK.NET_DVR_GetDVRConfig(UserId, HCNetSDK.NET_DVR_GET_PICCFG_V30, channel, deviceConfigPointer, (UInt32)size, ref result))
            {
                uint error = HCNetSDK.NET_DVR_GetLastError();

                Marshal.FreeHGlobal(deviceConfigPointer);

                return null;
            }

            configuration = (HCNetSDK.NET_DVR_PICCFG_V30)Marshal.PtrToStructure(deviceConfigPointer, typeof(HCNetSDK.NET_DVR_PICCFG_V30));

            channelInformation.ChannelNumber = channel;
            channelInformation.ChannelName = Encoding.UTF8.GetString(configuration.sChanName);
            channelInformation.ShowChannelName = configuration.dwShowChanName == 1;

            Marshal.FreeHGlobal(deviceConfigPointer);

            if (string.IsNullOrEmpty(AuthenticationString))
            {
                string authentication = Shared.Base64.Encode($"{DvrDevice.DvrUsername}:{DvrDevice.DvrPassword}");
                bool valid = false;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("Authorization", $"Basic {authentication}");

                    string data = webClient.DownloadString($"http://{DvrDevice.DvrHost}:{DvrDevice.DvrHttpPort}/ISAPI/Security/userCheck");

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(data);

                    if (xmlDocument.DocumentElement != null)
                    {
                        for (int i = 0; i < xmlDocument.DocumentElement.ChildNodes.Count; i++)
                        {
                            if (xmlDocument.DocumentElement.ChildNodes[i].Name == "statusValue")
                            {
                                if (xmlDocument.DocumentElement.ChildNodes[i].InnerText == "200")
                                {
                                    valid = true;
                                }
                            }
                        }
                    }
                }

                if (valid)
                {
                    AuthenticationString = authentication;
                }
            }

            if (!string.IsNullOrEmpty(AuthenticationString))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("Authorization", $"Basic {AuthenticationString}");
                    webClient.Headers.Add(HttpRequestHeader.Cookie, $"language=en; userInfo85={WebUtility.UrlEncode(AuthenticationString)}");

                    string data = webClient.DownloadString($"http://{DvrDevice.DvrHost}:{DvrDevice.DvrHttpPort}/ISAPI/System/Video/input/channels");

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(data);

                    for (int i = 0; i < xmlDocument.DocumentElement.ChildNodes.Count; i++)
                    {
                        XmlNode xmlNode = xmlDocument.DocumentElement.ChildNodes[i];

                        int id = 0;
                        string videoFormat = string.Empty;
                        string resolution = string.Empty;

                        for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
                        {
                            switch (xmlNode.ChildNodes[j].Name)
                            {
                                case "id":
                                    if (int.TryParse(xmlNode.ChildNodes[j].InnerText, out int tempId))
                                    {
                                        id = tempId;
                                    }

                                    break;
                                case "videoFormat":
                                    videoFormat = xmlNode.ChildNodes[j].InnerText;
                                    break;
                                case "resDesc":
                                    resolution = xmlNode.ChildNodes[j].InnerText;
                                    break;
                            }
                        }

                        if (id == channel)
                        {
                            if (resolution == "NO VIDEO")
                            {
                                channelInformation.VideoAvailable = false;
                                channelInformation.VideoResolution = string.Empty;
                            }
                            else
                            {
                                channelInformation.VideoAvailable = true;
                                channelInformation.VideoResolution = resolution;
                            }

                            channelInformation.VideoFormat = videoFormat;
                        }
                    }
                }
            }

            return channelInformation;
        }
    }
}
