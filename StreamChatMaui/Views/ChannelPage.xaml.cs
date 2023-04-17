using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Commands;
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
        _vm.MessageContextMenuRequested += _vm_MessageContextMenuRequested;

        InitializeComponent();
        BindingContext = _vm;

        if (!_chatPermissions.IsReady)
        {
            logger.LogError("Permissions service is not ready to use");
        }

        MessagesList.ChildAdded += MessagesList_ChildAdded;

    }

    private void _vm_MessageContextMenuRequested(MessageVM obj)
    {
        var popup = new MessageContextPopupView(_reactionsRepository);
        this.ShowPopup(popup);
    }

    private void MessagesList_ChildAdded(object sender, ElementEventArgs e)
    {
        var ele = e.Element as VerticalStackLayout;
        var b = ele.BindingContext;

    }

    private readonly ChannelVM _vm;
    private readonly IChatPermissionsService _chatPermissions;
    private readonly ReactionsRepository _reactionsRepository;

    /// <summary>
    /// We delay ContextActions generation until here because <see cref="MessageView_ChildAdded"/> has not bounded data yet
    /// </summary>
    private void ViewCell_Appearing(object sender, EventArgs eventArgs)
    {
        var viewCell = sender as ViewCell;
        viewCell.Appearing -= ViewCell_Appearing;

        var contextActions = viewCell.ContextActions;
        var messageVm = viewCell.BindingContext as MessageVM;

        if (messageVm.Message.IsDeleted)
        {
            return;
        }

        if (_chatPermissions.CanEdit(messageVm.Message))
        {
            contextActions.Add(new MenuItem
            {
                Text = "Edit",
            });
        }

        if (_chatPermissions.CanDelete(messageVm.Message))
        {
            contextActions.Add(new MenuItem
            {
                Text = "Delete",
                Command = _vm.DeleteMessageCommand,
                CommandParameter = messageVm
            });
        }

        //foreach (var (key, value) in _reactionsRepository.Reactions)
        //{
        //    contextActions.Add(new MenuItem
        //    {
        //        Text = value,
        //        Command = _vm.AddOrRemoveMessageReactionCommand,
        //        CommandParameter = new AddOrRemoveReactionCommandArgs(messageVm.Message, key)
        //    });
        //}
    }
}