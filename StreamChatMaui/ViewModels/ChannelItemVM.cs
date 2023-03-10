using StreamChat.Core.StatefulModels;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// ViewModel for a single item of <see cref="IStreamChannel"/> in <see cref="MainPageVM"/>
/// </summary>
public class ChannelItemVM : BaseViewModel
{
    //Todo: move to config
    public const int TitleMaxCharCount = 30;
    public const int DetailMaxCharCount = 30;

    public string Id => _channel.Id;
    public string Type => _channel.Type;

    public string Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    public string Detail
    {
        get => _detail;
        private set => SetProperty(ref _detail, value);
    }

    public ChannelItemVM(IStreamChannel channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));

        Title = _channel.GenerateChannelTitle(TitleMaxCharCount);
        Detail = GenerateDetail();

        _channel.MessageReceived += OnMessageReceived;
        _channel.MessageUpdated += OnMessageUpdated;
        _channel.MessageDeleted += OnMessageDeleted;
    }

    public void Dispose()
    {
        _channel.MessageReceived -= OnMessageReceived;
        _channel.MessageUpdated -= OnMessageUpdated;
        _channel.MessageDeleted -= OnMessageDeleted;
    }

    private readonly IStreamChannel _channel;

    private string _title;
    private string _detail;

    /// <summary>
    /// Generate the preview snippet for the channel
    /// </summary>
    private string GenerateDetail()
    {
        var lastMessage = _channel.Messages.LastOrDefault();
        if (lastMessage == null)
        {
            return string.Empty;
        }

        if (lastMessage.Text.Length <= DetailMaxCharCount)
        {
            return lastMessage.Text;
        }

        //Todo: Handle case if message contains emoji -> we don't want to cut string in the middle of emoji key but before it
        return lastMessage.Text.Substring(0, DetailMaxCharCount);
    }

    private void OnMessageDeleted(IStreamChannel channel, IStreamMessage message, bool isHardDelete)
        => Detail = GenerateDetail();

    private void OnMessageUpdated(IStreamChannel channel, IStreamMessage message) 
        => Detail = GenerateDetail();

    private void OnMessageReceived(IStreamChannel channel, IStreamMessage message) 
        => Detail = GenerateDetail();
}