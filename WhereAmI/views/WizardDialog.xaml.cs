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
using System.Windows.Shapes;
using WhereAmI.models;

namespace WhereAmI.views
{
    /// <summary>
    /// Interaction logic for WizardDialog.xaml
    /// </summary>
    public partial class WizardDialog : Window
    {
        public Place p { get; set; }
        public WizardDialog()
        {
            InitializeComponent();
            p = new Place();
            this.DataContext = p;
            this.listActions.selectedPlace = p;
            this.listActions.computeAvailablePlaces();
        }

        void okButton_Click(object sender, RoutedEventArgs e)
        {
            //To manually trigger update if no property has changed
            BindingExpression be = this.PlaceNameTextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();

            // Don't accept the dialog box if there is invalid data
            if (!IsValid(this)) return;

            var ctx = DataManager.Instance.context;
            DataManager.Instance.loadWifis();
            p.Snapshot = Place.serializationSnapshot(DataManager.Instance.wifis.ToList<Wifi>());
            ctx.Places.Add(p);
            ctx.SaveChanges();

            // Dialog box accepted
            this.DialogResult = true;
        }

        // Validate all dependency objects in a window
        bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
        }
    }
}
