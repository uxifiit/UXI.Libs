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
    [DeploymentItem(@"testfiles\", "testfiles")]
    public class ZipHelperTest
    {
        private const string TEMP_DIR_NAME = "temp";
        private const string TESTFILES_DIR_NAME = "testfiles";
        private const string SESSION_EXPORT_ITEM_NAME = "SessionExport";
        private const string SESSION_EXPORT_ZIP_FILENAME = SESSION_EXPORT_ITEM_NAME + ".zip";


        //private readonly string TempDirectoryLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ZipHelperTest)).Location), TEMP_DIR_NAME);
        //private readonly string TestFilesDirectoryLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ZipHelperTest)).Location), TESTFILES_DIR_NAME);

        [TestInitialize]
        public void Initialize()
        {
            if (Directory.Exists(TEMP_DIR_NAME) == false)
            {
                Directory.CreateDirectory(TEMP_DIR_NAME);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(TEMP_DIR_NAME))
            {
                Directory.Delete(TEMP_DIR_NAME, true);
            }
        }

        [TestMethod]
        public void TryPack_FolderToSingleArchiveTest()
        {
            bool packed = ZipHelper.TryPack
            (
                fileName: Path.Combine(TEMP_DIR_NAME, SESSION_EXPORT_ZIP_FILENAME), 
                folderPath: Path.Combine(TESTFILES_DIR_NAME, SESSION_EXPORT_ITEM_NAME), 
                progress: new Progress<int>(), 
                cancellationToken: CancellationToken.None
            );

            Assert.IsTrue(packed, $"{nameof(ZipHelper)}.{nameof(ZipHelper.Pack)} failed to pack folder.");
            Assert.IsTrue(File.Exists(Path.Combine(TEMP_DIR_NAME, SESSION_EXPORT_ZIP_FILENAME)));
        }

        [TestMethod]
        public void Pack_FolderToSingleArchiveSegmentTest()
        {
            IEnumerable<string> files = ZipHelper.Pack
            (
                fileName: Path.Combine(TEMP_DIR_NAME, SESSION_EXPORT_ZIP_FILENAME),
                folderPath: Path.Combine(TESTFILES_DIR_NAME, SESSION_EXPORT_ITEM_NAME),
                segmentSize: null,
                progress: new Progress<int>(),
                cancellationToken: CancellationToken.None
            );

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Any());
            Assert.AreEqual(1, files.Count());

            string file = files.First();
            Assert.AreEqual(SESSION_EXPORT_ZIP_FILENAME, file);
            Assert.IsTrue(File.Exists(Path.Combine(TEMP_DIR_NAME, file)));
        }

        [TestMethod]
        public void Pack_FolderToMultipleArchiveSegmentsTest()
        {
            int segmentSize = 2 * 1024 * 1024;
            IEnumerable<string> files = ZipHelper.Pack
            (
                fileName: Path.Combine(TEMP_DIR_NAME, SESSION_EXPORT_ZIP_FILENAME),
                folderPath: Path.Combine(TESTFILES_DIR_NAME, SESSION_EXPORT_ITEM_NAME),
                segmentSize: segmentSize,
                progress: new Progress<int>(),
                cancellationToken: CancellationToken.None
            );

            Assert.IsNotNull(files);
            Assert.IsTrue(files.Any());
            Assert.IsTrue(files.Count() > 1);

            Assert.IsTrue(files.Any(f => f.Equals(SESSION_EXPORT_ZIP_FILENAME)));

            string outputFileName = Path.GetFileNameWithoutExtension(files.First());
            foreach (var file in files.Select(f => new FileInfo(Path.Combine(TEMP_DIR_NAME, f))))
            {
                Assert.IsTrue(file.Exists);
                Assert.AreEqual(outputFileName, Path.GetFileNameWithoutExtension(file.Name));

                Assert.IsTrue(file.Length <= segmentSize);
            }
        }
    }
}
