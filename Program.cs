using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Input;

namespace LaundertaleDiscordBot
{
    internal class Program
    {
        static Task Main() => new Program().MainAsync();

        public static readonly Color defaultColor = Color.Purple;

        #region Token, Prefix & GuildID

        const string token = @"ODg0MTA3OTAzOTk5NTQ1NDA0.GD7EOX.DODH8C3XouHHuYOy-RwGXGDbY-1xuA66tqU2G4";
        const string prefix = "!";
        const ulong guildId = 997081753623732236;

        #endregion

        DiscordSocketClient? client;
        CommandService? commands;
        IServiceProvider? services;
        InteractionService? interactionService;

        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            services = new ServiceCollection()
                .AddSingleton(client)
                .BuildServiceProvider();
            commands = new CommandService();

            client.Log += Log;
            client.Ready += HandleSlashCommandAsync;
            client.MessageReceived += HandleCommandAsyncWithPrefix;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            
            await client.LoginAsync(TokenType.Bot, token);
            await client.SetStatusAsync(UserStatus.AFK);
            await client.SetGameAsync("крики маленьких девочек :3", null, ActivityType.Listening);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task HandleSlashCommandAsync()
        {
            interactionService = new InteractionService(client);
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await interactionService.RegisterCommandsToGuildAsync(guildId);

            if (client != null && services != null)
                client.InteractionCreated += async interaction =>
                {
                    var scope = services.CreateScope();
                    var ctx = new SocketInteractionContext(client, interaction);
                    await interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
                };
        }

        private async Task HandleCommandAsyncWithPrefix(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message != null && message.Author.IsBot)
                return;
            int argPos = 0;
            if (message != null && message.HasStringPrefix(prefix, ref argPos) && commands != null)
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                    await message.Channel.SendFileAsync(result.ErrorReason);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}