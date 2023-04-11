using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StreamChatMaui.Services;
using StreamChatMaui.ViewModels;
using StreamChatMaui.Views;

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
				fonts.AddFont("SF-Pro-Display-Regular.ttf", "SFProRegular");
			}).UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		// Services
		builder.Services.AddSingleton<IStreamChatService, StreamChatService>();
		builder.Services.AddSingleton<IChatPermissionsService, ChatPermissionsService>();
		builder.Services.AddSingleton<RouteUrlFactory>();
		builder.Services.AddSingleton<IViewModelFactory, ViewModelFactory>();
		
		// View models
		builder.Services.AddSingleton<MainPageVM>();
		builder.Services.AddTransient<ChannelVM>();
		builder.Services.AddTransient<MessageVM>();

		// Pages
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<ChannelDetailsPage>();

		return builder.Build();
	}
}