using Discord;
using Discord.Interactions;
using LaundertaleDiscordBot;

namespace src.Modules.PublicCommands.Commands
{
    public class Roll : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("roll", "получить случайное число")]
        public async Task SlashCommand(int Максимум = 100, int Минимум = 1)
        {
            if (Минимум > Максимум)
                (Минимум, Максимум) = (Максимум, Минимум);
            var random = new Random();
            var result = random.Next(Минимум, Максимум);
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Результат: {result}")
                .WithColor(Program.defaultColor)
                .WithFooter($"Случайное число от {Минимум} до {Максимум}.");

            await RespondAsync(embed: embedBuilder.Build());
        }
    }
}