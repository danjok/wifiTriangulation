using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WhereAmI
{
    class ActionManager
    {
        static public void execute(models.Action a)
        {
            try{
                switch(a.Type){
                case "cmd":
                    startProcessCmdLine(a.Command);
                    break;
                case "app":
                    startProcess(a.Command);
                    break;
                case "wallpaper":
                    changeWallpaper(a.Command);
                    break;
                }
            }catch(Exception e){
                System.Windows.MessageBox.Show(e.Message,"Error action: "+a.Name);
            }
        }

        private static void startProcess(string stringProc)
        {
            Process p = new Process();
            p.StartInfo.FileName = stringProc;
            p.Start();
        }

        private static void startProcessCmdLine(string cmd)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/C "+cmd;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            if (cmd.Contains("netsh"))
                proc.StartInfo.Verb = "runas";
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
        }

       [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        private static void changeWallpaper(string file)
        {

            if (File.Exists(file) == false)
            {
                throw new FileNotFoundException();
            }

            RegistryKey rkWallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

            if (rkWallPaper != null)
            {
                //WallpaperStyle.Stretched:
                rkWallPaper.SetValue(@"WallpaperStyle", 2.ToString());
                rkWallPaper.SetValue(@"TileWallpaper", 0.ToString());
            }

                //Set wallpaper 
                SystemParametersInfo(20, 0, file, 0x01 | 0x02);

                rkWallPaper.Close();
            }
        }
}
