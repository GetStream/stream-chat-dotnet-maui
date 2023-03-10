using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using StreamChat.Core;
using StreamChat.Core.StatefulModels;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels
{
    /// <summary>
    /// View model for the Main page with the list of channels
    /// </summary>
    public class MainPageVM : BaseViewModel
    {
        public ReadOnlyObservableCollection<ChannelItemVM> Channels { get; }

        public MainPageVM(IStreamChatService chatService, ILogger<MainPageVM> logger)
        {
            _chatService = chatService;
            Channels = new ReadOnlyObservableCollection<ChannelItemVM>(_channels);

            LoadChannelsAsync().LogIfFailed(logger);
        }

        private readonly ObservableCollection<ChannelItemVM> _channels = new();

        private readonly IStreamChatService _chatService;

        private async Task LoadChannelsAsync()
        {
            var client = await _chatService.GetClientWhenReady();

            var channels = await client.QueryChannelsAsync();

            if (!channels.Any())
            {
                channels = await CreateSampleChannelsAsync(client);
            }

            //Todo: move to factory service
            foreach (var c in channels)
            {
                var channelVm = new ChannelItemVM(c);
                _channels.Add(channelVm);
            }
        }

        /// <summary>
        /// Here we create sample channels of type <see cref="ChannelType.Livestream"/>. 
        /// This channel type allows regular users (with role=user) to create new channels.
        /// 
        /// If you're building a regular chat app you should use the <see cref="ChannelType.Messaging"/> 
        /// where regular users are not allowed to create new chanels (unless explicitly allowed through our permissions & roles system)
        /// 
        /// You can read more about channel types here https://getstream.io/chat/docs/unity/channel_features/?language=unity
        /// </summary>
        private async Task<IEnumerable<IStreamChannel>> CreateSampleChannelsAsync(IStreamChatClient client)
        {
            var c1 = await client.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "main", "Main");
            var c2 = await client.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "baseball", "Baseball Club");
            var c3 = await client.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "american_football",
                "American Football");
            var c4 = await client.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "basketball", "Basketball");
            var c5 = await client.GetOrCreateChannelWithIdAsync(ChannelType.Livestream, "ice_hockey", "Ice Hockey");

            return new[] { c1, c2, c3, c4, c5 };
        }
    }
}