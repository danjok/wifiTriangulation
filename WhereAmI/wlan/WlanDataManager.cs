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

        public static void loadWifis()
        {
            WlanClient client = new WlanClient();
            DataManager.Instance.wifis.Clear();
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
                    DataManager.Instance.wifis.Add(new Wifi() { SSID = ssid, PowerPerc = Convert.ToInt32(powerc) });
                    //if ( network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.WEP )
                    //{
                    //    Console.WriteLine( "Found WEP network with SSID {0}.", GetStringForSSID(network.dot11Ssid));
                    //}
                }
            }
        }
    }
}

