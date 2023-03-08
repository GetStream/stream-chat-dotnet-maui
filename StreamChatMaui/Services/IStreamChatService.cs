using StreamChat.Core;

namespace StreamChatMaui.Services
{
    /// <summary>
    /// Runs and provides an instance of the <see cref="IStreamChatClient"/>
    /// </summary>
    public interface IStreamChatService
    {
        /// <summary>
        /// Get <see cref="IStreamChatClient"/> instance once connected
        /// </summary>
        Task<IStreamChatClient> GetClientWhenReady();
    }
}
