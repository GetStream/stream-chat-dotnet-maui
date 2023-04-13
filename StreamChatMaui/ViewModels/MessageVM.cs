using Microsoft.Extensions.Logging;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;
using System.Collections.ObjectModel;

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

    public bool HasAnyReactions
    {
        get => _hasAnyReactions;
        set => SetProperty(ref _hasAnyReactions, value);
    }

    public IStreamMessage Message { get; }

    public ReadOnlyObservableCollection<ReactionVM> Reactions { get; }

    public MessageVM(IStreamMessage message, IStreamChatService chatService, ReactionsRepository reactionsRepository, ILogger<MessageVM> logger)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        _reactionsRepository = reactionsRepository;
        _logger = logger;
        Reactions = new ReadOnlyObservableCollection<ReactionVM>(_reactions);

        Refresh();
        UpdateIsLocalUserFlagAsync().LogIfFailed(logger);
    }

    public void Refresh()
    {
        Text = Message.IsDeleted ? "This message was deleted." : Message.Text;
        Author = Message.User.Id;

        _reactions.Clear();

        foreach(var reaction in Message.ReactionScores)
        {
            if (!_reactionsRepository.TryGetValue(reaction.Key, out var unicode)) 
            {
                _logger.LogError($"Failed to find reaction unicode symbol for {reaction.Key}");
                continue;
            }
            _reactions.Add(new ReactionVM(reaction.Key, unicode, reaction.Value));
        }

        HasAnyReactions = _reactions.Count > 0;
    }

    private readonly IStreamChatService _chatService;
    private readonly ReactionsRepository _reactionsRepository;
    private readonly ILogger<MessageVM> _logger;
    private readonly ObservableCollection<ReactionVM> _reactions = new();

    private bool _showAuthor = true;
    private bool _isLocalUserMessage;
    private bool _hasAnyReactions;
    private string _text;
    private string _author;

    private async Task UpdateIsLocalUserFlagAsync()
    {
        var client = await _chatService.GetClientWhenReadyAsync();
        IsLocalUserMessage = client.LocalUserData.User.Id == Message.User.Id;
    }
}