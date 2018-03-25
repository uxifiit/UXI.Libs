using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UXI.Common.Helpers
{
    public static class FilenameHelper
    {
        private static readonly string _invalidCharsRegExp = $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]+";

        private static readonly string[] _reservedWords = new[]
        {
            "CON", "PRN", "AUX", "CLOCK$", "NUL", "COM0", "COM1", "COM2", "COM3", "COM4",
            "COM5", "COM6", "COM7", "COM8", "COM9", "LPT0", "LPT1", "LPT2", "LPT3", "LPT4",
            "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };


        /// <summary>
        /// Replaces all the invalid characters specified by the <see cref="Path.GetInvalidFileNameChars"/> with the specified string.
        /// </summary>
        /// <param name="filename">Filename string to replace.</param>
        /// <param name="replaceString">String used to replace each invalid character.</param>
        /// <returns>String with replaced invalid filename characters.</returns>
        public static string ReplaceInvalidFileNameChars(string filename, string replaceString = "")
        {
            return Regex.Replace(filename, _invalidCharsRegExp, replaceString);
        }


        public static string ReplaceReservedWords(string filename, string replaceString = "_reservedWord_")
        {
            return _reservedWords.Select(w => String.Format("^{0}\\.", w))
                                 .Aggregate(filename, (f, r) => Regex.Replace(f, r, $"{replaceString}.", RegexOptions.IgnoreCase));
        }
    }
}
