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
using System.Threading;

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
            //Data binding after loading data
            App.loadedDataHandlers += (delegate()
            {
                // After the data is loaded call the DbSet<T>.Local property  
                // to use the DbSet<T> as a binding source. 
                actionsViewData.ItemsSource = DataManager.Instance.context.Actions.Local;
                buttonApp.IsEnabled = true;
                buttonCommand.IsEnabled = true;
                buttonImage.IsEnabled = true;
            });

            //Save change after having changed only a cell, if datagrid is not readOnly
            actionsViewData.CommitEdit(DataGridEditingUnit.Cell, true);
            actionsViewData.SelectionChanged += actionsViewData_Selected;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            buttonDelete.IsEnabled = false;
            buttonRun.IsEnabled = false;
        }
        private void actionsViewData_Selected(object sender, RoutedEventArgs e)
        {
            buttonDelete.IsEnabled = true;
            buttonRun.IsEnabled = true;
        }

        private void editEnd(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataManager.Instance.safeSave();
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

            if (result == MessageBoxResult.Yes && actionsViewData.SelectedItem !=null)
            {
                var ctx = DataManager.Instance.context;
                ctx.Actions.Remove(actionsViewData.SelectedItem as models.Action);
                DataManager.Instance.safeSave();
                buttonDelete.IsEnabled = false;
            }
        }

        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            models.Action a = new models.Action(){Type = "cmd"};

            AddCommandDialogBox dlg = new AddCommandDialogBox();
            // Instantiate the dialog box
            // Configure the dialog box
            dlg.Owner = Window.GetWindow(this) as MainWindow; ;
            dlg.DataContext = a;

            // Open the dialog box modally 
            dlg.ShowDialog();

            //var ctx = DataManager.Instance.context;
            if (dlg.DialogResult == true)
            {
                var ctx = DataManager.Instance.context;
                ctx.Actions.Add(a);
                DataManager.Instance.safeSave();
            }
        }


        private void AddApp_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".exe"; // Default file extension
            dlg.Filter = "Exe Files (.exe)|*.exe|Script Files (.bat)|*.bat "; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                models.Action a = new models.Action() {
                    Type="app",
                    Name= System.IO.Path.GetFileNameWithoutExtension(filename),
                    Command=filename
                };
                var ctx = DataManager.Instance.context;
                ctx.Actions.Add(a);
                DataManager.Instance.safeSave();
            }
        }

        private void AddSetWallpaper_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".bmp"; // Default file extension
            dlg.Filter = "Image Files (*.bmp, *.jpg, *.jpeg, *.jpe, *.png) | *.bmp; *.jpg; *.jpeg; *.jpe; *.png"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                models.Action a = new models.Action()
                {
                    Type = "wallpaper",
                    Name = System.IO.Path.GetFileNameWithoutExtension(filename),
                    Command = filename
                };
                var ctx = DataManager.Instance.context;
                ctx.Actions.Add(a);
                DataManager.Instance.safeSave();
            }
        }

        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            ActionManager.execute(actionsViewData.SelectedItem as models.Action);
        }
    }
}
