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
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using WhereAmI.models;

namespace WhereAmI.views
{
    /// <summary>
    /// Interaction logic for StatsView.xaml
    /// </summary>
    public partial class StatsView : UserControl
    {
        public StatsView()
        {
            InitializeComponent();
            
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();

            List<Place> places = DataManager.Instance.context.Places.Local.ToList();
            foreach (Place p in places)
            {
                items.Add(new KeyValuePair<string, int>(p.Name, (int)p.Cnt));
            }
            ((PieSeries)mcChart.Series[0]).ItemsSource = items.ToArray();
        }
    }
}
