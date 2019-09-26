using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SSMSPlusCore.Infra.Logging
{
    public class FileLoggerOptions
    {
        public string Folder { get; set; }

        public int RetainPolicyFileCount { get; set; } = 10;

        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}
