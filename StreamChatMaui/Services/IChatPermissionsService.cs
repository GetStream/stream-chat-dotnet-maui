using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.Services
{
    public interface IChatPermissionsService
    {
        Task<bool> CanDeleteMessageAsync(IStreamMessage message);
    }
}
