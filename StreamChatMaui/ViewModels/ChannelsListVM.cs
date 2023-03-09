using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.Utils;

namespace StreamChatMaui.ViewModels
{
    /// <summary>
    /// View model for a list of channels
    /// </summary>
    public class ChannelsListVM : BaseViewModel
    {
        public ReadOnlyObservableCollection<ChannelItemVM> Channels { get; }

        public ChannelsListVM(IStreamChatService chatService, ILogger<ChannelsListVM> logger)
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

            //Todo: move to factory service
            foreach(var c in channels)
            {
                var channelVm = new ChannelItemVM(c);
                _channels.Add(channelVm);
            }
        }
    }
}