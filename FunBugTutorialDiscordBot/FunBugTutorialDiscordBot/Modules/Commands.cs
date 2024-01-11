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
        }
    }
}
