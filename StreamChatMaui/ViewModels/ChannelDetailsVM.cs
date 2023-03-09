using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using StreamChat.Core;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// ViewModel for a page showing <see cref="IStreamChannel"/> details: messages, members, etc.
/// </summary>
public class ChannelDetailsVM : BaseViewModel
{
    public ChannelDetailsVM(IStreamChatService chatService, ILogger<ChannelDetailsVM> logger)
    {
        _chatService = chatService;
        _logger = logger;

        Messages = new ReadOnlyObservableCollection<MessageVM>(_messages);
    }

    public string ChannelId
    {
        set
        {
            _inputChannelId = value;
            LoadMessagesAsync().LogIfFailed(_logger);
        }
    }
    
    public ChannelType ChannelType
    {
        set
        {
            _inputChannelType = value;
            LoadMessagesAsync().LogIfFailed(_logger);
        }
    }
    
    public ReadOnlyObservableCollection<MessageVM> Messages { get; }
    
    private readonly ObservableCollection<MessageVM> _messages = new();
    private readonly IStreamChatService _chatService;
    private readonly ILogger<ChannelDetailsVM> _logger;

    private string _inputChannelId;
    private ChannelType? _inputChannelType;

    private IStreamChannel _channel;

    private async Task LoadMessagesAsync()
    {
        if (string.IsNullOrEmpty(_inputChannelId) || !_inputChannelType.HasValue)
        {
            return;
        }
        
        _logger.LogInformation($"Load channel with id: {_inputChannelId}, type: {_inputChannelType}");
        
        var client = await _chatService.GetClientWhenReady();
        _channel = await client.GetOrCreateChannelWithIdAsync(_inputChannelType.Value, _inputChannelId);

        foreach (var m in _channel.Messages)
        {
            //Todo: move to factory
            var messageVm = new MessageVM(m);
            _messages.Add(messageVm);
        }
    }

}