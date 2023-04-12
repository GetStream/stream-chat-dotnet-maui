
using StreamChat.Core;
using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.Services
{
    public class ChatPermissionsService : IChatPermissionsService
    {
        public bool IsReady => _chatClient != null;

        public ChatPermissionsService(IStreamChatService chatService)
        {
            _chatService = chatService;

            _chatService.GetClientWhenReadyAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine(t.Exception.Message);
                    return;
                }

                _chatClient = t.Result;
            });
        }

        public bool CanDelete(IStreamMessage message) => IsOwnMessage(message) || IsAdminOrModerator();
        public bool CanEdit(IStreamMessage message) => IsOwnMessage(message) || IsAdminOrModerator();

        private readonly IStreamChatService _chatService;
        private IStreamChatClient _chatClient;

        private void AssertReady()
        {
            if (!IsReady)
            {
                throw new InvalidOperationException("Service is not ready to use");
            }
        }

        private bool IsOwnMessage(IStreamMessage message) => message.User == _chatClient.LocalUserData.User;

        private bool IsAdminOrModerator()
        {
            AssertReady();
            var role = _chatClient.LocalUserData.User.Role.ToLower();
            return role == "admin" || role == "moderator";
        }
    }
}
