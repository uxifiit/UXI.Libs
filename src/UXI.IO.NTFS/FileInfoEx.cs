using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinet.Core.IO.Ntfs;

namespace UXI.IO.NTFS
{
    /// <summary>
    /// Provides NTFS-specific extension methods for <see cref="FileInfo"/> objects. 
    /// </summary>
    public static class FileInfoEx
    {
        private const string ZoneIdentifierStreamName = "Zone.Identifier";

        /// <summary>
        /// Deletes the Zone.Identifier data stream from the file, e.g. when it was marked as blocked by the operating system after downloading from the internet.
        /// </summary>
        /// <param name="file">File to unblock</param>
        /// <exception cref="ArgumentNullException">file is null</exception>
        /// <exception cref="FileNotFoundException">file does not exist</exception>
        public static void Unblock(this FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("Unable to find the specified file.", file.FullName);
            }

            if (file.Exists && file.AlternateDataStreamExists(ZoneIdentifierStreamName))
            {
                file.DeleteAlternateDataStream(ZoneIdentifierStreamName);
            }
        }
    }
}
