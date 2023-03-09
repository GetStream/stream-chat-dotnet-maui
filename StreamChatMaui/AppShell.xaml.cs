namespace StreamChatMaui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(ChannelDetailsPage), typeof(ChannelDetailsPage));
	}
}
