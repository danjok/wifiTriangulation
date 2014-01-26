using System;
using System.Collections.Generic;
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
    /// Interaction logic for PlacesView.xaml
    /// </summary>
    public partial class PlacesView : UserControl
    {
       
        public PlacesView()
        {
            InitializeComponent();
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            //MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            //dm = mainWindow.dm;
            placesViewData.ItemsSource = DataManager.Instance.places;
        }

        private void btnChangePlace_Click(object sender, RoutedEventArgs e)
        {
            //TODO register place with new wifis ?
            PlaceName.IsReadOnly = !PlaceName.IsReadOnly;
        }

        private void btnDeletePlace_Click(object sender, RoutedEventArgs e)
        {
            if (placesViewData.SelectedItem != null)
            {
                //places.Remove(placesViewData.SelectedItem as Place);
                placeViewDetail.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void btnResetStatPlace_Click(object sender, RoutedEventArgs e)
        {
            Place p = placesViewData.SelectedItem as Place;
            //p.Cnt = 0;
            //Only to test
            p.Cnt += 10;
        }

        private void placesViewData_Selected(object sender, RoutedEventArgs e)
        {
            placeViewDetail.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
