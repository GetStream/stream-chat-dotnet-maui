using System;
using System.Text;
using System.Threading.Tasks;
using StreamChat.Libs.Logs;

namespace StreamChat.Libs.Utils
{
    public static class TaskUtils
    {
        public static void LogIfFailed(this Task t, ILogs logger)
            => t.ContinueWith(_ =>
                {
                    if (!_.IsFaulted)
                    {
                        return;
                    }

                    Console.WriteLine(_.Exception);
                });

        /// <summary>
        /// Log exception thrown by this task with Debug.LogException
        /// </summary>
        /// <param name="t"></param>
        public static void LogIfFailed(this Task t)
            => t.ContinueWith(_ =>
                {
                    if (!_.IsFaulted)
                    {
                        return;
                    }

                    //Skip Debug.LogException because it doesn't print well nested exceptions, it just prints the most inner one
                    _sb.Length = 0;
                    Exception exception = _.Exception.Flatten();
                    while (exception != null)
                    {
                        if (exception is AggregateException)
                        {
                            exception = exception.InnerException;
                            continue;
                        }
                        _sb.AppendLine(exception.ToString());
                        _sb.AppendLine(exception.StackTrace);
                        _sb.AppendLine(Environment.NewLine);
                        _sb.AppendLine(Environment.NewLine);
                        
                        exception = exception.InnerException;
                    }

                    if (_sb.Length > 0)
                    {
                        Console.WriteLine(_sb.ToString());
                        _sb.Length = 0;
                    }
                });

        private static  readonly StringBuilder _sb = new StringBuilder();
    }
}