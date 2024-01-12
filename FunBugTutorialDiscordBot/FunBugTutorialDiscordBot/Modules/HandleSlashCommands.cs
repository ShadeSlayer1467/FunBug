using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunBugTutorialDiscordBot.CardGames;
using System.Runtime.Remoting.Contexts;

namespace FunBugTutorialDiscordBot.Modules
{
    public class HandleSlashCommands
    {
        public string FieldA { get; set; } = "test";
        public int FieldB { get; set; } = 10;
        public bool FieldC { get; set; } = true;
        public async Task SlashCommandHandler(SocketSlashCommand command, DiscordSocketClient _client)
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
                case "feedback":
                    await HandleFeedbackCommand(command);
                    break; 
                case "random-card-game":
                    await HandleRandomCardGameCommand(command, _client);
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
        private async Task HandleFeedbackCommand(SocketSlashCommand command)
        {
            var embedBuilder = new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Feedback")
                .WithDescription($"Thanks for your feedback! You rated us {command.Data.Options.First().Value}/5")
                .WithColor(Color.Green)
                .WithCurrentTimestamp();

            await command.RespondAsync(embed: embedBuilder.Build());
        }
        private async Task HandleRandomCardGameCommand(SocketSlashCommand command, DiscordSocketClient _client)
        {
            var userCard = CardGames.Deck.GenerateRandomCard();
            var botCard = CardGames.Deck.GenerateRandomCard();
            var embedBuilder = new EmbedBuilder()
            {
                Title = "Random Card Game",
                Description = $"{command.User.GlobalName} drew {userCard} and the bot drew {botCard}",
                Color = Color.DarkRed
            };
            await command.RespondAsync(embed: embedBuilder.Build());

            if (userCard.Rank == botCard.Rank) await command.Channel.SendMessageAsync("It's a tie!");
            else await command.Channel.SendMessageAsync($"{((userCard.Rank > botCard.Rank) ? command.User.Mention : _client.GetUser(command.ApplicationId).Mention)} drew a higher card!");
            }
    }
}
