using CommunityToolkit.Maui.Views;
using StreamChatMaui.Services;

namespace StreamChatMaui.Views;

public partial class MessageContextPopupView : Popup
{

    public MessageContextPopupView(ReactionsRepository reactionsRepository)
	{
		InitializeComponent();
        _reactionsRepository = reactionsRepository;

        ReactionsButtons.ItemsSource = _reactionsRepository.Reactions;
    }

    private readonly ReactionsRepository _reactionsRepository;
}