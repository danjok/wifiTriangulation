using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhereAmI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class CurrentState : INotifyPropertyChanged
        {
            private PlaceModel _place;
            //Property Definition
            public PlaceModel Place
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
        CurrentState currentState = new CurrentState();

        //A list that notifies any destinations of changes to its content
        //It works only when an item is added or deleted
        //It reflects changes in the list data source
        private ObservableCollection<PlaceModel> places = new ObservableCollection<PlaceModel>();
        private ObservableCollection<WifiModel> wifis = new ObservableCollection<WifiModel>();

        public MainWindow()
        {
            InitializeComponent();

            //Mock for wifis
            for (var i = 0; i < 10; i++)
            {
                wifis.Add(new WifiModel() { SSID = "AAAAAA", PowerPerc = 42 });
                wifis.Add(new WifiModel() { SSID = "BBB", PowerPerc = 39 });
                wifis.Add(new WifiModel() { SSID = "CCCCCCCCC", PowerPerc = 13 });
            }
            wifiData.ItemsSource = wifis;

            //Mock for snapshot 
            List<WifiModel> snapshot1 = new List<WifiModel>();
            snapshot1.Add(new WifiModel() { SSID = "AAA", PowerPerc = 100 });
            snapshot1.Add(new WifiModel() { SSID = "BBB", PowerPerc = 1 });
            snapshot1.Add(new WifiModel() { SSID = "CCCCCC", PowerPerc = 10 });

            List<WifiModel> snapshot2 = new List<WifiModel>();
            snapshot2.Add(new WifiModel() { SSID = "AAA", PowerPerc = 10 });
            snapshot2.Add(new WifiModel() { SSID = "BBB", PowerPerc = 30 });
            snapshot2.Add(new WifiModel() { SSID = "CCCCCCCCC", PowerPerc = 50 });

            List<WifiModel> snapshot3 = new List<WifiModel>();
            snapshot3.Add(new WifiModel() { SSID = "A", PowerPerc = 50 });
            snapshot3.Add(new WifiModel() { SSID = "BBB", PowerPerc = 20 });
            snapshot3.Add(new WifiModel() { SSID = "CCC", PowerPerc = 80 });

            //Mock for places 
            PlaceModel p1 = new PlaceModel("Casa", snapshot1);
            PlaceModel p2 = new PlaceModel("Polito", snapshot2);
            PlaceModel p3 = new PlaceModel("Lavoro", snapshot3);


            places.Add(p1);
            places.Add(p2);
            places.Add(p3);
            //Set the source data for the Template View
            placesViewData.ItemsSource = places;


            currentState.Place = p1;
            vState.DataContext = currentState;
        }

        public class WifiModel
        {
            public string SSID { get; set; }

            public float Power { get; set; }
            public int PowerPerc { get; set; }

            public override string ToString()
            {
                return this.SSID + ": " + this.PowerPerc;
            }
        }

        //Model class has to implement the INotifyPropertyChanged interface
        //It reflects changes in the data objects
        //objects are capable of alerting the UI layer of changes to its properties
        //Note: change will happen on the bound data object itself and not the source list 
        public class PlaceModel : INotifyPropertyChanged
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

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

            public PlaceModel(string name, List<WifiModel> wifis)
            {
                this.Name = name;
                this.Wifis = wifis;
                this.Cnt = 0;
            }

            public int Cnt { get; set; }
            public List<WifiModel> Wifis { get; set; }
            public List<ActionModel> BeforeActions { get; set; }
            public List<ActionModel> InActions { get; set; }
            public List<ActionModel> AfterActions { get; set; }

            public override string ToString()
            {
                return this.Name;
            }
        }

        public class ActionModel
        {
            public string Name { get; set; }
        }

        private void btnChangePlace_Click(object sender, RoutedEventArgs e)
        {
            //cast of object to a type with as
            if (placesViewData.SelectedItem != null)
                (placesViewData.SelectedItem as PlaceModel).Name = "Random Name";
        }

        private void btnDeletePlace_Click(object sender, RoutedEventArgs e)
        {
            if (placesViewData.SelectedItem != null)
                places.Remove(placesViewData.SelectedItem as PlaceModel);
        }

        private void btnAddPlace_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 30);
            places.Add(new PlaceModel(randomNumber+"FFF" , wifis.ToList<WifiModel>()));
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            wifis.Clear();
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            for (var i = 0; i < randomNumber; i++)
            {
                wifis.Add(new WifiModel() { SSID = "AAAAAA", PowerPerc = random.Next(0, 100)});
                wifis.Add(new WifiModel() { SSID = "BBB", PowerPerc = random.Next(0, 100) });
                wifis.Add(new WifiModel() { SSID = "CCCCCCCCC", PowerPerc = random.Next(0, 100) });
            }

            currentState.Place = places.ElementAt(random.Next(0, places.Count));
        }
    }
}
