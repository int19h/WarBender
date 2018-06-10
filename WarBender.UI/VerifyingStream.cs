using System;
using System.Diagnostics;
using System.IO;

namespace WarBender.UI {
    internal class VerifyingStream : Stream, IHasName {
        public string Name => nameof(VerifyingStream) + " for " + BaseStream.FileName();

        public Stream BaseStream { get; }

        public VerifyingStream(Stream baseStream) {
            BaseStream = baseStream;
            Trace.WriteLine(baseStream.FileName(), nameof(VerifyingStream));
        }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => true;

        public override long Length => BaseStream.Length;

        public override long Position {
            get => BaseStream.Position;
            set => BaseStream.Position = Position;
        }

        public override void Flush() =>
            BaseStream.Flush();

        public override long Seek(long offset, SeekOrigin origin) =>
            BaseStream.Seek(offset, origin);

        public override void SetLength(long value) =>
            throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count) =>
            BaseStream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count) {
            var original = new byte[count];
            var pos = BaseStream.Position;
            if (BaseStream.Read(original, 0, count) != count) {
                var msg = $"Mismatch at {pos:X08}: end of stream while reading {count} bytes";
                Trace.WriteLine(msg, nameof(VerifyingStream));
                throw new InvalidDataException(msg);
            }
            for (int i = 0; i < count; ++i, ++pos) {
                if (buffer[i] != original[i]) {
                    var msg = $"Mismatch at {pos:X08}: expected {original[i]:X02}, but got {buffer[i]:X02}";
                    Trace.WriteLine(msg, nameof(VerifyingStream));
                    throw new InvalidDataException(msg);
                }
            }
        }
    }
}
