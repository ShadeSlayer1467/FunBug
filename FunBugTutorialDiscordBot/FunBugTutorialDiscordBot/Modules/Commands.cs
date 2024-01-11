using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
