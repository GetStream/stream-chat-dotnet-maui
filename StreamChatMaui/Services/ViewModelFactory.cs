using Microsoft.Extensions.Logging;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Services
{
    public class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IStreamChatService chatService, ReactionsRepository reactionsRepository, ILogger<MessageVM> messageLogger)
        {
            _chatService = chatService;
            _reactionsRepository = reactionsRepository;
            _messageLogger = messageLogger;
        }

        public MessageVM CreateMessageVM(IStreamMessage message)
            => new MessageVM(message, _chatService, _reactionsRepository, _messageLogger);

        private readonly IStreamChatService _chatService;
        private readonly ReactionsRepository _reactionsRepository;
        private readonly ILogger<MessageVM> _messageLogger;
    }
}
