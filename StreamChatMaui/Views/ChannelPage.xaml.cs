using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class ChannelDetailsPage : ContentPage
{
    public ChannelDetailsPage(ChannelVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    public void MessageView_ChildAdded(object sender, ElementEventArgs e)
    {
        var viewCell = e.Element.Parent as ViewCell;
        viewCell.Appearing += ViewCell_Appearing;
    }

    //Todo: move to config
    private readonly string[] _popularEmojis = new string[]
    {
        "\U0001F602", // Face with tears of joy
        "\U0001F62D", // Loudly crying face
        "\U0001F60D", // Smiling face with heart-eyes
        "\U0001F618", // Face blowing a kiss
        "\U0001F60A", // Smiling face with smiling eyes
        "\U0001F44D", // Thumbs up
        "\U0001F44E", // Thumbs down
        "\U0001F644", // Rolling eyes
        "\U0001F914", // Thinking face
        "\U0001F610" // Neutral face
    };

    private readonly ChannelVM _vm;

    /// <summary>
    /// We delay ContextActions generation until here because <see cref="MessageView_ChildAdded"/> has not bounded data yet
    /// </summary>
    private void ViewCell_Appearing(object sender, EventArgs e)
    {
        var viewCell = sender as ViewCell;
        viewCell.Appearing -= ViewCell_Appearing;

        var contextActions = viewCell.ContextActions;
        var messageVm = viewCell.BindingContext as MessageVM;

        contextActions.Add(new MenuItem
        {
            Text = "Edit",
        });

        //Todo: don't show if no permissions to delete
        contextActions.Add(new MenuItem
        {
            Text = "Delete",
            Command = _vm.DeleteMessageCommand,
            CommandParameter = messageVm

        });

        foreach (var emoji in _popularEmojis)
        {
            contextActions.Add(new MenuItem
            {
                Text = emoji,
                Command = _vm.AddMessageReactionCommand,
                CommandParameter = messageVm
            });
        }
    }
}