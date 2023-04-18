using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using StreamChatMaui.Commands;
using StreamChatMaui.Models;
using StreamChatMaui.Services;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class MessageContextPopupView : Popup
{
    public IRelayCommand DeleteMessageCommand { get; private set; }
    public IRelayCommand<string> AddOrRemoveReactionCommand { get; private set; }

    public IEnumerable<ReactionOption> ReactionOptions => _reactionsRepository.Reactions;

    public MessageContextPopupView(ReactionsRepository reactionsRepository, MessageVM messageVM, ChannelVM channelItemVM)
    {
        InitializeComponent();

        _reactionsRepository = reactionsRepository;
        _messageVM = messageVM;
        _channelVM = channelItemVM;

        DeleteMessageCommand = new RelayCommand(ExecuteDeleteMessageCommand);
        AddOrRemoveReactionCommand = new RelayCommand<string>(ExecuteAddOrRemoveReactionCommand);

        BindingContext = this;
    }

    private readonly ReactionsRepository _reactionsRepository;

    private readonly MessageVM _messageVM;
    private readonly ChannelVM _channelVM;


    private void ExecuteDeleteMessageCommand()
    {
        _channelVM.DeleteMessageCommand.Execute(_messageVM);
        Close();
    }

    private void ExecuteAddOrRemoveReactionCommand(string reactionKey)
    {
        _channelVM.AddOrRemoveMessageReactionCommand.Execute(new AddOrRemoveReactionCommandArgs(_messageVM.Message, reactionKey));
        Close();
    }
}