using System;
using System.IO;

namespace WarBender
{
    internal sealed class NullableSerializer<T> : IValueSerializer<T?> 
        where T : struct { 

        public static readonly NullableSerializer<T> Instance = new NullableSerializer<T>();

        private static readonly IValueSerializer<T> _serializer = ValueSerializer.Get<T>();

        public T? Read(BinaryReader reader) => _serializer.Read(reader);

        public void Write(BinaryWriter writer, T? value) {
            if (value is T x) {
                _serializer.Write(writer, x);
            } else {
                throw new InvalidDataException();
            }
        }
    }

    internal static class NullableSerializer {
        public static IValueSerializer<T> Get<T>()
            where T : struct
            => (IValueSerializer<T>)Get(typeof(T));

        public static object Get(Type type) =>
            Activator.CreateInstance(typeof(NullableSerializer<>).MakeGenericType(type));
    }
}
