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
    /// Interaction logic for ActionsView.xaml
    /// </summary>
    public partial class ActionsView : UserControl
    {
        public ActionsView()
        {
            InitializeComponent();
            //Data binding at creation
            var ctx = DataManager.Instance.context;           
            // After the data is loaded call the DbSet<T>.Local property  
            // to use the DbSet<T> as a binding source. 
            actionsViewData.ItemsSource = ctx.Actions.Local;

            actionsViewData.CommitEdit(DataGridEditingUnit.Cell, true);
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            //placesViewData.ItemsSource = DataManager.Instance.places;
        }

        private void editEnd(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataManager.Instance.context.SaveChanges();
            //ctx.Entry<models.Action>(actionsViewData.SelectedItem as models.Action).Reload();
        }

        private void btnDeleteAction_Click(object sender, RoutedEventArgs e)
        {
            // Display message box
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.DeleteAction,
                Properties.Resources.DeleteActionTitle,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var ctx = DataManager.Instance.context;
                ctx.Actions.Remove(actionsViewData.SelectedItem as models.Action);
                ctx.SaveChanges();
            }
        }

    }
}
