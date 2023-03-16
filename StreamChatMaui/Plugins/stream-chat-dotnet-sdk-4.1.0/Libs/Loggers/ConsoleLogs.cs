namespace StreamChat.Libs.Loggers
{
    public class ConsoleLogs : ILogs
    {
        public ConsoleLogs(LogLevel logLevel = LogLevel.All)
        {
            _logLevel = logLevel;
        }
        
        public string Prefix { get => $"{GetTimestamp()} {_prefix}"; set => _prefix = value; }
        
        public void Info(string message)
        {
            if ((_logLevel & LogLevel.Info) != 0)
            {
                Console.WriteLine(Prefix + message);
            }
        }

        public void Warning(string message)
        {
            if ((_logLevel & LogLevel.Warning) != 0)
            {
                Console.WriteLine(Prefix + message);
            }
        }

        public void Error(string message)
        {
            if ((_logLevel & LogLevel.Error) != 0)
            {
                Console.WriteLine(Prefix + message);
            }
        }

        public void Exception(Exception exception)
        {
            if ((_logLevel & LogLevel.Exception) != 0)
            {
                Console.WriteLine(exception);
            }
        }

        private static string GetTimestamp() => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();

        private string _prefix;
        private readonly LogLevel _logLevel;
    }
}