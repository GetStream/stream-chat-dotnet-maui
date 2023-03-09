using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.ViewModels;

namespace StreamChatMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		// Services
		builder.Services.AddSingleton<IStreamChatService, StreamChatService>();
		builder.Services.AddSingleton<RouteUrlFactory>();
		
		// View models
		builder.Services.AddSingleton<ChannelsListVM>();
		builder.Services.AddSingleton<ChannelDetailsVM>();

		// Pages
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<ChannelDetailsPage>();

		return builder.Build();
	}
}
