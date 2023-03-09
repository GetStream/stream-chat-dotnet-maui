using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

public partial class MainPage : ContentPage
{
    public MainPage(ChannelsListVM vm, RouteUrlFactory routeUrlFactory)
    {
        _vm = vm;
        _routeUrlFactory = routeUrlFactory;
        InitializeComponent();

        //Todo: handle via Binding
        ChannelsList.ItemsSource = _vm.Channels;
    }

    private readonly ChannelsListVM _vm;
    private readonly RouteUrlFactory _routeUrlFactory;

    private async void ChannelsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(e.SelectedItem == null)
        {
            return;
        }

        var selectedChannel = (ChannelItemVM)e.SelectedItem;

        await Shell.Current.GoToAsync(_routeUrlFactory.CreateChannelDetailsPageRoute(selectedChannel));
    }
}