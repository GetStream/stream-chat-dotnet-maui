using StreamChatMaui.ViewModels;

namespace StreamChatMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}