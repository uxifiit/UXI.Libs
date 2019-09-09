using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UXI.ZIP.Test
{
    [TestClass]
    public class ZipHelperTest
    {
        private const string TEMP_DIR_NAME = "temp";
        private const string TESTFILES_DIR_NAME = "testfiles";
        private const string SESSION_EXPORT_ITEM_NAME = "SessionExport";
        private const string SESSION_EXPORT_ZIP_FILENAME = SESSION_EXPORT_ITEM_NAME + ".zip";

        private static string GetAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(typeof(ZipHelperTest)).Location);
        }

        private readonly string TempDirectoryFullPath = Path.Combine(GetAssemblyPath(), TEMP_DIR_NAME);
        private readonly string TestFilesDirectoryFullPath = Path.Combine(GetAssemblyPath(), TESTFILES_DIR_NAME);


        private void CreateDummyFile(string directory, string filename, int sizeInKilobytes)
        {
            if (Directory.Exists(directory) == false)
            {
                throw new DirectoryNotFoundException($"Target directory does not exist at: {directory}");
            }

            string path = Path.Combine(directory, filename);

            const int seed = 17;
            var random = new Random(seed);
            byte[] buffer = new byte[1024];

            using (var fs = new FileStream(path, FileMode.Create))
            {
                while (sizeInKilobytes-- > 0)
                {
                    random.NextBytes(buffer);
                    fs.Write(buffer, 0, 1024);
                }
                fs.Close();
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            Directory.CreateDirectory(TestFilesDirectoryFullPath);

            string sessionFiles = Path.Combine(TestFilesDirectoryFullPath, SESSION_EXPORT_ITEM_NAME);
            Directory.CreateDirectory(sessionFiles);

            CreateDummyFile(sessionFiles, "ET_data.json", 10 * 1024);
            CreateDummyFile(sessionFiles, "ET_status.json", 100);
            CreateDummyFile(sessionFiles, "session.json", 1 * 1024);
            CreateDummyFile(sessionFiles, "empty_file.txt", 0); 

            Directory.CreateDirectory(TempDirectoryFullPath);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(TestFilesDirectoryFullPath))
            {
                Directory.Delete(TestFilesDirectoryFullPath, true);
            }

            if (Directory.Exists(TempDirectoryFullPath))
            {
                Directory.Delete(TempDirectoryFullPath, true);
            }
        }


        [TestMethod]
        public void TryPack_FolderToSingleArchiveTest()
        {
            bool packed = ZipHelper.TryPack
            (
                fileName: Path.Combine(TempDirectoryFullPath, SESSION_EXPORT_ZIP_FILENAME), 
                folderPath: Path.Combine(TestFilesDirectoryFullPath, SESSION_EXPORT_ITEM_NAME), 
                progress: new Progress<int>(), 
                cancellationToken: CancellationToken.None
            );

            Assert.IsTrue(packed, $"{nameof(ZipHelper)}.{nameof(ZipHelper.Pack)} failed to pack folder.");
            Assert.IsTrue(File.Exists(Path.Combine(TempDirectoryFullPath, SESSION_EXPORT_ZIP_FILENAME)));
        }


        [TestMethod]
        public void Pack_FolderToSingleArchiveSegmentTest()
        {
            IEnumerable<string> files = ZipHelper.Pack
            (
                fileName: Path.Combine(TempDirectoryFullPath, SESSION_EXPORT_ZIP_FILENAME),
                folderPath: Path.Combine(TestFilesDirectoryFullPath, SESSION_EXPORT_ITEM_NAME),
                segmentSize: null,
                progress: new Progress<int>(),
                cancellationToken: CancellationToken.None
            );

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Any());
            Assert.AreEqual(1, files.Count());

            string file = files.First();
            Assert.AreEqual(SESSION_EXPORT_ZIP_FILENAME, file);
            Assert.IsTrue(File.Exists(Path.Combine(TempDirectoryFullPath, file)));
        }


        [TestMethod]
        public void Pack_FolderToMultipleArchiveSegmentsTest()
        {
            int segmentSize = 2 * 1024 * 1024;
            IEnumerable<string> files = ZipHelper.Pack
            (
                fileName: Path.Combine(TempDirectoryFullPath, SESSION_EXPORT_ZIP_FILENAME),
                folderPath: Path.Combine(TestFilesDirectoryFullPath, SESSION_EXPORT_ITEM_NAME),
                segmentSize: segmentSize,
                progress: new Progress<int>(),
                cancellationToken: CancellationToken.None
            );

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Any());
            Assert.AreEqual(6, files.Count());

            Assert.IsTrue(files.Any(f => f.Equals(SESSION_EXPORT_ZIP_FILENAME)));

            string outputFileName = Path.GetFileNameWithoutExtension(files.First());
            foreach (var file in files.Select(f => new FileInfo(Path.Combine(TempDirectoryFullPath, f))))
            {
                Assert.IsTrue(file.Exists);
                Assert.AreEqual(outputFileName, Path.GetFileNameWithoutExtension(file.Name));

                Assert.IsTrue(file.Length <= segmentSize);
            }
        }
    }
}
