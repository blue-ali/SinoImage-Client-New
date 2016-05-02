using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocScanner.LibCommon
{
    public class MessageService : IDisposable
    {
        public void LogMessage(EMessageType type, string msg)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(type, msg));
            }

            FileLogger.LogMessage(type, msg);
        }

        public void LogInfo(string msg)
        {
            LogMessage(EMessageType.Info, msg);
        }

        public void LogError(string msg)
        {
            LogMessage(EMessageType.Error, msg);
        }

        public void LogDebug(string msg)
        {
            LogMessage(EMessageType.Debug, msg);
        }

        public void LogWarning(string msg)
        {
            LogMessage(EMessageType.Warning, msg);
        }

        public void LogSuccess(string msg)
        {
            LogMessage(EMessageType.Success, msg);
        }

        public void Dispose()
        {
            this.ClearEvents();
        }

        public event EventHandler<MessageEventArgs> OnMessage;
    }
}