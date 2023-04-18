using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelVM vm, IChatPermissionsService chatPermissionsService, ReactionsRepository reactionsRepository, ILogger<ChannelDetailsPage> logger)
    {
        _chatPermissions = chatPermissionsService;
        _reactionsRepository = reactionsRepository;
        _vm = vm;
        _vm.MessageContextMenuRequested += MessageContextMenuRequested;

        InitializeComponent();
        BindingContext = _vm;

        if (!_chatPermissions.IsReady)
        {
            logger.LogError("Permissions service is not ready to use");
        }
    }

    protected override void OnDisappearing()
    {
        _vm.MessageContextMenuRequested -= MessageContextMenuRequested;

        base.OnDisappearing();
    }

    private void MessageContextMenuRequested(MessageVM messageVm)
    {
        var popup = new MessageContextPopupView(_reactionsRepository, messageVm, _vm);
        this.ShowPopup(popup);
    }

    private readonly ChannelVM _vm;
    private readonly IChatPermissionsService _chatPermissions;
    private readonly ReactionsRepository _reactionsRepository;
}