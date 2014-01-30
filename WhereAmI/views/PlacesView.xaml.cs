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
using System.Data.Entity;

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
            //placesViewData.ItemsSource = DataManager.Instance.places;
            
            // Load is an extension method on IQueryable,  
            // defined in the System.Data.Entity namespace. 
            // This method enumerates the results of the query,  
            // similar to ToList but without creating a list. 
            // When used with Linq to Entities this method  
            // creates entity objects and adds them to the context.
            var ctx = DataManager.Instance.context;
            ctx.Places.Load();
            
            // After the data is loaded call the DbSet<T>.Local property  
            // to use the DbSet<T> as a binding source. 
            placesViewData.ItemsSource = ctx.Places.Local;
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
                var ctx = DataManager.Instance.context;
                ctx.Places.Remove(placesViewData.SelectedItem as Place);
                ctx.SaveChangesAsync(); 

                //To hide UI buttons
                placeViewDetail.Visibility = System.Windows.Visibility.Hidden;
                //this.placesViewData.Items.Refresh();
            }
        }

        private void btnResetStatPlace_Click(object sender, RoutedEventArgs e)
        {
            Place p = placesViewData.SelectedItem as Place;
            //Only to test
            p.Cnt += 10;
            //p.Cnt = 0;
           
            var ctx = DataManager.Instance.context;  
            var pEntity = ctx.Places.Find(p.PlaceId);
            ctx.Entry(pEntity).Property(v => v.Cnt).CurrentValue = p.Cnt;
            ctx.SaveChangesAsync();          
           }

        private void placesViewData_Selected(object sender, RoutedEventArgs e)
        {
            placeViewDetail.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
