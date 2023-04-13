namespace StreamChatMaui.ViewModels
{
    public class ReactionVM : BaseViewModel
    {
        public string Key { get; set; }
        public string Unicode { get; set; }
        public int Count { get; set; }

        public ReactionVM(string type, string unicode, int count)
        {
            Key = type;
            Unicode = unicode;
            Count = count;
        }
    }
}
