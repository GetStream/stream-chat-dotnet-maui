using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelVM vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}