using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using WhereAmI.models;

namespace WhereAmI
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow mw;
        private BackgroundWorker loadingWorker;
        private Thread refreshThread;
        
        void AppStartup(object sender, StartupEventArgs e)
        {
            DataManager.Instance.init();
            ThreadStart ts = new ThreadStart(BackgroundWork.Execute);
            refreshThread = new Thread(ts);
            refreshThread.IsBackground = true;

            /**
             * Main Window
             */
            mw = new MainWindow();
            mw.Show();
            refreshThread.Start();
            
            return;
            
            //TODO
            /**
             * Loading worker
             */
            loadingWorker = new BackgroundWorker();
            loadingWorker.DoWork += delegate(object ss, DoWorkEventArgs ee)
            {
                mw.Dispatcher.Invoke(delegate()
                {
                    mw.Status.Text = "Loading data...";
                });
                DataManager.Instance.doLoad();    
            };

            loadingWorker.RunWorkerCompleted += delegate(object ss, RunWorkerCompletedEventArgs ee)
            {
                //UI thread
                mw.Status.Text = "Loading completed successfully!";
                refreshThread.Start();
            };
            loadingWorker.RunWorkerAsync();

        }

    }
}
