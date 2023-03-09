using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels
{
    /// <summary>
    /// View model for a list of channels
    /// </summary>
    public class ChannelsListVM
    {
        public ReadOnlyObservableCollection<ChannelVM> Channels { get; }

        public ChannelsListVM(IStreamChatService chatService, ILogger<ChannelsListVM> logger)
        {
            _chatService = chatService;
            Channels = new ReadOnlyObservableCollection<ChannelVM>(_channels);

            LoadChannelsAsync().LogIfFailed(logger);
        }

        private readonly ObservableCollection<ChannelVM> _channels = new();

        private readonly IStreamChatService _chatService;

        private async Task LoadChannelsAsync()
        {
            var client = await _chatService.GetClientWhenReady();

            var channels = await client.QueryChannelsAsync();

            //Todo: move to factory service
            foreach(var c in channels)
            {
                var channelVm = new ChannelVM(c);
                _channels.Add(channelVm);
            }
        }
    }
}