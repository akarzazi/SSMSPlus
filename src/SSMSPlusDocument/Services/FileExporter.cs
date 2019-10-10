using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Messaging;
using SSMSPlusCore.Utils.IO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSMSPlusDocument.Services
{
    public class FileExporter
    {
        public static async Task ExportFiles(DbConnectionString dbConnectionString, string sqlQuery, string folderPath, CancellationToken Ct, IProgress<ReportMessage> progress)
        {
            using (SqlConnection dbCon = new SqlConnection(dbConnectionString.ConnectionString))
            {
                dbCon.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, dbCon))
                {
                    // 10 hours
                    cmd.CommandTimeout = 60 * 60 * 10;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                Ct.ThrowIfCancellationRequested();

                                var fileName = Convert.ToString(reader.GetValue(0));
                                string validFileName = MakeValidFileName(progress, fileName);
                                var bytes = (byte[])reader.GetValue(1);
                                var fullPath = Path.Combine(folderPath, validFileName);
                                File.WriteAllBytes(fullPath, bytes);

                                progress.Report(ReportMessage.Standard("Exported: " + validFileName));
                            }
                        }
                        finally
                        {
                            cmd.Cancel();
                            reader.Close();
                        }
                    }
                }
            }
        }

        private static string MakeValidFileName(IProgress<ReportMessage> progress, string fileName)
        {
            bool changed;
            var validFileName = FileExtensions.MakeValidFileName(fileName, out changed, (invalidChar) => "_", true);
            if (changed)
            {
                progress.Report(ReportMessage.Warning("Invalid filename: " + fileName));
                progress.Report(ReportMessage.Warning("Replacement: " + validFileName));
            }

            return validFileName;
        }
    }
}
