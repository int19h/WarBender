using System;
using System.Diagnostics;
using System.IO;

namespace WarBender {
    internal unsafe sealed class EnumSerializer<T> : IValueSerializer<T>
        where T : unmanaged, Enum {

        public static readonly EnumSerializer<T> Instance = new EnumSerializer<T>();

        static EnumSerializer() {
            switch (sizeof(T)) {
                case sizeof(byte):
                case sizeof(short):
                case sizeof(int):
                case sizeof(long):
                    break;
                default:
                    throw new NotSupportedException($"sizeof({typeof(T).FullName}) == {sizeof(T)}");
            }
        }

        public T Read(BinaryReader reader) {
            void* p;
            if (sizeof(T) == sizeof(byte)) {  
                var raw = reader.ReadByte();
                p = &raw;
            } else if (sizeof(T) == sizeof(short)) {
                var raw = reader.ReadInt16();
                p = &raw;
            } else if (sizeof(T) == sizeof(int)) {
                var raw = reader.ReadInt32();
                p = &raw;
            } else if (sizeof(T) == sizeof(long)) {
                var raw = reader.ReadInt64();
                p = &raw;
            } else {
                throw new NotSupportedException();
            }
            return *(T*)p;
        }

        public void Write(BinaryWriter writer, T value) {
            void* p = &value;
            if (sizeof(T) == sizeof(byte)) {
                writer.Write(*(byte*)p);
            } else if (sizeof(T) == sizeof(short)) {
                writer.Write(*(short*)p);
            } else if (sizeof(T) == sizeof(int)) {
                writer.Write(*(int*)p);
            } else if (sizeof(T) == sizeof(long)) {
                writer.Write(*(long*)p);
            } else {
                throw new NotSupportedException();
            }
        }
    }

    internal static class EnumSerializer {
        public static IValueSerializer<T> Get<T>() {
            return (IValueSerializer<T>)Activator.CreateInstance(typeof(EnumSerializer<>).MakeGenericType(typeof(T)));
        }
    }
}
