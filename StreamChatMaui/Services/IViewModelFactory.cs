using StreamChat.Core.StatefulModels;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Services
{
    public interface IViewModelFactory
    {
        MessageVM CreateMessageVM(IStreamMessage message);
    }
}
