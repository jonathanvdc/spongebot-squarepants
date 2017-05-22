using System;
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
            this.client = new DiscordSocketClient();
            this.Token = token;
        }

        private DiscordSocketClient client;

        /// <summary>
        /// Gets Spongebot's token.
        /// </summary>
        public string Token { get; private set; }

        private Task MessageReceived(SocketMessage message)
        {
            if (message.Content == ">mock")
            {
                return message.Channel.SendMessageAsync(
                    "![Mocking Spongebob](http://cdn3-www.craveonline.com/assets/uploads/2017/05/mocking-spongebob.jpeg)");
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Runs Spongebot forever.
        /// </summary>
        public void Run()
        {
            client.LoginAsync(TokenType.Bot, Token).Wait();
            client.StartAsync().Wait();
            Console.WriteLine("Spongebot's ready for action!");
            while (true)
            { }
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