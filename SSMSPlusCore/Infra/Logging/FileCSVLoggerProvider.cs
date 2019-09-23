using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SSMSPlusCore.Infra.Logging
{
    [Microsoft.Extensions.Logging.ProviderAlias("FileCSV")]
    public class FileCSVLoggerProvider : LoggerProvider
    {
        private string _currentFilePath;
        private int timeLength = 24;
        private int levelLength = 14;
        private int categoryLength = 60;

        ConcurrentQueue<CsvLogEntry> InfoQueue = new ConcurrentQueue<CsvLogEntry>();

        void ApplyRetainPolicy()
        {
            FileInfo FI;
            try
            {
                List<FileInfo> FileList = new DirectoryInfo(Settings.Folder)
                .GetFiles("*.log", SearchOption.TopDirectoryOnly)
                .OrderBy(fi => fi.CreationTime)
                .ToList();

                while (FileList.Count >= Settings.RetainPolicyFileCount)
                {
                    FI = FileList.First();
                    FI.Delete();
                    FileList.Remove(FI);
                }
            }
            catch
            {
            }
        }



        string Pad(string Text, int MaxLength)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return "".PadRight(MaxLength);

            //if (Text.Length > MaxLength)
            //    return Text.Substring(0, MaxLength);

            return Text.PadRight(MaxLength);
        }

        void BeginFile()
        {
            Directory.CreateDirectory(Settings.Folder);
            this._currentFilePath = Path.Combine(Settings.Folder, "log_" + DateTime.Now.ToString("yyyy-MM-dd@HH_mm") + ".csv");

            using (var writer = new StreamWriter(this._currentFilePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteHeader<CsvLogEntry>();
                csv.NextRecord();
            }

            ApplyRetainPolicy();
        }

        private CsvLogEntry ConvertToCsvLogEntry(LogEntry Info)
        {
            var csvEntry = new CsvLogEntry();
            csvEntry.LocalDate = Pad(Info.TimeStampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.ff"), timeLength);
            csvEntry.Level = Pad(Info.Level.ToString(), levelLength);
            csvEntry.Category = Pad(Info.Category, categoryLength);
            csvEntry.Text = Info.Text;

            //if (Info.StateProperties != null && Info.StateProperties.Count > 0)
            //{
            //    csvEntry.Properties = JsonConvert.SerializeObject(Info.StateProperties);
            //}

            return csvEntry;
        }

        public FileCSVLoggerProvider(FileLoggerOptions Settings)
        {
            this.Settings = Settings;
            BeginFile();
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            bool Result = logLevel != LogLevel.None
                && this.Settings.LogLevel != LogLevel.None
                && Convert.ToInt32(logLevel) >= Convert.ToInt32(this.Settings.LogLevel);

            return Result;
        }

        public override void WriteLog(LogEntry Info)
        {
            InfoQueue.Enqueue(ConvertToCsvLogEntry(Info));

            Task.Delay(1000).ContinueWith((t) => this.SavePendingLogs());
        }

        private void SavePendingLogs()
        {
            // prevent concurrent access to the file
            lock (InfoQueue)
            {
                var logEntries = new List<CsvLogEntry>();
                CsvLogEntry logEntry;

                while (InfoQueue.TryDequeue(out logEntry))
                {
                    logEntries.Add(logEntry);
                }

                if (logEntries.Count > 0)
                {
                    WriteLines(logEntries);
                }
            }
        }

        void WriteLines(IEnumerable<CsvLogEntry> csvLogLines)
        {
            using (var writer = new StreamWriter(this._currentFilePath, append: true))
            using (var csv = new CsvWriter(writer, new Configuration { HasHeaderRecord = false }))
            {
                csv.WriteRecords(csvLogLines);
            }
        }

        internal FileLoggerOptions Settings { get; private set; }
    }
}
