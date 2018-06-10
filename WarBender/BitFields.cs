using System.IO;

namespace WarBender {
    internal struct BitFieldReader {
        private BinaryReader _reader;
        private uint _bits;

        public void Start(BinaryReader reader) {
            _reader = reader;
            _bits = 1;
        }

        public uint ReadBit() {
            if (_bits <= 1) {
                _bits = 0x100u | _reader.ReadByte();
            }

            var bit = _bits & 1u;
            _bits >>= 1;
            return bit;
        }

        public ulong ReadField(uint size) {
            ulong result = 0;
            for (var i = 0; i < size; ++i) {
                result |= ReadBit() << i;
            }
            return result;
        }
    }

    internal struct BitFieldWriter {
        private BinaryWriter _writer;
        private uint _bits;
        private int _current;

        public void Start(BinaryWriter writer) {
            _writer = writer;
            _bits = 0;
            _current = 0;
        }

        public void WriteBit(uint bit) {
            _bits |= bit << _current;
            if (++_current >= 8) {
                _writer.Write((byte)_bits);
                _bits = 0;
                _current = 0;
            }
        }

        public void WriteField(uint size, ulong bits) {
            for (var i = 0; i < size; ++i) {
                WriteBit((uint)(bits & 1u));
                bits >>= 1;
            }
        }
    }
}
