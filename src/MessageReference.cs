using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.WebSocket;
using System;

namespace Spongebot
{
    /// <summary>
    /// A reference to a chat message.
    /// </summary>
    public sealed class MessageReference
    {
        public MessageReference(string Author, int MessageIndex)
        {
            this.Author = Author;
            this.MessageIndex = MessageIndex;
        }

        public string Author { get; private set; }
        public int MessageIndex { get; private set; }

        public static MessageReference Parse(string Input)
        {
            string[] split = Input.Split(new char[] { '^', '~' }, 2);
            if (split.Length == 1)
            {
                return new MessageReference(Input, 0);
            }
            else
            {
                int index;
                if (!int.TryParse(split[1], out index))
                {
                    index = split[1].Length + 1;
                }
                return new MessageReference(split[0], index);
            }
        }

        public async Task<IMessage> FindAsync(ISocketMessageChannel Channel)
        {
            var allMessages = Channel.GetMessagesAsync(50)
                .SelectMany(xs => xs.ToAsyncEnumerable());

            IMessage refMessage = null;
            int i = 0;
            await allMessages.ForEachAsync(msg =>
            {
                if (msg.Author.Username.Equals(Author, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (i == MessageIndex)
                    {
                        refMessage = msg;
                    }
                    i++;
                }
            });
            return refMessage;
        }
    }
}