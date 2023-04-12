
using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.Commands
{
    /// <summary>
    /// Args for add or remove reaction command
    /// </summary>
    public readonly struct AddOrRemoveReactionCommandArgs
    {
        public readonly IStreamMessage Message;
        public readonly string Reaction;

        public AddOrRemoveReactionCommandArgs(IStreamMessage message, string reaction)
        {
            Message = message;
            Reaction = reaction;
        }
    }
}
