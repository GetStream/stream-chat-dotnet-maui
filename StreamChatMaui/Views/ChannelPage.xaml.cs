using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelVM vm, IChatPermissionsService chatPermissionsService, ReactionsRepository reactionsRepository, ILogger<ChannelDetailsPage> logger)
    {
        InitializeComponent();

        _chatPermissions = chatPermissionsService;
        _reactionsRepository = reactionsRepository;
        _logger = logger;
        _vm = vm;
        _vm.MessageContextMenuRequested += OnMessageContextMenuRequested;
        MessagesList.Scrolled += OnMessagesListScrolled;

        BindingContext = _vm;

        if (!_chatPermissions.IsReady)
        {
            logger.LogError("Permissions service is not ready to use");
        }
    }

    protected override void OnDisappearing()
    {
        MessagesList.Scrolled -= OnMessagesListScrolled;
        _vm.MessageContextMenuRequested -= OnMessageContextMenuRequested;

        base.OnDisappearing();
    }

    private readonly ChannelVM _vm;
    private readonly IChatPermissionsService _chatPermissions;
    private readonly ReactionsRepository _reactionsRepository;
    private readonly ILogger<ChannelDetailsPage> _logger;

    private void OnMessagesListScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (ShouldLoadOlderMessages(e) && _vm.LoadOlderMessagesCommand.CanExecute(default))
        {
            _vm.LoadOlderMessagesCommand.Execute(default);
        }
    }

    /// <summary>
    /// If scrolling upwards and reached the minimum messages threshold -> we should try to load older 
    /// </summary>
    private bool ShouldLoadOlderMessages(ItemsViewScrolledEventArgs e)
    {
        const int messagesThreshold = 5;

        var isScrollingUpwards = e.VerticalDelta < 0;
        if (!isScrollingUpwards)
        {
            return false;
        }

        return e.FirstVisibleItemIndex <= messagesThreshold;
    }

    private void OnMessageContextMenuRequested(MessageVM messageVm)
    {
        var popup = new MessageContextPopupView(_reactionsRepository, _chatPermissions, messageVm, _vm);
        this.ShowPopup(popup);
    }
}