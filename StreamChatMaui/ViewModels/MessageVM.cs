using Microsoft.Extensions.Logging;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

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

    public bool IsLocalUserMessage
    {
        get => _isLocalUserMessage;
        set => SetProperty(ref _isLocalUserMessage, value);
    }

    public IStreamMessage Message { get; }

    public MessageVM(IStreamMessage message, IStreamChatService chatService, ILogger<MessageVM> logger)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));

        Refresh();
        UpdateIsLocalUserFlagAsync().LogIfFailed(logger);
    }

    public void Refresh()
    {
        Text = Message.Text;
        Author = Message.User.Id;
    }

    private bool _showAuthor = true;
    private bool _isLocalUserMessage;
    private string _text;
    private string _author;
    private readonly IStreamChatService _chatService;

    private async Task UpdateIsLocalUserFlagAsync()
    {
        var client = await _chatService.GetClientWhenReadyAsync();
        IsLocalUserMessage = client.LocalUserData.User.Id == Message.User.Id;
    }
}