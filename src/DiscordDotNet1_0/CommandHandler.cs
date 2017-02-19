using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System;

namespace DiscordExampleBot
{
    public class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IDependencyMap map;

        public async Task Install(IDependencyMap _map)
        {
            // Create Command Service, inject it into Dependency Map
            client = _map.Get<DiscordSocketClient>();
            commands = new CommandService();
            _map.Add(commands);
            map = _map;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

            client.MessageReceived += HandleCommand;
            
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            var message = parameterMessage as SocketUserMessage;

            if (message == null) return;

            int argPos = 0;

            if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('!', ref argPos))) return;
            
            var context = new CommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, map);
            
            if (!result.IsSuccess)
                await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        }

    }
}
