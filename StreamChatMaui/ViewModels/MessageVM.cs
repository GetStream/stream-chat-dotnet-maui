using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// View model for a <see cref="IStreamMessage"/>
/// </summary>
public class MessageVM : BaseViewModel
{
    //Todo: change to observable
    public string Text { get; private set; }
    public string Author { get; private set; }

    public IStreamMessage Message { get; }

    public MessageVM(IStreamMessage message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));

        Refresh();
    }

    public void Refresh()
    {
        Text = Message.Text;
        Author = Message.User.Id;
    }
}