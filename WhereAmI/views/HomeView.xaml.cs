using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WhereAmI.views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            //MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            //dm = mainWindow.dm;

            wifiData.ItemsSource = DataManager.Instance.wifis;
            vState.DataContext = DataManager.Instance.currentState;
        }

        private void btnAddPlace_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 30);
            DataManager.Instance.places.Add(new Place(randomNumber + "FFF", DataManager.Instance.wifis.ToList<Wifi>()));
        }

        //Refresh the system from UI manually
        //TODO 
        //refresh periodically
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.loadWifis();
            DataManager.Instance.computeCurrentPlace();
        }
    }
}
