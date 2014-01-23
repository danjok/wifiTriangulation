using System;
using System.Collections.Generic;
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
        private int _cnt;
        //Property Definition
        public int Cnt
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

        public List<Wifi> Wifis { get; set; }
        public List<Action> BeforeActions { get; set; }
        public List<Action> InActions { get; set; }
        public List<Action> AfterActions { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
