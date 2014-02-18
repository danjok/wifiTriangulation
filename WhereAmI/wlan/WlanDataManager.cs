using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereAmI.models;

namespace WhereAmI.wlan
{
    class WlanDataManager
    {
        /// <summary>
        /// Converts a 802.11 SSID to a string.
        /// </summary>
        private static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        private static string GetBssIdString(byte[] dot11Bssid)
        {
            StringBuilder sb = new StringBuilder(dot11Bssid.Length * 2);
            foreach (byte b in dot11Bssid)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
        
        public static List<Wifi> loadWifis()
        {
            WlanClient client = new WlanClient();
            List<Wifi> wifis = new List<Wifi>();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanBssEntry[] bssEntries = wlanIface.GetNetworkBssList();
                foreach (Wlan.WlanBssEntry bssEntry in bssEntries)
                {   
                    var mac = GetBssIdString(bssEntry.dot11Bssid);
                    var ssid = GetStringForSSID(bssEntry.dot11Ssid);
                    int powerc = (int)bssEntry.linkQuality;
                    wifis.Add(new Wifi() { SSID = ssid, PowerPerc = powerc, MAC = mac });   
                }
            }
            return wifis;
        }
    }
}

