using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// View model for a <see cref="IStreamMessage"/>
/// </summary>
public class MessageVM : BaseViewModel
{
    public string Text
    {
        get => _text;
        private set => SetProperty(ref _text, value);
    }

    public string Author
    {
        get => _author;
        private set => SetProperty(ref _author, value);
    }
    public bool ShowAuthor
    {
        get => _showAuthor;
        set => SetProperty(ref _showAuthor, value);
    }

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

    private bool _showAuthor = true;
    private string _text;
    private string _author;

}