using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageVM vm, RouteUrlFactory routeUrlFactory)
    {
        _vm = vm;
        _routeUrlFactory = routeUrlFactory;
        InitializeComponent();

        //Todo: handle via Binding
        ChannelsList.ItemsSource = _vm.Channels;
    }

    private readonly MainPageVM _vm;
    private readonly RouteUrlFactory _routeUrlFactory;

    //Todo: refactor to command
    private async void ChannelsList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
        {
            return;
        }

        var selectedChannel = (ChannelItemVM)e.Item;

        await Shell.Current.GoToAsync(_routeUrlFactory.CreateChannelDetailsPageRoute(selectedChannel));
    }
}