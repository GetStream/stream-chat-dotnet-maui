
using StreamChatMaui.Models;

namespace StreamChatMaui.Services
{
    public class ReactionsRepository
    {
        public IEnumerable<ReactionOption> Reactions => _repository;

        public bool TryGetValue(string key, out ReactionOption value) => _lookupTable.TryGetValue(key, out value);

        public ReactionsRepository()
        {
            foreach(var r in _repository)
            {
                _lookupTable.Add(r.Key, r);
            }
        }

        private readonly List<ReactionOption> _repository = new List<ReactionOption>()
        {
            new ReactionOption { Key = "face_with_tears_of_joy", Value = "\U0001F602" },
            new ReactionOption { Key = "loudly_crying_face", Value = "\U0001F62D" },
            new ReactionOption { Key = "smiling_face_with_heart_eyes", Value = "\U0001F60D" },
            new ReactionOption { Key = "face_blowing_a_kiss", Value = "\U0001F618" },
            new ReactionOption { Key = "smiling_face_with_smiling_eyes", Value = "\U0001F60A" },
            new ReactionOption { Key = "thumbs_up", Value = "\U0001F44D" },
            new ReactionOption { Key = "thumbs_down", Value = "\U0001F44E" },
            new ReactionOption { Key = "rolling_eyes", Value = "\U0001F644" },
            new ReactionOption { Key = "thinking_face", Value = "\U0001F914" },
            new ReactionOption { Key = "neutral_face", Value = "\U0001F610" }
        };

        private readonly Dictionary<string, ReactionOption> _lookupTable = new Dictionary<string, ReactionOption>();
    }
}
