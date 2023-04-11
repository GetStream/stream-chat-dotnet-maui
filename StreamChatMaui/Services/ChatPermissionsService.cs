
using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.Services
{
    public class ChatPermissionsService : IChatPermissionsService
    {
        public ChatPermissionsService(IStreamChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public async Task<bool> CanDeleteMessageAsync(IStreamMessage message)
        {
            var client = await _chatService.GetClientWhenReadyAsync();

            if(message.User == client.LocalUserData.User)
            {
                return true;
            }

            var role = client.LocalUserData.User.Role.ToLower();
            if (role == "admin" || role == "moderator")
            {
                return true;
            }

            return false;
        }

        private readonly IStreamChatService _chatService;
    }
}
