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
        private DateTime date;
        private Place _place;
        private Place _old;
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
                    this.date = DateTime.Now;
                }
                var now = DateTime.Now;
                TimeSpan diff = now.Subtract(this.date);
                this.date = now;
                this._place.Cnt += (int)diff.TotalSeconds;
                DataManager.Instance.context.SaveChanges();
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
        
        //public ObservableCollection<Place> places;
        public ObservableCollection<Wifi> wifis;
        public CurrentState currentState;
        
        public AppContext context;
        public Algorithm algo;

        public void init()
        {
            wifis = new ObservableCollection<Wifi>();
            currentState = new CurrentState();
            //loadData();
            //loadDataDB();
            context = new AppContext();
            algo = new Algorithm();
            // Load is an extension method on IQueryable,  
            // defined in the System.Data.Entity namespace. 
            // This method enumerates the results of the query,  
            // similar to ToList but without creating a list. 
            // When used with Linq to Entities this method  
            // creates entity objects and adds them to the context.
            context.Places.Load();
            context.Actions.Load();
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

        private void loadDataDB()
        {

            using (var ctx = new AppContext())
            {

                var action = new WhereAmI.models.Action { Name = "prova", Command = "run prova" };
                ctx.Actions.Add(action);
                ctx.SaveChanges();

                var actions = ctx.Actions;
                foreach (var a in actions)
                {
                    Console.WriteLine("Action: "+a.ToString());
                }

                //var place = new Place { Name = "home", Cnt = 0, Snapshot = "dlink:10, home:30" };
                //ctx.Places.Add(place);
                //ctx.SaveChanges();

                var places = ctx.Places;
                foreach (var p in places)
                {
                    Console.WriteLine("Place:" + p.ToString());
                }
            }
                 
            }
       
        private void loadData()
        {
            //Mock for snapshot 
            List<Wifi> snapshot1 = new List<Wifi>();
            snapshot1.Add(new Wifi() { SSID = "Home", PowerPerc = 100 });
            snapshot1.Add(new Wifi() { SSID = "HomeDani", PowerPerc = 1 });
            snapshot1.Add(new Wifi() { SSID = "dlink", PowerPerc = 10 });

            List<Wifi> snapshot2 = new List<Wifi>();
            snapshot2.Add(new Wifi() { SSID = "Polito", PowerPerc = 10 });
            snapshot2.Add(new Wifi() { SSID = "Lab1", PowerPerc = 30 });
            snapshot2.Add(new Wifi() { SSID = "Lab2", PowerPerc = 50 });

            List<Wifi> snapshot3 = new List<Wifi>();
            snapshot3.Add(new Wifi() { SSID = "CompanyHall", PowerPerc = 50 });
            snapshot3.Add(new Wifi() { SSID = "CompanyRoom1", PowerPerc = 20 });
            snapshot3.Add(new Wifi() { SSID = "CompanyLab", PowerPerc = 80 });

            //Mock for places 
            Place p1 = new Place("Home", snapshot1);
            Place p2 = new Place("Polito", snapshot2);
            Place p3 = new Place("Work", snapshot3);

            //places.Add(p1);
            //places.Add(p2);
            //places.Add(p3);

        }

        //Read the current wifis networks connections from the WLAN API
        //update the wifis list
        public void loadWifis()
        {
            loadWifisMockVersion();
            try
            {
                wlan.WlanDataManager.loadWifis();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void loadWifisMockVersion(){
            wifis.Clear();
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            for (var i = 0; i < randomNumber; i++)
            {
                wifis.Add(new Wifi() { SSID = "AAAAAA", PowerPerc = random.Next(0, 100) });
                wifis.Add(new Wifi() { SSID = "BBB", PowerPerc = random.Next(0, 100) });
                wifis.Add(new Wifi() { SSID = "CCCCCCCCC", PowerPerc = random.Next(0, 100) });
            }
        }

        //Retrieve the current place from the list of recorded places
        //using the triangulation algorithm 
        //that receives in input the list of current detected wifis
        private void computeCurrentPlace()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            //currentState.Place = places.ElementAt(random.Next(0, places.Count));
            //currentState.Place = context.Places.AsEnumerable<Place>().ElementAt<Place>(0);
            currentState.Place = algo.computeCurrentPlace(wifis);
        }

        public void refresh()
        {
            loadWifis();
            computeCurrentPlace();
        }
    }
}
