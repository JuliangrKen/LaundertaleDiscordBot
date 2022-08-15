using Discord;
using Discord.Interactions;
using LaundertaleDiscordBot;

namespace src.Modules.PublicCommands.Commands
{
    public class Coin : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("coin", "орёл или решка")]
        public async Task SlashCommand()
        {
            var result = new Random().Next(1, 100);

            if (result > 50) result = 1;
            else result = 0;

            string[] res = { "Вам выпал орёл!", "Вам выпала решка!" };
            string[] url = new string[2];
            url[0] = "https://media.discordapp.net/attachments/995650101336883241/995650130239823892/heads.png";
            url[1] = "https://media.discordapp.net/attachments/995650101336883241/995650130537627678/tails.png";

            var embedBuilder = new EmbedBuilder()
                .WithTitle(res[result])
                .WithDescription("Недеюсь, это то,\n чего вы ожидали :3")
                .WithColor(Program.defaultColor)
                .WithThumbnailUrl(url[result]);

            await RespondAsync(embed: embedBuilder.Build());
        }
    }
}