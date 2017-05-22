using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Spongebot
{
    public sealed class SleepCommand : Command
    {
        public SleepCommand(string parent)
        {
            this.Parent = parent;
        }

        public string Parent { get; private set; }

        public override string Description => "makes this bot go to sleep";

        public override string Usage => "you rest in a deep and dreamless slumber";

        public override Task Run(string commandArgument, SocketMessage message)
        {
            if (commandArgument == Usage && message.Author.Username == Parent)
            {
                Environment.Exit(0);
                return Task.CompletedTask;
            }
            else
            {
                return message.Channel.SendMessageAsync(
                    MockCommand.GenerateMockMessage(
                        message.Author.Username, "Spongebot", message.Content.TrimStart('>')));
            }
        }
    }
}