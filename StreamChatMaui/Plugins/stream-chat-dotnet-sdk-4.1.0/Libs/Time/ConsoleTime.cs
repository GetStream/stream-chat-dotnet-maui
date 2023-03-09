namespace StreamChat.Libs.Time
{
    /// <summary>
    /// <see cref="ITimeService"/> based on <see cref="UnityEngine.Time"/>
    /// </summary>
    public class ConsoleTime : ITimeService
    {
        public float Time => (float)(DateTime.UtcNow - _timeStarted).TotalSeconds;
        public float DeltaTime => 0.1f;

        private readonly DateTime _timeStarted = DateTime.UtcNow;
    }
}