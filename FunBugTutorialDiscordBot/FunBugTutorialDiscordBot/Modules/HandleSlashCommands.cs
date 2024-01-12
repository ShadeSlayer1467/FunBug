using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunBugTutorialDiscordBot.Modules
{
    public class HandleSlashCommands
    {
        public string FieldA { get; set; } = "test";
        public int FieldB { get; set; } = 10;
        public bool FieldC { get; set; } = true;
        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "list-roles":
                    await HandleListRoleCommand(command, false);
                    break;
                case "list-roles-hidden":
                    await HandleListRoleCommand(command, true);
                    break;
                case "settings":
                    await HandleSettingsCommand(command);
                    break;
                default:
                    await command.RespondAsync($"You executed {command.Data.Name}");
                    break;
            }
        }

        public async Task HandleListRoleCommand(SocketSlashCommand command, bool hidden)
        {
            // get user
            var guildUser = (SocketGuildUser)command.Data.Options.First().Value;

            var roleList = string.Join(",\n", guildUser.Roles.Where(x => !x.IsEveryone).Select(x => x.Mention));

            var embedBuiler = new EmbedBuilder()
                .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
                .WithTitle("Roles")
                .WithDescription(roleList)
                .WithColor(Color.Green)
                .WithCurrentTimestamp();
            await command.RespondAsync(embed: embedBuiler.Build(), ephemeral: hidden );
        }
        private async Task HandleSettingsCommand(SocketSlashCommand command)
        {

            // First lets extract our variables
            var fieldName = command.Data.Options.First().Name;
            var getOrSet = command.Data.Options.First().Options.First().Name;
            // Since there is no value on a get command, we use the ? operator because "Options" can be null.
            var temp = command.Data.Options.First().Options.First().Options;
            var value = ((temp.Count != 0) ? temp.FirstOrDefault().Value: null);

            switch (fieldName)
            {
                case "field-a":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-a` is `{FieldA}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldA = (string)value;
                            await command.RespondAsync($"`field-a` has been set to `{FieldA}`");
                        }
                    }
                    break;
                case "field-b":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-b` is `{FieldB}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldB = (int)value;
                            await command.RespondAsync($"`field-b` has been set to `{FieldB}`");
                        }
                    }
                    break;
                case "field-c":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-c` is `{FieldC}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldC = (bool)value;
                            await command.RespondAsync($"`field-c` has been set to `{FieldC}`");
                        }
                    }
                    break;
            }
        }
    }
}
