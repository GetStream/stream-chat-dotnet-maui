using StreamChat.Libs.AppInfo;
using StreamChat.Libs.Auth;
using StreamChat.Libs.ChatInstanceRunner;
using StreamChat.Libs.Http;
using StreamChat.Libs.Loggers;
using StreamChat.Libs.Serialization;
using StreamChat.Libs.Time;
using StreamChat.Libs.Websockets;

namespace StreamChat.Libs
{
    /// <summary>
    /// Factory that provides external dependencies for the Stream Chat Client.
    /// Stream chat client depends only on the interfaces therefore you can provide your own implementation for any of the dependencies
    /// </summary>
    public static class StreamDependenciesFactory
    {
        public static ILogs CreateLogger(LogLevel logLevel = LogLevel.All)
            => new ConsoleLogs(logLevel);

        public static IWebsocketClient CreateWebsocketClient(ILogs logs, bool isDebugMode = false) => new WebsocketClient(logs, isDebugMode: isDebugMode);

        public static IHttpClient CreateHttpClient() => new HttpClientAdapter();

        public static ISerializer CreateSerializer() => new NewtonsoftJsonSerializer();

        public static ITimeService CreateTimeService() => new ConsoleTime();

        public static IApplicationInfo CreateApplicationInfo() => new ConsoleApplicationInfo();
        
        public static ITokenProvider CreateTokenProvider(TokenProvider.TokenUriHandler urlFactory) => new TokenProvider(CreateHttpClient(), urlFactory);

        public static IStreamChatClientRunner CreateChatClientRunner() => null;
    }
}