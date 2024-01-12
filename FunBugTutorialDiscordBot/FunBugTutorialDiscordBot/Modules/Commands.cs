using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using FunBugTutorialDiscordBot.CardGames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Interop;

namespace FunBugTutorialDiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }
        [Command("deltaalpha")]
        public async Task DeleteAll()
        {
            var channel = Context.Channel;
            var messages = await channel.GetMessagesAsync().FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            //send delete count
            var deleteCount = messages.Count();
            await ReplyAsync($"Deleted {deleteCount} messages.");
        }
        [Command("add")]
        public async Task Add(int num1, int num2)
        {
            await ReplyAsync(num1 + " + " + num2 + " = " + (num1 + num2));
        }
        [Command("embed")]
        public async Task Embed([Remainder]string input)
        {
            var embed = new EmbedBuilder()
            {
                Title = "Here is an embedded message",
                Description = $"{Context.User.GlobalName} said {input}",
                Color = Color.DarkRed
            };
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("randomcardgame")]
        public async Task RandomCardGame()
        {
            var userCard = CardGames.Deck.GenerateRandomCard();
            var botCard = CardGames.Deck.GenerateRandomCard();
            var embed = new EmbedBuilder()
            {
                Title = "Random Card Game",
                Description = $"{Context.User.GlobalName} drew {userCard} and the bot drew {botCard}",
                Color = Color.DarkRed
            };
            await Context.Channel.SendMessageAsync("", false, embed.Build());

            if (userCard.Rank > botCard.Rank)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.GlobalName} wins!");
            }
            else if (userCard.Rank < botCard.Rank)
            {
                await Context.Channel.SendMessageAsync("Bot wins!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("It's a tie!");
            }
        }
    }
    public class MyModule : InteractionModuleBase
    {
        private readonly MyService _service;

        public MyModule(MyService service)
        {
            _service = service;
        }

        [SlashCommand("things", "Shows things")]
        public async Task ThingsAsync()
        {
            var str = string.Join("\n", _service.Things);
            await RespondAsync(str);
        }
    }
    public class MyService
    {
        public List<string> Things { get; }

        public MyService()
        {
            List<string> list = new List<string>();
            Things = list;
        }
    }
}
