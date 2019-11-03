using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common;

namespace UXI.Common.IO
{
    public class TempFile : DisposableBase
    {
        public static string GetDateRandomName()
        {
            return DateTime.Today.ToString("yyyy-MM-dd_") + System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
        }

        private TempFile(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static TempFile Create(string path)
        {
            return new TempFile(path);
        }

        public static TempFile Create(string folder, string filename)
        {
            return new TempFile(System.IO.Path.Combine(folder, filename));
        }

        public static TempFile CreateInTempFolder()
        {
            return new TempFile(System.IO.Path.GetTempFileName());
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    try
                    {
                        string path = Path;
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
