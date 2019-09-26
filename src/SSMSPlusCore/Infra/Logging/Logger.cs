using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SSMSPlusCore.Infra.Logging
{
    internal class Logger : ILogger
    {
        public Logger(LoggerProvider Provider, string Category)
        {
            this.Provider = Provider;
            this.Category = Category;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return Provider.IsEnabled(logLevel);
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!(this as ILogger).IsEnabled(logLevel))
                return;

            LogEntry Info = new LogEntry();
            Info.Category = this.Category;
            Info.Level = logLevel;
            Info.Text = formatter(state, exception) + (exception != null ? "\n" + exception.ToString() : "");
            Info.Exception = exception;
            Info.EventId = eventId;
            Info.State = state;


            if (state is IEnumerable<KeyValuePair<string, object>> Properties)
            {
                Info.StateProperties = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> item in Properties)
                {
                    if (item.Key == "{OriginalFormat}")
                        continue;

                    Info.StateProperties[item.Key] = item.Value;
                }
            }

            Provider.WriteLog(Info);
        }

        public LoggerProvider Provider { get; private set; }
        public string Category { get; private set; }
    }

    public class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        {
        }
    }
}
