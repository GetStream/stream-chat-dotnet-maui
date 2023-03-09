using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

public partial class MainPage : ContentPage
{
    public MainPage(ChannelsListVM vm)
    {
        _vm = vm;
        InitializeComponent();

        ChannelsList.ItemsSource = _vm.Channels;
    }

    private readonly ChannelsListVM _vm;
}