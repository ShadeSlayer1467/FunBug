using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FunBugTutorialDiscordBot
{
    internal class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            var JSONReader = new config.JSONReader();
            await JSONReader.ReadConfigJSON();

            var config = new DiscordSocketConfig()
            {
                // Other config options can be presented here.
                GatewayIntents = GatewayIntents.All
            };
            _client = new DiscordSocketClient(config);
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            string DISCORD_FUNBUG_TOKEN = JSONReader.token;
            _client.Log += _client_Log;
            _client.Ready += RegiserSlashCommands;
            await _client.LoginAsync(TokenType.Bot, DISCORD_FUNBUG_TOKEN);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
        public async Task RegiserSlashCommands()
        {
            var JSONReader = new config.JSONReader();
            await JSONReader.ReadConfigJSON();

            List<KeyValuePair<SlashCommandBuilder, ulong>> guildCommands = new List<KeyValuePair<SlashCommandBuilder, ulong>>()
            {
                new KeyValuePair<SlashCommandBuilder, ulong>(
                    new SlashCommandBuilder().WithName("first-command").WithDescription("This is my first guild slash command!"),
                    JSONReader.BasicGuildID),
                new KeyValuePair<SlashCommandBuilder, ulong>(
                    new SlashCommandBuilder()
                        .WithName("list-roles")
                        .WithDescription("Lists all roles of a user.")
                        .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true),
                    JSONReader.BasicGuildID),
                new KeyValuePair<SlashCommandBuilder, ulong>(
                    new SlashCommandBuilder()
                        .WithName("list-roles-hidden")
                        .WithDescription("Lists all roles of a user only to you.")
                        .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true),
                    JSONReader.BasicGuildID),
                new KeyValuePair<SlashCommandBuilder, ulong>(
                    new SlashCommandBuilder()
                        .WithName("settings")
                        .WithDescription("Changes some settings within the bot.")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithName("field-a")
                            .WithDescription("Gets or sets the field A")
                            .WithType(ApplicationCommandOptionType.SubCommandGroup)
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithName("set")
                                .WithDescription("Sets the field A")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                                .AddOption("value", ApplicationCommandOptionType.String, "the value to set the field", isRequired: true)
                            ).AddOption(new SlashCommandOptionBuilder()
                                .WithName("get")
                                .WithDescription("Gets the value of field A.")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                            )
                        ).AddOption(new SlashCommandOptionBuilder()
                            .WithName("field-b")
                            .WithDescription("Gets or sets the field B")
                            .WithType(ApplicationCommandOptionType.SubCommandGroup)
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithName("set")
                                .WithDescription("Sets the field B")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                                .AddOption("value", ApplicationCommandOptionType.Integer, "the value to set the fie to.", isRequired: true)
                            ).AddOption(new SlashCommandOptionBuilder()
                                .WithName("get")
                                .WithDescription("Gets the value of field B.")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                            )
                        ).AddOption(new SlashCommandOptionBuilder()
                            .WithName("field-c")
                            .WithDescription("Gets or sets the field C")
                            .WithType(ApplicationCommandOptionType.SubCommandGroup)
                            .AddOption(new SlashCommandOptionBuilder()
                                .WithName("set")
                                .WithDescription("Sets the field C")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                                .AddOption("value", ApplicationCommandOptionType.Boolean, "the value to set the fie to.", isRequired: true)
                            ).AddOption(new SlashCommandOptionBuilder()
                                .WithName("get")
                                .WithDescription("Gets the value of field C.")
                                .WithType(ApplicationCommandOptionType.SubCommand)
                            )
                        ),
                    JSONReader.BasicGuildID)
            };

            foreach (var guildCommand in guildCommands)
            {
                await AddSlashCommands.AddGuildSlashCommand(guildCommand.Value, _client, guildCommand.Key);
            }

            List<SlashCommandBuilder> globalCommands = new List<SlashCommandBuilder>()
            {
                new SlashCommandBuilder()
                    .WithName("first-global-command")
                    .WithDescription("This is my first global slash command")
            };

            foreach (var globalCommand in globalCommands)
            {
                await AddSlashCommands.AddGlobalSlashCommand(_client, globalCommand);
            }
        }
    }
}



/*
 * An app can have up to 25 subcommand groups on a top-level command
 * An app can have up to 25 subcommands within a subcommand group
 * commands can have up to 25 options
 * options can have up to 25 choices
 *
VALID

command
|
|__ subcommand
|
|__ subcommand

----

command
|
|__ subcommand-group
    |
    |__ subcommand
|
|__ subcommand-group
    |
    |__ subcommand


-------

INVALID


command
|
|__ subcommand-group
    |
    |__ subcommand-group
|
|__ subcommand-group
    |
    |__ subcommand-group

----

INVALID

command
|
|__ subcommand
    |
    |__ subcommand-group
|
|__ subcommand
    |
    |__ subcommand-group

 */