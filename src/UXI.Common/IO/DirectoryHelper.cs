using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.IO
{
    public static class DirectoryHelper
    {
        public static void Move(string sourceDirName, string destDirName, bool overwrite)
        {
            Copy(sourceDirName, destDirName, true, overwrite);
            Directory.Delete(sourceDirName, true);
        }


        //http://stackoverflow.com/questions/2947300/copy-a-directory-to-a-different-drive
        public static void Copy(string sourcePath, string destinationPath, bool recursive, bool overwrite = false)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourcePath);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string destinationFilePath = Path.Combine(destinationPath, file.Name);

                // Copy the file.
                file.CopyTo(destinationFilePath, overwrite);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (recursive)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destinationPath, subdir.Name);

                    // Copy the subdirectories.
                    Copy(subdir.FullName, temppath, recursive, overwrite);
                }
            }
        }
    }
}
