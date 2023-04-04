using Microsoft.Extensions.Logging;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Services
{
    public class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IStreamChatService chatService, ILogger<MessageVM> messageLogger)
        {
            _chatService = chatService;
            _messageLogger = messageLogger;
        }

        public MessageVM CreateMessageVM(IStreamMessage message) 
            => new MessageVM(message, _chatService, _messageLogger);

        private readonly IStreamChatService _chatService;
        private readonly ILogger<MessageVM> _messageLogger;
    }
}
