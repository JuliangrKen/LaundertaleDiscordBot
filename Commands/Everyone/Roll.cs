using Discord;
using Discord.Interactions;

namespace LaundertaleDiscordBot.Commands.Everyone
{
    public class Roll : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("roll", "получить случайное число")]
        public async Task SlashCommand(int Максимум = 100, int Минимум = 1)
        {
            if (Минимум > Максимум)
                (Минимум, Максимум) = (Максимум, Минимум);

            var result = new Random().Next(Минимум, Максимум);
            
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Результат: {result}")
                .WithColor(Program.defaultColor)
                .WithFooter($"Случайное число от {Минимум} до {Максимум}.");

            await RespondAsync(embed: embedBuilder.Build());
        }
    }
}