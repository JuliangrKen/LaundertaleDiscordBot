using Discord;
using Discord.Interactions;

namespace LaundertaleDiscordBot.Commands.Everyone
{
    public class Coin : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("coin", "орёл или решка")]
        public async Task SlashCommand()
        {
            var result = new Random().Next(1, 100) > 50 ? 1 : 0;

            var res = new string[] { "Вам выпал орёл!", "Вам выпала решка!" };
            var url = new string[]
            {
                "https://media.discordapp.net/attachments/995650101336883241/995650130239823892/heads.png",
                "https://media.discordapp.net/attachments/995650101336883241/995650130537627678/tails.png"
            };

            var embedBuilder = new EmbedBuilder()
                .WithTitle(res[result])
                .WithDescription("Недеюсь, это то,\n чего вы ожидали :3")
                .WithColor(Program.defaultColor)
                .WithThumbnailUrl(url[result]);

            await RespondAsync(embed: embedBuilder.Build());
        }
    }
}