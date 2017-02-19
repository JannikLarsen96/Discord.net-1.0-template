using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordExampleBot.Modules.Public;
using System.Threading;
using System.Linq;

namespace DiscordExampleBot
{
    public class Program
    {
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandHandler handler;

        PublicModule module = new PublicModule();

        DateTime startTime = DateTime.Now;

        public async Task Start()
        {
            client = new DiscordSocketClient();

            await client.LoginAsync(TokenType.Bot, "TOKEN HERE");
            await client.ConnectAsync();

            var map = new DependencyMap();
            map.Add(client);
             
            handler = new CommandHandler();
            await handler.Install(map);
            
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}
