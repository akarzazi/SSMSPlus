using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Utils.IO
{
    public class FileExtensions
    {
        /// <summary>Replaces characters in <c>text</c> that are not allowed in 
        /// file names with the specified replacement character.</summary>
        /// <param name="text">Text to make into a valid filename. The same string is returned if it is valid already.</param>
        /// <param name="replacement">Replacement character, or null to simply remove bad characters.</param>
        /// <param name="fancy">Whether to replace quotes and slashes with the non-ASCII characters ” and ⁄.</param>
        /// <returns>A string that can be used as a filename. If the output string would otherwise be empty, returns "_".</returns>
        public static string MakeValidFileName(string text, out bool changed, Func<char, string> replacement, bool fancy = true)
        {
            StringBuilder sb = new StringBuilder(text.Length);
            var invalids = Path.GetInvalidFileNameChars();
            changed = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (invalids.Contains(c))
                {
                    changed = true;
                    var repl = replacement(c);
                    if (fancy)
                    {
                        if (c == '"') repl = "”"; // U+201D right double quotation mark
                        else if (c == '\'') repl = "’"; // U+2019 right single quotation mark
                        else if (c == '/') repl = "⁄"; // U+2044 fraction slash
                    }

                    sb.Append(repl);
                }
                else
                    sb.Append(c);
            }
            return changed ? sb.ToString() : text;
        }
    }
}
