using System;
using System.Diagnostics;

namespace StreamChat.Libs.Time
{
    /// <summary>
    /// <see cref="ITimeService"/> based on <see cref="UnityEngine.Time"/>
    /// </summary>
    public class ConsoleTime : ITimeService
    {
        public float Time => (float)(DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalSeconds;
        public float DeltaTime => 0.1f;
    }
}