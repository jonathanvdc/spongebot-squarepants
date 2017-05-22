using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Spongebot
{
    /// <summary>
    /// Represents command-line arguments that have been parsed.
    /// </summary>
    public struct ParsedArguments
    {
        private ParsedArguments(string token)
        {
            this.Token = token;
        }

        /// <summary>
        /// Gets the Discord access token.
        /// </summary>
        /// <returns>The token.</returns>
        public string Token { get; private set; }

        /// <summary>
        /// Tries to parse the given command-line arguments.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="parsedArgs">The parsed arguments.</param>
        /// <returns><c>true</c> if the given arguments were parsed successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string[] args, out ParsedArguments parsedArgs)
        {
            if (args.Length != 1)
            {
                parsedArgs = default(ParsedArguments);
                return false;
            }

            parsedArgs = new ParsedArguments(args[0]);
            return true;
        }
    }

    /// <summary>
    /// Represents out bot.
    /// </summary>
    public sealed class Spongebot
    {
        private Spongebot(string token)
        {
            this.Token = token;
            this.client = new DiscordSocketClient();
            this.commands = new Dictionary<string, Command>()
            {
                { "mock", new MockCommand() },
                { "mockify", new MockifyCommand() }
            };
            this.commands.Add("help", new HelpCommand(commands));
        }

        private DiscordSocketClient client;

        /// <summary>
        /// Gets Spongebot's token.
        /// </summary>
        public string Token { get; private set; }

        private Dictionary<string, Command> commands;

        private Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith(">"))
            {
                string[] splitCommand = message.Content.Substring(1).Split(
                    new char[] { }, 2, StringSplitOptions.RemoveEmptyEntries);
                string commandName = splitCommand[0];
                string commandArg = splitCommand.Length > 1 ? splitCommand[1] : "";
                Command command;
                if (commands.TryGetValue(commandName, out command))
                {
                    command.Run(commandArg, message);
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Runs Spongebot forever.
        /// </summary>
        public void Run()
        {
            client.LoginAsync(TokenType.Bot, Token).Wait();
            client.StartAsync().Wait();
            Console.WriteLine("Spongebot's ready for action!");

            // Wait forever.
            Task.Delay(-1).Wait();
        }

        /// <summary>
        /// Creates a new Spongebot from the given access token.
        /// </summary>
        /// <param name="token">Spongebot's token.</param>
        /// <returns>A Spongebot instance.</returns>
        public static Spongebot Create(string token)
        {
            var bot = new Spongebot(token);
            bot.client.MessageReceived += bot.MessageReceived;
            return bot;
        }
    }

    public static class Program
    {
        public static int Main(string[] args)
        {
            ParsedArguments parsedArgs;
            if (!ParsedArguments.TryParse(args, out parsedArgs))
            {
                Console.Error.WriteLine("usage: spongebot-squarepants token");
                return 1;
            }

            Spongebot.Create(parsedArgs.Token).Run();
            return 0;
        }
    }
}