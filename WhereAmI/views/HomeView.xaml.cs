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
            //Data binding at creation
            wifiData.ItemsSource = DataManager.Instance.wifis;
            vState.DataContext = DataManager.Instance.currentState;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {     
        }

        private void btnAddPlace_Click(object sender, RoutedEventArgs e)
        {                     
            WizardDialog dlg = new WizardDialog();

            // Configure the dialog box
            dlg.Owner = Window.GetWindow(this) as MainWindow;
            
            // Open the dialog box modally 
            dlg.ShowDialog();         
        }

        //Refresh the system from UI manually
        //TODO 
        //refresh periodically
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.refresh();
        }
    }
}
