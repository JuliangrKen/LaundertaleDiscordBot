using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LaundertaleDiscordBot
{
    internal class Program
    {
        static Task Main() => new Program().MainAsync();

        public static readonly Discord.Color defaultColor = Discord.Color.Purple;

        #region Token & GuildID

        const string token = @"ODg0MTA3OTAzOTk5NTQ1NDA0.GD7EOX.DODH8C3XouHHuYOy-RwGXGDbY-1xuA66tqU2G4";
        const ulong guildId = 997081753623732236;

        #endregion

        DiscordSocketClient? client;
        IServiceProvider? services;
        InteractionService? interactionService;

        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            services = new ServiceCollection()
                .AddSingleton(client)
                .BuildServiceProvider();

            client.Log += Log;
            client.Ready += HandleSlashCommandAsync;

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

        private Task Log(Discord.LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}