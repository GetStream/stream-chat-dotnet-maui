using CommunityToolkit.Mvvm.Input;
using StreamChatMaui.Commands;

namespace StreamChatMaui.ViewModels
{
    public class ReactionButtonVM : BaseViewModel
    {
        public string Text { get; set; }
        public IAsyncRelayCommand<AddOrRemoveReactionCommandArgs> Command { get; set; }
        public AddOrRemoveReactionCommandArgs CommandParameter { get; set; }
    }
}
