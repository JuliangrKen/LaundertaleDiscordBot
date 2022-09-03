using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace LaundertaleDiscordBot.Commands.Everyone
{
    public class Roll
    {
        public static EmbedBuilder RollLogic(int Максимум, int Минимум)
        {
            if (Минимум > Максимум)
                (Минимум, Максимум) = (Максимум, Минимум);

            var result = new Random().Next(Минимум, Максимум);

            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Результат: {result}")
                .WithColor(Program.defaultColor)
                .WithFooter($"Случайное число от {Минимум} до {Максимум}.");

            return embedBuilder;
        }

        public class SlashCommand : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("roll", "получить случайное число")]
            public async Task Main(int Максимум = 100, int Минимум = 1)
            {
                await RespondAsync(embed: RollLogic(Максимум, Минимум).Build());
            }
        }

        public class PrefixCommands : ModuleBase<SocketCommandContext>
        {
            [Command("roll")]
            public async Task Main(int max, int min)
            {
                var message = Context.Message;
                await message.ReplyAsync(embed: RollLogic(max, min).Build());
            }

            [Command("roll")]
            public async Task Main(int max) => await Main(max, 1);

            [Command("roll")]
            public async Task Main() => await Main(100, 1);
        }
    }
}