using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereAmI.models
{
    //Model class has to implement the INotifyPropertyChanged interface
    //It reflects changes in the data objects
    //objects are capable of alerting the UI layer of changes to its properties
    //Note: change will happen on the bound data object itself and not the source list 
    public class Place : INotifyPropertyChanged
    {
        public long PlaceId { get; set; }
        private string _name;
        //Property Definition
        public string Name
        {
            get { return this._name; }
            //The setter has to call NotifyPropertyChanged to reflect changes
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        private long _cnt;
        //Property Definition
        public long Cnt
        {
            get { return this._cnt; }
            //The setter has to call NotifyPropertyChanged to reflect changes
            set
            {
                if (this._cnt != value)
                {
                    this._cnt = value;
                    this.NotifyPropertyChanged("Cnt");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        } 

        public Place(string name, List<Wifi> wifis)
        {
            this.Name = name;
            this.Wifis = wifis;
            this.Cnt = 0;
        }

        public Place(){
            //BeforeActions = new HashSet<Action>();
            //AfterActions = new HashSet<Action>();
            //InActions = new HashSet<Action>();
            InActions = new ObservableCollection<Action>();
        }

        private string _snapshot;
        public string Snapshot {
            get
            {
                return this._snapshot; 
            } set{
                this._snapshot = value;
                Wifis = Place.deserializationSnapshot(value);
                this.NotifyPropertyChanged("Wifis");
            }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<Wifi> Wifis { get; set; }

        //Relationships
        //public virtual ICollection<Action> BeforeActions { get; set; }
        //public virtual ICollection<Action> AfterActions { get; set; }

        public virtual ObservableCollection<Action> InActions { get; set; }
        //public virtual ICollection<Action> InActions { get; set; }


        public override string ToString()
        {
            return this.Name + ": " + this.Cnt + ": "+ this.Snapshot+ ";";
        }

        static public string serializationSnapshot(List<Wifi> snapshot)
        {
            string serialized = "";
            foreach (Wifi wifi in snapshot)
                serialized += wifi.ToString();
            return serialized;
        }

        static public List<Wifi> deserializationSnapshot(string ser)
        {
            List<Wifi> snapshot = new List<Wifi>();
            string[] wifiStrings = ser.Split(';');
            string[] sProperties;

            foreach (string ws in wifiStrings)
            {
                sProperties = ws.Split(':');
                if (sProperties.Length == 2)
                    snapshot.Add(new Wifi { 
                        SSID = sProperties[0], 
                        PowerPerc = int.Parse(sProperties[1])
                    });
            }

            return snapshot;
        }

    }
}
