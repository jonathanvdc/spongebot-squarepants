using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Spongebot
{
    public sealed class HelpCommand : Command
    {
        public HelpCommand(IReadOnlyDictionary<string, Command> commands)
        {
            this.commands = commands;
        }

        private IReadOnlyDictionary<string, Command> commands;

        public override string Description => "print a description of the commands understood by Spongebot";

        public override string Usage => "";

        public override Task Run(string CommandArgument, SocketMessage Message)
        {
            var builder = new StringBuilder();
            builder.AppendLine("```");
            foreach (var pair in commands)
            {
                builder.Append(">");
                builder.Append(pair.Key);
                builder.Append(" ");
                builder.Append(pair.Value.Usage);
                builder.Append(" -- ");
                builder.Append(pair.Value.Description);
                builder.AppendLine();
            }
            builder.Append("```");
            return Message.Channel.SendMessageAsync(builder.ToString());
        }
    }
}