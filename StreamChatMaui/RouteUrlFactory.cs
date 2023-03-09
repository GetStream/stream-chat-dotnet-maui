using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

/// <summary>
/// Factory for creating route URLs
/// </summary>
public class RouteUrlFactory
{
    public string CreateChannelDetailsPageRoute(ChannelItemVM channel)
        => $"{nameof(ChannelDetailsPage)}?{QueryParams.ChannelId}={channel.Id}&{QueryParams.ChannelType}={channel.Type}";
}