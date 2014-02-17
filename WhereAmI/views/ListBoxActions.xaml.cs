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
using System.Data.Entity;
using System.Collections;
using System.Collections.ObjectModel;
using WhereAmI.models;

namespace WhereAmI.views
{
    /// <summary>
    /// Interaction logic for ListBoxActions.xaml
    /// </summary>
    public partial class ListBoxActions : UserControl
    {
        ObservableCollection<models.Action> availableActions = new ObservableCollection<models.Action>();
        public Place selectedPlace { get; set; }

        public ListBoxActions()
        {
            InitializeComponent();
            lbAvailableActions.ItemsSource = availableActions;
        }

        public void computeAvailablePlaces()
        {
            if (this.selectedPlace != null)
            {
                var ctx = DataManager.Instance.context;
                var actionsIdList = from a in selectedPlace.InActions select a.ActionId;
                availableActions.Clear();
                ctx.Actions.Local.Where(a => !actionsIdList.Contains(a.ActionId)).ToList().ForEach(availableActions.Add);
            }
        }

        DataGrid dragSource = null;
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid parent = (DataGrid)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                var dragData = new DataObject(typeof(WhereAmI.models.Action), data);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(DataGrid source, Point point)
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
            DataManager.Instance.safeSave();
        }

        private void ListBox_DropActions(object sender, DragEventArgs e)
        {
            if (dragSource.Equals(sender))
                return;
            var dataObj = e.Data as DataObject;
            var data = dataObj.GetData(typeof(WhereAmI.models.Action)) as WhereAmI.models.Action;
            availableActions.Remove(data);
            selectedPlace.InActions.Add(data);
            DataManager.Instance.safeSave();
        }
    }
}
