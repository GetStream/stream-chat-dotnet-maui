using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.Services
{
    /// <summary>
    /// Service for checking if certain actions are allowed to execute in chat
    /// </summary>
    public interface IChatPermissionsService
    {
        bool IsReady { get; }

        bool CanDelete(IStreamMessage message);
        bool CanEdit(IStreamMessage message);
    }
}
