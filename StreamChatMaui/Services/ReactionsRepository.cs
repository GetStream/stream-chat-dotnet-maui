
namespace StreamChatMaui.Services
{
    public class ReactionsRepository
    {
        public IEnumerable<(string Key, string Value)> Reactions => _repository.Select(r => (r.Key, r.Value));

        public bool TryGetValue(string key, out string value) => _repository.TryGetValue(key, out value);

        private Dictionary<string, string> _repository = new()
        {
            { "face_with_tears_of_joy", "\U0001F602" },
            { "loudly_crying_face", "\U0001F62D" },
            { "smiling_face_with_heart_eyes", "\U0001F60D" },
            { "face_blowing_a_kiss", "\U0001F618" },
            { "smiling_face_with_smiling_eyes", "\U0001F60A" },
            { "thumbs_up", "\U0001F44D" },
            { "thumbs_down", "\U0001F44E" },
            { "rolling_eyes", "\U0001F644" },
            { "thinking_face", "\U0001F914" },
            { "neutral_face", "\U0001F610" }
        };
    }
}
