
namespace StreamChatMaui.Utils
{
    internal static class StringUtils
    {
        public static string TakeSnippet(this string text, int maxLenght)
        {
            var snippet = text[..Math.Min(maxLenght, text.Length)];

            if(text.Length > maxLenght)
            {
                return snippet + "...";
            }

            return snippet;
        }
    }
}
