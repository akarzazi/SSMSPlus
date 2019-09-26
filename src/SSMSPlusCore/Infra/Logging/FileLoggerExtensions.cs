using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SSMSPlusCore.Infra.Logging
{
    static public class FileLoggerExtensions
    {

        static public ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Func<FileLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileCSVLoggerProvider>(services => new FileCSVLoggerProvider(configure()));

           // builder.AddFilter<FileLoggerProvider>(null, LogLevel.Trace);

            return builder;
        }
    }
}
