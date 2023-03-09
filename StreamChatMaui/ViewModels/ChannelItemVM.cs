using System.Text;
using StreamChat.Core.StatefulModels;

namespace StreamChatMaui.ViewModels;

/// <summary>
/// ViewModel for a single <see cref="IStreamChannel"/>
/// </summary>
public class ChannelItemVM : BaseViewModel
{
    //Todo: move to config
    public const int TitleMaxCharCount = 30;
    public const int DetailMaxCharCount = 30;

    public string Id => _channel.Id;
    public string Type => _channel.Type;
    
    //Todo: change to observable properties
    public string Title { get; private set; }
    public string Detail{ get; private set; }

    public ChannelItemVM(IStreamChannel channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));

        Title = GenerateTitle();
        Detail = GenerateDetail();

        //Todo: subscribe to message received so we update the Details snippet
    }

    private readonly IStreamChannel _channel;

    /// <summary>
    /// Generate title in the following order:
    /// - If channel has a name -> display Name
    /// - If channel has an ID -> display ID
    /// - If channel is just a group chat -> display participants names
    /// </summary>
    private string GenerateTitle()
    {
        if (!string.IsNullOrEmpty(_channel.Name))
        {
            return _channel.Name;
        }

        if (!string.IsNullOrEmpty(_channel.Id))
        {
            return _channel.Id;
        }

        // if no Name and Id it means it's a channel for a unique group of users

        var sb = new StringBuilder();

        for (var i = 0; i < _channel.Members.Count; i++)
        {
            var member = _channel.Members[i];

            sb.Append(member.User.Name);

            if (sb.Length > TitleMaxCharCount)
            {
                break;
            }

            if (i < _channel.Members.Count)
            {
                sb.Append(", ");
            }
        }

        if (sb.Length > TitleMaxCharCount)
        {
            sb.Length = TitleMaxCharCount;
            sb.Append("...");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Generate the preview snippet for a channel
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
}