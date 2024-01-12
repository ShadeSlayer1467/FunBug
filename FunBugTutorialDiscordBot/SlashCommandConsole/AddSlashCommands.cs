using Discord.Net;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace FunBugTutorialDiscordBot
{
    public class AddSlashCommands
    {
        public static async Task AddGuildSlashCommand(ulong GuildID, DiscordSocketClient _client, SlashCommandBuilder guildCommand)
        {
            try
            {
                await _client.Rest.CreateGuildCommand(guildCommand.Build(), GuildID);
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
        public static async Task AddGlobalSlashCommand(DiscordSocketClient _client, SlashCommandBuilder globalCommand)
        {
            try
            {
                await _client.Rest.CreateGlobalCommand(globalCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    }
}
