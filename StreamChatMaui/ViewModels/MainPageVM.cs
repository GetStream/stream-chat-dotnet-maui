using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using StreamChat.Core;
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
                await CreateSampleChannelsAsync(client);
            }

            //Todo: move to factory service
            foreach(var c in channels)
            {
                var channelVm = new ChannelItemVM(c);
                _channels.Add(channelVm);
            }
        }

        private async Task CreateSampleChannelsAsync(IStreamChatClient client)
        {
            await client.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, "Main");
            await client.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, "The Amazing Channel");
            await client.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, "Private Group");
        }
    }
}