using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelVM vm)
    {
        BindingContext = vm;

        InitializeComponent();
    }
}