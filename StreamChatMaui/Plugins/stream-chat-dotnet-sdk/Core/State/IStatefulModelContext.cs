using StreamChat.Core.State.Caches;
using StreamChat.Libs.Loggers;
using StreamChat.Libs.Serialization;

namespace StreamChat.Core.State
{
    internal interface IStatefulModelContext
    {
        ICache Cache { get; }
        StreamChatClient Client { get; }
        ILogs Logs { get; }
        ISerializer Serializer { get; }
    }
}