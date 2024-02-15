using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace StreamChat.Libs.ChatInstanceRunner
{
    public class MauiChatClientRunner : IStreamChatClientRunner
    {
        public void RunChatInstance(IStreamChatClientEventsListener streamChatInstance)
        {
            if(_timer != null)
            {
                return;
            }

            _streamChatInstance = streamChatInstance ?? throw new ArgumentNullException(nameof(streamChatInstance));
            _streamChatInstance.Disposed += OnStreamChatInstanceDisposed;

            _timer = new Timer(16);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private IStreamChatClientEventsListener _streamChatInstance;
        private Timer _timer;

        private void OnTick(Object source, ElapsedEventArgs e)
        {
            // We call updated from the UI thread so that any chat client events also get called from the UI thread
            MainThread.BeginInvokeOnMainThread(_streamChatInstance.Update);
        }

        private void OnStreamChatInstanceDisposed()
        {
            if (_streamChatInstance == null)
            {
                return;
            }

            _streamChatInstance.Disposed -= OnStreamChatInstanceDisposed;
            _streamChatInstance = null;

            _timer.Stop();
        }
    }
}
