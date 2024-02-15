using System;

namespace StreamChat.Libs.Time
{
    public class ConsoleTime : ITimeService
    {
        public ConsoleTime()
        {
            _startTime = DateTime.UtcNow;
        }

        public float Time => (float)(DateTime.UtcNow - _startTime).TotalSeconds;
        
        public float DeltaTime => 0.1f;
        
        private readonly DateTime _startTime;
    }
}