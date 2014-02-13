using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhereAmI.models;

namespace WhereAmI
{
    class BackgroundWork
    {
        public static int refreshTime = 10000;

        public delegate void onMessageRefresh(string msg);
        public delegate void onWifiChanged(List<Wifi> w);
        public delegate void onPlaceChanged(Place p);

        public static onMessageRefresh messageRefreshHandlers;
        public static onPlaceChanged placeChangedHandlers;

        static public AutoResetEvent eventX = new AutoResetEvent(false);
        public static void Execute()
        {
            while (true)
            {
                DataManager.Instance.doRefresh();
                //Thread.Sleep(refreshTime);
                eventX.WaitOne(refreshTime);
            }
        }

        
    }
}
