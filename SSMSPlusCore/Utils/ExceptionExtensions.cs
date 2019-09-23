using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Utils
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get full stack trace message with the exception message
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>Error message detail</returns>
        public static string GetFullStackTraceWithMessage(this Exception ex)
        {
            var stackTrace = new StringBuilder();

            stackTrace.AppendLine(ex.Message);
            stackTrace.AppendLine(GetFullStackTrace(ex));
            return stackTrace.ToString();
        }

        /// <summary>
        /// Get full stack trace message
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>Error message detail</returns>
        public static string GetFullStackTrace(this Exception ex)
        {
            var stackTrace = new StringBuilder();

            stackTrace.AppendLine(ex.GetType().FullName);
            stackTrace.AppendLine(ex.StackTrace);

            for (var exception = ex.InnerException; exception != null; exception = exception.InnerException)
            {
                stackTrace.AppendLine("- Caused by: " + exception.Message);
                stackTrace.AppendLine(exception.GetType().FullName);
                stackTrace.AppendLine(exception.StackTrace);
            }

            return stackTrace.ToString();
        }
    }
}
