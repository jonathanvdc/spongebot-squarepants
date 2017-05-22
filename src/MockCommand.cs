using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.WebSocket;
using System.Text;

namespace Spongebot
{
    /// <summary>
    /// A command that mocks someone.
    /// </summary>
    public sealed class MockCommand : Command
    {
        public override string Description =>
            "mocks a chat message";

        public override string Usage => "username[^...|~...|^n|~n]";

        private const string MockImageUrl = 
            "http://cdn3-www.craveonline.com/assets/uploads/2017/05/mocking-spongebob.jpeg";

        public static string Mockify(string text)
        {
            var builder = new StringBuilder(text);
            bool inEmoticon = false;
            for (int i = 0; i < builder.Length; i++)
            {
                char c = builder[i];
                if (c == ':')
                {
                    inEmoticon = !inEmoticon;
                }
                else if (!inEmoticon)
                {
                    if (i % 2 == 0)
                    {
                        builder[i] = char.ToLower(c);
                    }
                    else
                    {
                        builder[i] = char.ToUpper(c);
                    }
                }
            }
            return builder.ToString();
        }

        public static string GenerateMockMessage(string author, string mockingAuthor, string message)
        {
            return string.Format(
                "**{0}:** {1}\n**{2}:** {3}\n{4}",
                author,
                message,
                mockingAuthor,
                Mockify(message),
                MockImageUrl);
        }

        public override async Task Run(string commandArgument, SocketMessage message)
        {
            var messageRef = MessageReference.Parse(commandArgument.Trim());
            var messageToMock = await messageRef.FindAsync(message.Channel);

            if (messageToMock == null)
            {
                await message.Channel.SendMessageAsync(
                    GenerateMockMessage(message.Author.Username, "Me", "mock " + commandArgument));
            }
            else
            {
                await message.Channel.SendMessageAsync(
                    GenerateMockMessage(messageRef.Author, message.Author.Username, messageToMock.Content));
            }
        }
    }

    /// <summary>
    /// A command that mocks someone saying something.
    /// </summary>
    public sealed class MockifyCommand : Command
    {
        public override string Description =>
            "mocks someone saying something";

        public override string Usage => "someone: something stupid";

        public override async Task Run(string commandArgument, SocketMessage message)
        {
            string[] split = commandArgument.Split(new char[] { ':' }, 2);
            if (split.Length == 1)
            {
                await message.Channel.SendMessageAsync(
                    MockCommand.GenerateMockMessage("Someone", message.Author.Username, split[0]));
            }
            else
            {
                await message.Channel.SendMessageAsync(
                    MockCommand.GenerateMockMessage(split[0], message.Author.Username, split[1].TrimStart()));
            }
        }
    }
}