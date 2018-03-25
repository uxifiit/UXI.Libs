using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXI.Common.Web
{
    public class ProgressStreamContent : HttpContent
    {
        private const int defaultBufferSize = 4096;

        private readonly Stream _content;
        private readonly int _bufferSize;
        private readonly IProgress<int> _progress;

        private readonly CancellationToken _cancellationToken;

        public ProgressStreamContent(Stream content, IProgress<int> progress)
            : this(content, progress, defaultBufferSize)
        { }

        public ProgressStreamContent(Stream content, IProgress<int> progress, CancellationToken cancellationToken)
            : this(content, progress, defaultBufferSize, cancellationToken)
        { }

        public ProgressStreamContent(Stream content, IProgress<int> progress, int bufferSize)
            : this(content, progress, bufferSize, CancellationToken.None)
        { }

        public ProgressStreamContent(Stream content, IProgress<int> progress, int bufferSize, CancellationToken cancellationToken)
        {
            content.ThrowIfNull(nameof(content));
            bufferSize.ThrowIf(b => b <= 0, () => new ArgumentOutOfRangeException(nameof(bufferSize)));

            _content = content;
            _progress = progress;
            _bufferSize = bufferSize;
            _cancellationToken = CancellationToken.None;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            stream.ThrowIfNull(nameof(stream));

            TrySeekContentToStart();

            if (stream.CanWrite)
            {
                return _content.ProgressCopyToAsync(stream, _progress, _bufferSize, _cancellationToken);
            }

            return Task.FromResult(false);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _content.Length;
            return true;
        }

        private void TrySeekContentToStart()
        {
            if (_content.CanSeek)
            {
                _content.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
