using System.Threading.Tasks;
using Discord.WebSocket;

namespace Spongebot
{
    /// <summary>
    /// A base type for commands.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Gets a description of this command.
        /// </summary>
        /// <returns>A textual description.</returns>
        public abstract string Description { get; }

        /// <summary>
        /// Gets this command's format.
        /// </summary>
        /// <returns>The command's format.</returns>
        public abstract string Usage { get; }

        /// <summary>
        /// Runs this command on the given message.
        /// </summary>
        /// <param name="commandArgument">The command's argument string.</param>
        /// <param name="message">The message that prompted this command.</param>
        /// <returns>A task.</returns>
        public abstract Task Run(string commandArgument, SocketMessage message);
    }
}