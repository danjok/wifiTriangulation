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
using System.Collections.ObjectModel;
using System.Collections;

namespace WhereAmI.views
{
    /// <summary>
    /// Interaction logic for PlacesView.xaml
    /// </summary>
    public partial class PlacesView : UserControl
    {
        ObservableCollection<models.Action> availableActions = new ObservableCollection<models.Action>();
        Place selectedPlace;        
        public PlacesView()
        {
            InitializeComponent();
            lbAvailableActions.ItemsSource = availableActions;
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
            //To update the list Actions-Place.InActions when Actions is modified
            computeAvailablePlaces();
        }

        private void btnChangePlace_Click(object sender, RoutedEventArgs e)
        {
            string name=selectedPlace.Name;
            // Instantiate the dialog box
            EditDialogBox dlg = new EditDialogBox();

            // Configure the dialog box
            dlg.Owner = Window.GetWindow(this) as MainWindow;;
            dlg.DataContext = selectedPlace;

            // Open the dialog box modally 
            dlg.ShowDialog();

            var ctx = DataManager.Instance.context;
            if (dlg.DialogResult == true)
            {
                ctx.SaveChanges();
            }
            else
            {
                selectedPlace.Name = name;
                ctx.SaveChanges();
                //ctx.Entry<Place>(placesViewData.SelectedItem as Place).Reload();
            }
        }

        private void btnDeletePlace_Click(object sender, RoutedEventArgs e)
        {
            // Display message box
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.DeletePlace,
                Properties.Resources.DeletePlaceTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                //places.Remove(placesViewData.SelectedItem as Place);
                var ctx = DataManager.Instance.context;
                ctx.Places.Remove(selectedPlace);
                ctx.SaveChangesAsync();

                //To hide UI buttons
                placeViewDetail.Visibility = System.Windows.Visibility.Hidden;           
            }
        }

        private void btnResetStatPlace_Click(object sender, RoutedEventArgs e)
        {
            // Display message box
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.ResetCntPlace,
                Properties.Resources.ResetCntPlaceTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                selectedPlace.Cnt = 0;
                var ctx = DataManager.Instance.context;
                ctx.SaveChangesAsync();
            }
        }

        private void computeAvailablePlaces()
        {
            if (selectedPlace != null)
            {
                var ctx = DataManager.Instance.context;
                var actionsIdList = from a in selectedPlace.InActions select a.ActionId;
                availableActions.Clear();
                ctx.Actions.Where(a => !actionsIdList.Contains(a.ActionId)).ToList().ForEach(availableActions.Add);
            }
        }

        private void placesViewData_Selected(object sender, RoutedEventArgs e)
        {
            placeViewDetail.Visibility = System.Windows.Visibility.Visible;
            selectedPlace = placesViewData.SelectedItem as Place;
            computeAvailablePlaces();
        }


        ListBox dragSource = null;
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                var dragData = new DataObject(typeof(WhereAmI.models.Action), data);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }
        
        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }
        #endregion

        private void ListBox_DropAvailableActions(object sender, DragEventArgs e)
        {
            if (dragSource.Equals(sender))
                return;
            var dataObj = e.Data as DataObject;
            var data = dataObj.GetData(typeof(WhereAmI.models.Action)) as WhereAmI.models.Action;
            selectedPlace.InActions.Remove(data);
            availableActions.Add(data);
            DataManager.Instance.context.SaveChanges();
        }

        private void ListBox_DropActions(object sender, DragEventArgs e)
        {
            if (dragSource.Equals(sender))
                return;
            var dataObj = e.Data as DataObject;
            var data = dataObj.GetData(typeof(WhereAmI.models.Action)) as WhereAmI.models.Action;
            availableActions.Remove(data);
            selectedPlace.InActions.Add(data);
            DataManager.Instance.context.SaveChanges();
        }
    }
}
