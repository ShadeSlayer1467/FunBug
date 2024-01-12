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
using FunBugTutorialDiscordBot.Modules;
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
            await JSONReader.ReadJSON();

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
            await RegisterCommandsAsync();
            //----------------------------------------------------
            // this line has been moved to a new exe
            //_client.Ready += RegiserSlashCommands;
            // ---------------------------------------------------
            await _client.LoginAsync(TokenType.Bot, DISCORD_FUNBUG_TOKEN);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.SlashCommandExecuted += SlashCommandHandler;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        public async Task RegiserSlashCommands()
        {
            var JSONReader = new config.JSONReader();
            await JSONReader.ReadJSON();

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
        private Task HandleCommandAsync(SocketMessage arg)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var message = arg as SocketUserMessage;
                    var context = new SocketCommandContext(_client, message);
                    if (message.Author.IsBot) return;

                    int argPos = 0;
                    var JSONReader = new config.JSONReader();
                    await JSONReader.ReadJSON();
                    if (message.HasStringPrefix(JSONReader.prefix, ref argPos))
                    {
                        var result = await _commands.ExecuteAsync(context, argPos, _services);
                        if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            return Task.CompletedTask;
        }
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            HandleSlashCommands slashCommands = new HandleSlashCommands();
            await slashCommands.SlashCommandHandler(command);
        }
    }
}
