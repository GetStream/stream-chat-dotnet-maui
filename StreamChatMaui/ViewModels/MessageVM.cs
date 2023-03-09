using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.ViewModels;

public class MessageVM : BaseViewModel
{
    //Todo: change to observable
    public string Text { get; private set; }
    public string Author { get; private set; }

    public MessageVM(IStreamMessage message)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));

        Update();
    }

    public void Update()
    {
        Text = _message.Text;
        Author = _message.User.Id;
    }

    private readonly IStreamMessage _message;
}