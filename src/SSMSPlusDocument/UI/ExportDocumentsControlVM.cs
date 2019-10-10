namespace SSMSPlusDocument.UI
{
    using Microsoft.Win32;
    using SSMSPlusCore.Integration.Connection;
    using SSMSPlusCore.Messaging;
    using SSMSPlusCore.Ui;
    using SSMSPlusCore.Ui.Text;
    using SSMSPlusCore.Utils;
    using SSMSPlusDocument.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ExportDocumentsControlVM : ViewModelBase
    {
        private DbConnectionString _dbConnectionString;

        public Command ChooseFolderCmd { get; private set; }
        public AsyncCommand ExportFilesCmd { get; private set; }
        public Command CancelExportFilesCmd { get; private set; }

        public CancellationTokenSource CancelToken { get; private set; }
        public RunStream ConsoleOutput { get; private set; }

        public ExportDocumentsControlVM()
        {
            ChooseFolderCmd = new Command(OnChooseFolder, null, HandleError);
            ExportFilesCmd = new AsyncCommand(OnExportFiles, null, HandleError);
            CancelExportFilesCmd = new Command(OnCancelExportFiles, null, HandleError);
            ConsoleOutput = new RunStream();
        }

        public void InitializeDb(DbConnectionString cnxStr)
        {
            _dbConnectionString = cnxStr;
            DbDisplayName = _dbConnectionString.DisplayName;
        }

        private async Task OnExportFiles()
        {
            this.CancelToken = new CancellationTokenSource();
            IsExporting = true;
            ConsoleOutput.SendStandard("Starting Export");
            var exportedProgress = new Progress<ReportMessage>(OnExportProgress);
            try
            {
                await FileExporter.ExportFiles(_dbConnectionString, SqlQuery, FolderPath, this.CancelToken.Token, exportedProgress);
                ConsoleOutput.SendSuccess("Export finished");
            }
            catch (OperationCanceledException)
            {
                Message = "Cancelled";
                ConsoleOutput.SendStandard("Export cancelled");
            }
            finally
            {
                IsExporting = false;
            }
        }

        private void OnExportProgress(ReportMessage obj)
        {
            switch (obj.Level)
            {
                case ReportMessageLevel.Warning:
                    ConsoleOutput.SendWarning(obj.Message);
                    break;
                case ReportMessageLevel.Error:
                    ConsoleOutput.SendError(obj.Message);
                    break;
                default:
                    ConsoleOutput.SendStandard(obj.Message);
                    break;
            }
        }

        private void OnCancelExportFiles()
        {
            this.CancelToken.Cancel();
        }

        private void OnChooseFolder()
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == true)
            {
                FolderPath = Path.GetDirectoryName(folderBrowser.FileName);
                IsValidFolderPath = true;
            }
        }

        private void HandleError(Exception ex)
        {
            ConsoleOutput.SendError(ex.GetFullStackTraceWithMessage());
        }

        private string _sqlQuery = "Select top 10 FileName, FileContent FROM exploitation.Files";
        public string SqlQuery
        {
            get => _sqlQuery;
            set => SetField(ref _sqlQuery, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        private string _folderPath;
        public string FolderPath
        {
            get => _folderPath;
            set => SetField(ref _folderPath, value);
        }

        private bool _isValidFolderPath;
        public bool IsValidFolderPath
        {
            get => _isValidFolderPath;
            set
            {
                SetField(ref _isValidFolderPath, value);
                RaisePropertyChanged(nameof(CanExport));
            }
        }

        private bool _isExporting;
        public bool IsExporting
        {
            get => _isExporting;
            set
            {
                SetField(ref _isExporting, value);
                RaisePropertyChanged(nameof(CanExport));
            }
        }

        public bool CanExport
        {
            get => !IsExporting && IsValidFolderPath;
        }

        private string _dbDisplayName;
        public string DbDisplayName
        {
            get => _dbDisplayName;
            set => SetField(ref _dbDisplayName, value);
        }

        public void Free()
        {
            this.CancelToken?.Cancel();
        }
    }
}
