using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;
using WhereAmI.models;
using WhereAmI.wlan;

namespace WhereAmI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer timer;
        public MainWindow()
        {
            //Init with data loading before view tab creations
            DataManager.Instance.init();
            InitializeComponent();
            
            /*
            dt = new DispatcherTimer();
            dt.Tick += new EventHandler(timer_Tick);
            dt.Interval = new TimeSpan(0, 0, 10);    
        
             */
            timer = new System.Timers.Timer();
            timer.Interval = 1000; //10 sec
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timerElapsed);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        /*
        private void timer_Tick(object sender, EventArgs e)
        {
            DataManager.Instance.refresh();
        }
        */

        void timerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                (System.Action)delegate()
                {
                    //System.Threading.Thread.Sleep(5000);
                    DataManager.Instance.refresh();
                });
        }
    }
}
