using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereAmI.models;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using WhereAmI.triangulation;

namespace WhereAmI
{
    public class CurrentState : INotifyPropertyChanged
    {
        private Place _place;
        //Property Definition
        public Place Place
        {
            get { return this._place; }
            //The setter has to call NotifyPropertyChanged to reflect changes
            set
            {
                if (this._place != value)
                {
                    this._place = value;
                    NotifyPropertyChanged("Place");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class DataManager
    {
        private static DataManager dmInstance;

        //A list that notifies any destinations of changes to its content
        //It works only when an item is added or deleted
        //It reflects changes in the list data source

        public List<Wifi> mockWifis = new List<Wifi>();
        public ObservableCollection<Wifi> wifis;
        //public ObservableCollection<Place> places;
        //public ObservableCollection<models.Action> actions;

        public CurrentState currentState;

        public AppContext context;
        public Algorithm algo;

        public void init()
        {
            wifis = new ObservableCollection<Wifi>();
            currentState = new CurrentState();
            context = new AppContext();
            algo = new Algorithm();
        }

        public void doLoad()
        {
            bool loaded=false;
            while (!loaded)
            {
                try
                {
                    //do on Worker Thread
                    context.Places.Load();
                    context.Actions.Load();
                    //Only for tests
                    foreach (Place p in context.Places.Local)
                        foreach (Wifi w in p.Wifis)
                            mockWifis.Add(w);
                    loaded = true;
                }
                catch (Exception e)
                {
                    loaded = false;
                    System.Windows.MessageBox.Show(e.Message, "Error Loading");
                    context = new AppContext();
                }
            }
        }

        private DataManager()
        {

        }

        public static DataManager Instance
        {
            get
            {
                if (dmInstance == null)
                    dmInstance = new DataManager();
                return dmInstance;
            }
        }

        //Read the current wifis networks connections from the WLAN API
        //update the wifis list
        public List<Wifi> loadWifis()
        {
            List<Wifi> currentWifis = null; ;
            try
            {
                //currentWifis = loadWifisMockVersion();
                currentWifis = wlan.WlanDataManager.loadWifis();

                //Problem: wifis is an observable collection created in the UI Thread, 
                //so it can be modified only by UI Thread
                App.Current.Dispatcher.BeginInvoke((System.Action)delegate
                 {
                     wifis.Clear();
                     foreach (Wifi w in currentWifis)
                         wifis.Add(w);
                 });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return currentWifis;
        }

        private List<Wifi> loadWifisMockVersion()
        {
            List<Wifi> currentWifis = new List<Wifi>();
            Random random = new Random();
            int randomNumber = random.Next(0, mockWifis.Count / 4);
            for (var i = 0; i < randomNumber; i++)
            {
                currentWifis.Add(mockWifis.ElementAt(random.Next(0, mockWifis.Count)));
            }
            return currentWifis;
        }

        private DateTime oldDate;

        private Object writeLock = new Object();

        public void doRefresh()
        {
            //Call all registered handlers
            BackgroundWork.messageRefreshHandlers("Starting refresh...");

            //Saved all input data in local variable
            var currentWifis = loadWifis();
            var currentPlaces = DataManager.Instance.context.Places.Local.ToList();
            if (currentWifis == null)
            {
                BackgroundWork.messageRefreshHandlers("Error: no wifi detected!");
                return;
            }

            if (currentPlaces == null)
            {
                BackgroundWork.messageRefreshHandlers("No places stored!");
                return;
            }
            //Run algorithm with retrieved input data
            Place newPlace = algo.computeCurrentPlace(currentWifis, currentPlaces);
            BackgroundWork.messageRefreshHandlers("Place Found");

            //Update time counter - safe?
            //Also other write from UI events
            lock (writeLock)
            {   //it is necessary to compute time
                //from last refesh, because it can be done manually
                if (this.currentState.Place == null)
                    this.oldDate = DateTime.Now;
                DateTime now = DateTime.Now;
                TimeSpan diff = now.Subtract(oldDate);
                oldDate = now;
                newPlace.Cnt += (ulong)diff.TotalMilliseconds;
                //or simply
                //newPlace.Cnt = newPlace.Cnt + BackgroundWork.refreshTime / 1000;
                safeSave();
            }

            if (currentState.Place == null || newPlace.PlaceId != currentState.Place.PlaceId)
            {
                currentState.Place = newPlace;
                var currentActions = currentState.Place.InActions.ToList();

                //System.Threading.Thread.Sleep(5000);
                foreach (models.Action a in currentActions)
                {
                    //System.Threading.Thread.Sleep(10000);
                    ActionManager.execute(a);
                }
            }
            BackgroundWork.messageRefreshHandlers("Done");

        }

        public void safeSave()
        {
            lock (writeLock)
            {
                try { 
                    this.context.SaveChanges();
                }
                catch (Exception e)
                {
                    BackgroundWork.messageRefreshHandlers(e.Message);
                    System.Windows.MessageBox.Show(e.Message, "Error Loading");
                }
                
            }
            /* TODO write with thread
             * 
            System.Threading.ThreadPool.QueueUserWorkItem(
            (o) => {
                lock (writeLock)
                {
                    this.context.SaveChanges();
                }
            });
             * 
             */
        }
    }
}
