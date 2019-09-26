using SSMSPlusCore.Integration.Connection;
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
        public static async Task ExportFiles(DbConnectionString dbConnectionString, string sqlQuery, string folderPath, CancellationToken Ct, IProgress<string> progress)
        {
            using (SqlConnection dbCon = new SqlConnection(dbConnectionString.ConnectionString))
            {
                dbCon.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, dbCon))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Ct.ThrowIfCancellationRequested();

                            var fileName = Convert.ToString(reader.GetValue(0));
                            var bytes = (byte[])reader.GetValue(1);

                            var fullPath = Path.Combine(folderPath, fileName);
                            File.WriteAllBytes(fullPath, bytes);

                            progress.Report(fileName);
                        }
                    }
                }
            }
        }
    }
}
