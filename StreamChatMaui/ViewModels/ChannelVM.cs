using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using StreamChat.Core;
using StreamChat.Core.StatefulModels;
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

    public ReadOnlyObservableCollection<MessageVM> Messages { get; }

    public ChannelVM(IStreamChatService chatService, IViewModelFactory viewModelFactory, ILogger<ChannelVM> logger)
    {
        _chatService = chatService;
        _viewModelFactory = viewModelFactory;
        _logger = logger;

        Messages = new ReadOnlyObservableCollection<MessageVM>(_messages);
        _messages.CollectionChanged += OnMessagesCollectionChanged;

        SendMessageCommand = new AsyncRelayCommand(ExecuteSendMessageCommand, CanSendMessageCommand);
    }

    public void Dispose() => UnsubscribeFromEvents();

    private readonly ObservableCollection<MessageVM> _messages = new();
    private readonly IStreamChatService _chatService;
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
        catch(Exception e)
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
            AddMessage(m);
        }
    }

    private void AddMessage(IStreamMessage message)
    {
        var vm = _viewModelFactory.CreateMessageVM(message);

        var previousMessage = _messages.LastOrDefault();
        if(previousMessage != null && previousMessage.Message.User == message.User)
        {
            previousMessage.ShowAuthor = false;
        }

        _messages.Add(vm);
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
    }

    private void UnsubscribeFromEvents()
    {
        _messages.CollectionChanged -= OnMessagesCollectionChanged;

        if (_channel != null)
        {
            _channel.MessageReceived -= OnMessageReceived;
            _channel.MessageUpdated -= OnMessageUpdated;
            _channel.MessageDeleted -= OnMessageDeleted;
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
        var msg = _messages.FirstOrDefault(m => m.Message == message);
        if (msg != null)
        {
            _messages.Remove(msg);
        }
    }

    private void OnMessageUpdated(IStreamChannel channel, IStreamMessage message)
    {
        var msg = _messages.FirstOrDefault(m => m.Message == message);
        msg?.Refresh();
    }

    private void OnMessageReceived(IStreamChannel channel, IStreamMessage message) => AddMessage(message);

    private void OnMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ShowEmptyView = _messages.Count == 0;
        Console.WriteLine($"IsEmpty: {ShowEmptyView}");
    }
}