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
using WhereAmI.models;

namespace WhereAmI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class CurrentState : INotifyPropertyChanged
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
        CurrentState currentState = new CurrentState();

        //A list that notifies any destinations of changes to its content
        //It works only when an item is added or deleted
        //It reflects changes in the list data source
        private ObservableCollection<Place> places = new ObservableCollection<Place>();
        private ObservableCollection<Wifi> wifis = new ObservableCollection<Wifi>();

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

            places.Add(p1);
            places.Add(p2);
            places.Add(p3);  
        }

        private void bindData()
        {   
            //Bind the source data for the template View
            wifiData.ItemsSource = wifis;
            placesViewData.ItemsSource = places;
            vState.DataContext = currentState;
        }

        public MainWindow()
        {
            InitializeComponent();
            loadData();
            bindData();
        }

        private void btnChangePlace_Click(object sender, RoutedEventArgs e)
        {
            //cast of object to a type with as
            if (placesViewData.SelectedItem != null)
                (placesViewData.SelectedItem as Place).Name = "Random Name";
        }

        private void btnDeletePlace_Click(object sender, RoutedEventArgs e)
        {
            if (placesViewData.SelectedItem != null)
            {
                places.Remove(placesViewData.SelectedItem as Place);
            }
        }

        private void btnAddPlace_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 30);
            places.Add(new Place(randomNumber+"FFF" , wifis.ToList<Wifi>()));
        }

        private void btnResetStatPlace_Click(object sender, RoutedEventArgs e)
        {
            Place p = placesViewData.SelectedItem as Place;
            p.Cnt += 10;
        }

        //Refresh the system from UI manually
        //TODO 
        //refresh periodically
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            readCurrentWifis();
            computeCurrentPlace();
        }

        //Retrieve the current place from the list of recorded places
        //using the triangulation algorithm 
        //that receives in input the list of current detected wifis
        private void computeCurrentPlace()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            currentState.Place = places.ElementAt(random.Next(0, places.Count));
            //TODO
            //currentState.Place = triangAlgo(places, wifis);
        }

        //Read the current wifis networks connections from the WLAN API
        //update the wifis list
        private void readCurrentWifis()
        {
            wifis.Clear();
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            for (var i = 0; i < randomNumber; i++)
            {
                wifis.Add(new Wifi() { SSID = "AAAAAA", PowerPerc = random.Next(0, 100) });
                wifis.Add(new Wifi() { SSID = "BBB", PowerPerc = random.Next(0, 100) });
                wifis.Add(new Wifi() { SSID = "CCCCCCCCC", PowerPerc = random.Next(0, 100) });
            }
            //TODO
            //create wifi models for wifis list using WLAN API
        }

    }
}
