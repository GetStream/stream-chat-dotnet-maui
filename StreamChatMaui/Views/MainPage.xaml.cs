using Microsoft.Extensions.Logging;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageVM vm, ILogger<MainPage> logger)
    {
        BindingContext =_vm = vm;
        _logger = logger;

        InitializeComponent();
    }

    private readonly MainPageVM _vm;
    private readonly ILogger _logger;

    private async void ChannelsList_ItemTapped(object sender, ItemTappedEventArgs args)
    {
        if (args.Item == null)
        {
            return;
        }

        try
        {
            await _vm.OpenChannelCommand.ExecuteAsync(args.Item);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}