using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace WarBender {
    internal interface IValueSerializer<T> {
        T Read(BinaryReader reader);
        void Write(BinaryWriter writer, T value);
    }

    internal interface IBatchValueSerializer<T> {
        T[] Read(BinaryReader reader, int count);
        void Write(BinaryWriter writer, T[] value);
    }

    internal sealed partial class ValueSerializer :
        IValueSerializer<sbyte>,
        IValueSerializer<byte>,
        IValueSerializer<short>,
        IValueSerializer<ushort>,
        IValueSerializer<int>,
        IBatchValueSerializer<int>,
        IValueSerializer<uint>,
        IValueSerializer<long>,
        IBatchValueSerializer<long>,
        IValueSerializer<ulong>,
        IValueSerializer<float>,
        IValueSerializer<bool>,
        IValueSerializer<Color>,
        IValueSerializer<string> {

        public static readonly ValueSerializer Instance = new ValueSerializer();

        private ValueSerializer() { }

        public static IValueSerializer<T> TryGet<T>() {
            var t = typeof(T);
            if (t.IsEnum) {
                return EnumSerializer.Get<T>();
            } else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                return (IValueSerializer<T>)NullableSerializer.Get(t.GenericTypeArguments[0]);
            }
            return (object)Instance as IValueSerializer<T>;
        }

        public static IValueSerializer<T> Get<T>() =>
            TryGet<T>() ?? throw new ArgumentException();

        private unsafe T[] ReadBatch<T>(BinaryReader reader, int count)
            where T : unmanaged {
            var values = new T[count];
            var bytes = reader.ReadBytes(Buffer.ByteLength(values));
            Buffer.BlockCopy(bytes, 0, values, 0, bytes.Length);
            return values;
        }

        private unsafe void WriteBatch<T>(BinaryWriter writer, T[] values)
            where T : unmanaged {
            var bytes = new byte[Buffer.ByteLength(values)];
            Buffer.BlockCopy(values, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
        }

        sbyte IValueSerializer<sbyte>.Read(BinaryReader reader) => reader.ReadSByte();

        void IValueSerializer<sbyte>.Write(BinaryWriter writer, sbyte value) => writer.Write(value);

        byte IValueSerializer<byte>.Read(BinaryReader reader) => reader.ReadByte();

        void IValueSerializer<byte>.Write(BinaryWriter writer, byte value) => writer.Write(value);

        short IValueSerializer<short>.Read(BinaryReader reader) => reader.ReadInt16();

        void IValueSerializer<short>.Write(BinaryWriter writer, short value) => writer.Write(value);

        ushort IValueSerializer<ushort>.Read(BinaryReader reader) => reader.ReadUInt16();

        void IValueSerializer<ushort>.Write(BinaryWriter writer, ushort value) => writer.Write(value);

        int IValueSerializer<int>.Read(BinaryReader reader) => reader.ReadInt32();

        void IValueSerializer<int>.Write(BinaryWriter writer, int value) => writer.Write(value);

        uint IValueSerializer<uint>.Read(BinaryReader reader) => reader.ReadUInt32();

        int[] IBatchValueSerializer<int>.Read(BinaryReader reader, int count) => ReadBatch<int>(reader, count);

        void IBatchValueSerializer<int>.Write(BinaryWriter writer, int[] values) => WriteBatch(writer, values);

        void IValueSerializer<uint>.Write(BinaryWriter writer, uint value) => writer.Write(value);

        long IValueSerializer<long>.Read(BinaryReader reader) => reader.ReadInt64();

        void IValueSerializer<long>.Write(BinaryWriter writer, long value) => writer.Write(value);

        long[] IBatchValueSerializer<long>.Read(BinaryReader reader, int count) => ReadBatch<long>(reader, count);

        void IBatchValueSerializer<long>.Write(BinaryWriter writer, long[] values) => WriteBatch(writer, values);

        ulong IValueSerializer<ulong>.Read(BinaryReader reader) => reader.ReadUInt64();

        void IValueSerializer<ulong>.Write(BinaryWriter writer, ulong value) => writer.Write(value);

        float IValueSerializer<float>.Read(BinaryReader reader) => reader.ReadSingle();

        void IValueSerializer<float>.Write(BinaryWriter writer, float value) => writer.Write(value);

        Color IValueSerializer<Color>.Read(BinaryReader reader) => Color.FromArgb(reader.ReadInt32());

        void IValueSerializer<Color>.Write(BinaryWriter writer, Color value) => writer.Write(value.ToArgb());

        unsafe bool IValueSerializer<bool>.Read(BinaryReader reader) {
            // Preserve the original byte value exactly.
            bool result;
            *(byte*)&result = reader.ReadByte();
            return result;
        }

        unsafe void IValueSerializer<bool>.Write(BinaryWriter writer, bool value) => writer.Write(*(byte*)&value);

        string IValueSerializer<string>.Read(BinaryReader reader) {
            var length = reader.ReadInt32();
            var bytes = reader.ReadBytes(length);
            if (bytes.Length != length) {
                throw new EndOfStreamException();
            }
            return Encoding.Default.GetString(bytes);
        }

        void IValueSerializer<string>.Write(BinaryWriter writer, string value) {
            writer.Write(value.Length);
            writer.Write(Encoding.Default.GetBytes(value));
        }
    }
}
