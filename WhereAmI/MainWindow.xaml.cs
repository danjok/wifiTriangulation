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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using WhereAmI.models;

namespace WhereAmI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool closing = false;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip notifyMenu = new System.Windows.Forms.ContextMenuStrip();
        
        public MainWindow()
        {
            InitializeComponent();
            BackgroundWork.placeChangedHandlers += ((BackgroundWork.onPlaceChanged) this.notifyPlaceChanged);
            BackgroundWork.messageRefreshHandlers += (delegate(string msg)
            {
                Dispatcher.Invoke(delegate()
                {
                    Status.Text = msg;
                });
            });
            

            
            // Add menu items to context menu.
            ToolStripItem show = notifyMenu.Items.Add("Show");
            ToolStripItem exit = notifyMenu.Items.Add("Exit");
            show.Click += showInterface;
            exit.Click += exitApplication;

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.ContextMenuStrip = notifyMenu;
            notifyIcon.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Information, 40, 40);
            notifyIcon.Visible = false;

            System.Windows.Forms.MouseEventHandler meh;
            meh = clickNotifyIcon;
            notifyIcon.MouseClick += meh;
        }

        private void notifyPlaceChanged(Place p)
        {
            notifyIcon.BalloonTipText = "Entering "+p.Name;
            notifyIcon.ShowBalloonTip(1000); 
        }
        private void clickNotifyIcon(object sender, System.Windows.Forms.MouseEventArgs arg)
        {

            if (arg.Button == MouseButtons.Left)
            {
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
            }
            if (arg.Button == System.Windows.Forms.MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip.Show();
            }

        }

        private void showInterface(object sender, EventArgs arg)
        {

            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon.Visible = false;

        }

        private void exitApplication(object sender, EventArgs arg)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
            closing = true;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            notifyIcon.BalloonTipText = "hidden";
            notifyIcon.ShowBalloonTip(1000); 
            if (WindowState == WindowState.Minimized)
                notifyIcon.Visible = false;
            if (WindowState == WindowState.Normal)
            {
                notifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!closing)
            {
                WindowState = WindowState.Minimized;
                ShowInTaskbar = false;
                notifyIcon.Visible = true;
                base.OnClosing(e);
                e.Cancel = true;
            }


        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
    
        }
    }
}
