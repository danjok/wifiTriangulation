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

        public static List<Wifi> loadWifis()
        {
            WlanClient client = new WlanClient();
            List<Wifi> wifis = new List<Wifi>();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    var ssid = GetStringForSSID(network.dot11Ssid);
                    var name = network.profileName;
                    var s = network.ToString();
                    uint powerc = network.wlanSignalQuality;
                    wifis.Add(new Wifi() { SSID = ssid, PowerPerc = Convert.ToInt32(powerc) });   
                }
            }
            return wifis;
        }
    }
}

