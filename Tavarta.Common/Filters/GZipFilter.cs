﻿using System.IO;
using System.IO.Compression;
using Tavarta.Common.HttpCompress;

namespace Tavarta.Common.Filters
{
    /// <summary>
    ///     This is a little filter to support HTTP compression using GZip
    /// </summary>
    public class GZipFilter : CompressingFilter
    {
        /// <summary>
        ///     compression stream member
        ///     has to be a member as we can only have one instance of the
        ///     actual filter class
        /// </summary>
        private readonly GZipStream m_stream;

        /// <summary>
        ///     Primary constructor.  Need to pass in a stream to wrap up with gzip.
        /// </summary>
        /// <param name="baseStream">The stream to wrap in gzip.  Must have CanWrite.</param>
        public GZipFilter(Stream baseStream)
            : base(baseStream, CompressionLevels.Normal)
        {
            m_stream = new GZipStream(baseStream, CompressionMode.Compress);
        }

        /// <summary>
        ///     The Http name of this encoding.  Here, gzip.
        /// </summary>
        public override string ContentEncoding
        {
            get { return "gzip"; }
        }

        /// <summary>
        ///     Write content to the stream and have it compressed using gzip.
        /// </summary>
        /// <param name="buffer">The bytes to write</param>
        /// <param name="offset">The offset into the buffer to start reading bytes</param>
        /// <param name="count">The number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!HasWrittenHeaders) WriteHeaders();
            m_stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Closes this Filter and calls the base class implementation.
        /// </summary>
        public override void Close()
        {
            m_stream.Close(); // this will close the gzip stream along with the underlying stream
            // no need for call to base.Close() here.
        }

        /// <summary>
        ///     Flushes the stream out to underlying storage
        /// </summary>
        public override void Flush()
        {
            m_stream.Flush();
        }
    }
}