using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereAmI.models
{
    public class Wifi
    {
        public string MAC { get; set; }
        public string SSID { get; set; }

        public float Power { get; set; }
        public int PowerPerc { get; set; }

        public override string ToString()
        {
            return this.MAC+ ": "+ this.SSID + ": " + this.PowerPerc+";";
        }
    }
}
