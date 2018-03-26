using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;

namespace Mimicry.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        Color erCol = new Color(221, 98, 98);
        Color okCol = new Color(251, 247, 173);
        Color sucCol = new Color(180, 224, 149);
        Telemetry tlm = new Telemetry();

        [Command("echo")]
        public async Task Echo([Remainder]string msg)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Message from " + Context.User.Username);
            embed.WithDescription(msg);
            embed.WithColor(okCol);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }
        
        [Command("halt")]
        public async Task Quit()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Closing connection with " + tlm.getIP() + "...");
            embed.WithColor(erCol);
            await Context.Channel.SendMessageAsync("", embed: embed);
            Environment.Exit(0);
        }

        [Command("getlog")]
        public async Task GetLog()
        {
            string logdata = tlm.LoadLog();
            try
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Log from " + tlm.getIP());
                embed.WithColor(okCol);
                embed.WithDescription(logdata);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            catch (ArgumentException)
            {
                await Context.Channel.SendFileAsync(Telemetry.mimlog, "Text is too large, sending as file.");
                throw;
            }
        }

        [Command("getseslog")]
        public async Task GetSesLog()
        {
            string logdata = tlm.LoadSesLog();
            await Context.Channel.SendMessageAsync(logdata);
        }

        [Command("clearlog")]
        public async Task ClearLog()
        {
            tlm.ClearLog();
            await Context.Channel.SendMessageAsync("Succesfully cleared log file");
        }

        [Command("exit")]
        public async Task Exit()
        {
            await Context.Channel.SendMessageAsync("Exiting @" + tlm.getIP() + "...");
            await Context.Channel.SendMessageAsync("Bye!");
            Environment.Exit(0);
        }
    }
}
