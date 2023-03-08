using Microsoft.Extensions.Logging;

namespace StreamChatMaui.Utils
{
    /// <summary>
    /// Task extensions
    /// </summary>
    internal static class TaskExt
    {
        public static void LogIfFailed(this Task task, ILogger logger)
            => task.ContinueWith(t => logger.LogError(t.Exception.Message), TaskContinuationOptions.OnlyOnFaulted);
    }
}