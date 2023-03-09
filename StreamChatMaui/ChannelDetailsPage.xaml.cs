using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

[QueryProperty(nameof(ChannelId), QueryParams.ChannelId)]
[QueryProperty(nameof(ChannelType), QueryParams.ChannelType)]
public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelDetailsVM vm)
    {
        _vm = vm;
        InitializeComponent();

        MessagesList.ItemsSource = _vm.Messages;
    }

    public string ChannelId
    {
        set => _vm.ChannelId = value;
    }
    
    public string ChannelType
    {
        set => _vm.ChannelType = StreamChat.Core.ChannelType.Custom(value);
    }

    private readonly ChannelDetailsVM _vm;
}