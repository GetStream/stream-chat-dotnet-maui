﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using StreamChat.Core;
using StreamChat.Core.Exceptions;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.Commands;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// ViewModel for a page showing <see cref="IStreamChannel"/> details: messages, members, etc.
/// </summary>
[QueryProperty(nameof(ChannelId), QueryParams.ChannelId)]
[QueryProperty(nameof(ChannelType), QueryParams.ChannelType)]
public partial class ChannelVM : BaseViewModel, IDisposable
{
    //Todo: move to config
    public const int TitleMaxCharCount = 30;

    public event Action<MessageVM> MessageContextMenuRequested;

    public string MessageInput
    {
        get => _messageInput;
        set
        {
            SetProperty(ref _messageInput, value);
            SendMessageCommand.NotifyCanExecuteChanged();
        }
    }

    public string ChannelId
    {
        set
        {
            _inputChannelId = value;
            LoadContentsAsync().LogIfFailed(_logger);
        }
    }

    public string ChannelType
    {
        set
        {
            _inputChannelType = StreamChat.Core.ChannelType.Custom(value);
            LoadContentsAsync().LogIfFailed(_logger);
        }
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsEntryEnabled
    {
        get => _isEntryEnabled;
        set => SetProperty(ref _isEntryEnabled, value);
    }

    public bool ShowEmptyView
    {
        get => _showEmptyView;
        set => SetProperty(ref _showEmptyView, value);
    }

    public IAsyncRelayCommand SendMessageCommand { get; private set; }
    public IAsyncRelayCommand<AddOrRemoveReactionCommandArgs> AddOrRemoveMessageReactionCommand { get; private set; }
    public IAsyncRelayCommand<MessageVM> DeleteMessageCommand { get; private set; }
    public IAsyncRelayCommand<MessageVM> TapMessageCommand { get; private set; }
    public IAsyncRelayCommand LoadOlderMessagesCommand { get; private set; }

    public ReadOnlyObservableCollection<MessageVM> Messages { get; }

    public ChannelVM(IStreamChatService chatService, IChatPermissionsService chatPermissionsService, IViewModelFactory viewModelFactory, ILogger<ChannelVM> logger)
    {
        _chatService = chatService;
        _chatPermissions = chatPermissionsService;
        _viewModelFactory = viewModelFactory;
        _logger = logger;

        Messages = new ReadOnlyObservableCollection<MessageVM>(_messages);
        _messages.CollectionChanged += OnMessagesCollectionChanged;

        SendMessageCommand = new AsyncRelayCommand(ExecuteSendMessageCommand, CanSendMessageCommand);
        AddOrRemoveMessageReactionCommand = new AsyncRelayCommand<AddOrRemoveReactionCommandArgs>(ExecuteAddOrRemoveMessageReactionCommand);
        DeleteMessageCommand = new AsyncRelayCommand<MessageVM>(ExecuteDeleteMessageCommand);
        TapMessageCommand = new AsyncRelayCommand<MessageVM>(ExecuteTapMessageCommand);
        LoadOlderMessagesCommand = new AsyncRelayCommand(ExecuteLoadOlderMessagesCommand, CanExecuteLoadOlderMessagesCommand);
    }

    public void Dispose() => UnsubscribeFromEvents();

    private readonly ObservableCollection<MessageVM> _messages = new();
    private readonly IStreamChatService _chatService;
    private readonly IChatPermissionsService _chatPermissions;
    private readonly IViewModelFactory _viewModelFactory;
    private readonly ILogger<ChannelVM> _logger;

    private string _inputChannelId;
    private ChannelType? _inputChannelType;

    private IStreamChannel _channel;

    private string _title = string.Empty;
    private string _messageInput = string.Empty;
    private bool _isEntryEnabled = true;
    private bool _showEmptyView = true;
    private bool _isSending;
    private bool _isLoadingOlderMessages;

    private async Task ExecuteLoadOlderMessagesCommand()
    {
        _logger.LogInformation("Load older messages");

        try
        {
            var firstMessage = _channel.Messages.FirstOrDefault();
            _isLoadingOlderMessages = true;
            var countBefore = _channel.Messages.Count();
            await _channel.LoadOlderMessagesAsync();
            var countAfter = _channel.Messages.Count();

            _logger.LogInformation($"Loaded {countAfter - countBefore} older messages");

            int previousFirstMessageIndex = -1;
            for (int i = 0; i < _channel.Messages.Count; i++)
            {
                if (_channel.Messages[i] == firstMessage)
                {
                    previousFirstMessageIndex = i;
                    break;
                }
            }

            for (int i = previousFirstMessageIndex - 1; i >= 0; i--)
            {
                PrependMessage(_channel.Messages[i]);
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        finally
        {
            _isLoadingOlderMessages = false;
        }
    }

    private bool CanExecuteLoadOlderMessagesCommand()
    {
        //Todo: check if already loading or if reached timeout
        return !_isLoadingOlderMessages;
    }

    private async Task ExecuteAddOrRemoveMessageReactionCommand(AddOrRemoveReactionCommandArgs args)
    {
        var mesage = args.Message;
        var hasReaction = mesage.OwnReactions.Any(r => r.Type == args.Reaction);

        if (hasReaction)
        {
            await mesage.DeleteReactionAsync(args.Reaction);
        }
        else
        {
            await mesage.SendReactionAsync(args.Reaction);
        }
    }

    private async Task ExecuteDeleteMessageCommand(MessageVM messageVm)
    {
        var client = await _chatService.GetClientWhenReadyAsync();
        var canDelete = _chatPermissions.CanDelete(messageVm.Message);
        if (!canDelete)
        {
            throw new InvalidOperationException($"User `{client.LocalUserData.UserId}` is not allowed to delete message {messageVm.Message.Id} from Channel with Cid: {messageVm.Message.Cid}");
        }

        var snippet = messageVm.Text.TakeSnippet(40);
        var confirmed = await Application.Current.MainPage.DisplayAlert("Delete message", $"Are you sure you want to delete message `{snippet}`? This action cannot be undone.", "Yes", "No");
        if (!confirmed)
        {
            return;
        }

        try
        {
            await messageVm.Message.SoftDeleteAsync();
        }
        catch (StreamApiException streamApiException)
        {
            Console.WriteLine(streamApiException.Message);
            throw;
        }
    }

    private Task ExecuteTapMessageCommand(MessageVM messageVm)
    {
        MessageContextMenuRequested?.Invoke(messageVm);

        return Task.CompletedTask;
    }

    private async Task ExecuteSendMessageCommand()
    {
        if (_isSending)
        {
            return;
        }

        _isSending = true;

        try
        {
            await _channel.SendNewMessageAsync(MessageInput);
            MessageInput = string.Empty;

            HideMobileKeyboard();
        }
        finally
        {
            _isSending = false;
        }
    }

    private bool CanSendMessageCommand() => MessageInput?.Length > 0;

    private async Task LoadContentsAsync()
    {
        if (string.IsNullOrEmpty(_inputChannelId) || !_inputChannelType.HasValue || IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;

            _logger.LogInformation($"Load channel with id: {_inputChannelId}, type: {_inputChannelType}");

            var client = await _chatService.GetClientWhenReadyAsync();
            var channel = await client.GetOrCreateChannelWithIdAsync(_inputChannelType.Value, _inputChannelId);
            SetChannel(channel);

            LoadMessages();

            Title = _channel.GenerateChannelTitle(TitleMaxCharCount);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LoadMessages()
    {
        foreach (var m in _channel.Messages)
        {
            AppendMessage(m);
        }
    }

    private void AppendMessage(IStreamMessage message)
    {
        var vm = _viewModelFactory.CreateMessageVM(message);

        var previousMessage = _messages.LastOrDefault();
        if (previousMessage != null && previousMessage.Message.User == message.User)
        {
            previousMessage.ShowAuthor = false;
        }

        _messages.Add(vm);
    }

    private void PrependMessage(IStreamMessage message)
    {
        var vm = _viewModelFactory.CreateMessageVM(message);

        var nextMessage = _messages.FirstOrDefault();
        if (nextMessage != null && nextMessage.Message.User == message.User)
        {
            vm.ShowAuthor = false;
        }

        _messages.Insert(0, vm);
    }

    private void SetChannel(IStreamChannel channel)
    {
        if (_channel != null)
        {
            UnsubscribeFromEvents();
        }

        _channel = channel;

        if (_channel == null)
        {
            return;
        }

        _channel.MessageReceived += OnMessageReceived;
        _channel.MessageUpdated += OnMessageUpdated;
        _channel.MessageDeleted += OnMessageDeleted;
        _channel.ReactionAdded += OnReactionChanged;
        _channel.ReactionRemoved += OnReactionChanged;
        _channel.ReactionUpdated += OnReactionChanged;
    }

    private void UnsubscribeFromEvents()
    {
        _messages.CollectionChanged -= OnMessagesCollectionChanged;

        if (_channel != null)
        {
            _channel.MessageReceived -= OnMessageReceived;
            _channel.MessageUpdated -= OnMessageUpdated;
            _channel.MessageDeleted -= OnMessageDeleted;
            _channel.ReactionAdded -= OnReactionChanged;
            _channel.ReactionRemoved -= OnReactionChanged;
            _channel.ReactionUpdated -= OnReactionChanged;
        }
    }

    /// <summary>
    /// Workaround to hide keyboard on mobile
    /// </summary>
    private void HideMobileKeyboard()
    {
        IsEntryEnabled = false;
        IsEntryEnabled = true;
    }

    private void OnMessageDeleted(IStreamChannel channel, IStreamMessage message, bool isHardDelete)
    {
        var msgVm = _messages.FirstOrDefault(m => m.Message == message);
        if (msgVm == null)
        {
            return;
        }

        if (isHardDelete)
        {
            _messages.Remove(msgVm);
        }
        else
        {
            msgVm.UpdateData();
        }
    }

    private void OnMessageUpdated(IStreamChannel channel, IStreamMessage message)
    {
        var msg = _messages.FirstOrDefault(m => m.Message == message);
        msg?.UpdateData();
    }

    private void OnMessageReceived(IStreamChannel channel, IStreamMessage message) => AppendMessage(message);

    private void OnMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        => ShowEmptyView = _messages.Count == 0;

    private void OnReactionChanged(IStreamChannel channel, IStreamMessage message, StreamChat.Core.Models.StreamReaction reaction)
    {
        var msgVm = _messages.FirstOrDefault(m => m.Message == message);
        msgVm?.UpdateData();
    }
}