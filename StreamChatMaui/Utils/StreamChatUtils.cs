using StreamChat.Core;
using StreamChat.Core.StatefulModels;
using System.Text;

namespace StreamChatMaui.Utils
{
    /// <summary>
    /// Utility class for <see cref="IStreamChatClient"/> related components
    /// </summary>
    public static class StreamChatUtils
    {
        /// <summary>
        /// Generate Channel title in the following order:
        /// - If channel has a name -> display Name
        /// - If channel has an ID -> display ID
        /// - If channel is a group chat for a unique combination of users -> display participants names
        /// </summary>
        public static string GenerateChannelTitle(this IStreamChannel channel, int maxLength)
        {
            if(maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException($"Expected {nameof(maxLength)} to be greater than 0");
            }

            if (!string.IsNullOrEmpty(channel.Name))
            {
                return channel.Name;
            }

            if (!string.IsNullOrEmpty(channel.Id))
            {
                return channel.Id;
            }

            // if no Name and Id it means it's a channel for a unique group of users

            var sb = new StringBuilder();

            for (var i = 0; i < channel.Members.Count; i++)
            {
                var member = channel.Members[i];

                sb.Append(member.User.Name);

                if (sb.Length > maxLength)
                {
                    break;
                }

                if (i < channel.Members.Count)
                {
                    sb.Append(", ");
                }
            }

            if (sb.Length > maxLength)
            {
                sb.Length = maxLength;
                sb.Append("...");
            }

            return sb.ToString();
        }

        public static string GenerateRandomName()
        {
            var names = new[]
            {
                "Jeffrey",
                "Tom",
                "John",
                "Bob",
                "Suzie",
                "Monica",
                "Veronica",
                "Mia"
            };
            
            var rnd = new Random();
            var randomIndex = rnd.Next(names.Length);
            
            return names[randomIndex];
        }
    }
}
