using Microsoft.Extensions.Logging;
using StreamChat.Core;
using StreamChat.Core.StatefulModels;
using StreamChat.Libs.Auth;
using StreamChat.Libs.Utils;
using StreamChatMaui.Utils;

namespace StreamChatMaui.Services
{
    public class StreamChatService : IStreamChatService, IDisposable
    {
        public StreamChatService(ILogger<StreamChatService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var credentials = StreamChatService.GetAuthCredentials();

            _client = StreamChatClient.CreateDefaultClient();
            _client.Connected += OnConnected;
            _client.ConnectUserAsync(credentials).LogIfFailed();

            _logger.LogInformation("Start Service");
        }

        public async Task<IStreamChatClient> GetClientWhenReadyAsync()
        {
            if (_client.IsConnected)
            {
                return _client;
            }

            var cts = new TaskCompletionSource<IStreamChatClient>();

            void OnClientConnected(IStreamLocalUserData localUserData)
            {
                cts.SetResult(_client);
                _client.Connected -= OnClientConnected;
            }

            _client.Connected += OnClientConnected;

            await cts.Task;
            return _client;
        }
        
        public void Dispose() => DisposeAsync().LogIfFailed(_logger);

        private readonly IStreamChatClient _client;

        private readonly CancellationTokenSource _cts = new();
        private readonly ILogger<StreamChatService> _logger;

        private void OnConnected(IStreamLocalUserData localUserData)
        {
            _logger.LogInformation($"User connected: {localUserData.UserId}");
        }

        /// <summary>
        /// Dispose the client in order to signal the server that the user is becoming offline
        /// </summary>
        private async Task DisposeAsync()
        {
            _logger.LogInformation("Disposing Service");
            _client.Connected -= OnConnected;

            if (_client.IsConnected)
            {
                await _client.DisconnectUserAsync();
            }

            _cts.Cancel();
            _client.Dispose();
        }

        private static AuthCredentials GetAuthCredentials()
        {
            var apiKey = StaticConfig.StreamApiKey;
            var forcedUserId = StaticConfig.ForcedUserId;
            var forcedUserToken = StaticConfig.ForcedUserToken;

            if (!forcedUserId.IsNullOrEmpty() && !forcedUserToken.IsNullOrEmpty())
            {
                return new AuthCredentials(apiKey, forcedUserId, forcedUserToken);
            }

            //Todo: get userId & token from an authorization endpoint
            var userName = StreamChatUtils.GenerateRandomName();
            var userId = StreamChatClient.SanitizeUserId(userName);

            // Developer tokens need to be enabled (check docs below). This is fine for testing but in production user tokens should be provided by a customer endpoint
            // https://getstream.io/chat/docs/unity/tokens_and_authentication/?language=unity#developer-tokens
            var userToken = StreamChatClient.CreateDeveloperAuthToken(userId);

            return new AuthCredentials(apiKey, userId, userToken);
        }
    }
}