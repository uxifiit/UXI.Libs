using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXI.Common
{
    public class CompositeProgress<T, P> : IProgress<P>
    {
        private readonly IProgress<T> _progress;
        private readonly Action<P, T> _convert;

        public CompositeProgress(IProgress<T> progress, T status, Action<P, T> convert)
        {
            _progress = progress;
            _convert = convert;
            Status = status;
        }

        public T Status { get; private set; }

        public void Report(P status)
        {
            _convert.Invoke(status, Status);
            _progress.Report(Status);
        }

        public void Report(T status)
        {
            if (status.Equals(Status) == false)
            {
                Status = status;
            }
            _progress.Report(Status);
        }

        public void Report()
        {
            _progress.Report(Status);
        }
    }
}
