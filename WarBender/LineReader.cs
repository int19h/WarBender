using System;
using System.Globalization;
using System.IO;
using static System.Convert;

namespace WarBender {
    internal class LineReader {
        private static readonly char[] fieldSeparators = { ' ' };

        public TextReader Reader { get; }

        public string FileName => Reader.FileName();

        public int CurrentLine { get; private set; }

        public LineReader(TextReader reader) {
            Reader = reader;
        }

        public static implicit operator LineReader(TextReader reader) => new LineReader(reader);

        public string ReadLine() {
            ++CurrentLine;
            return Reader.ReadLine();
        }

        public void SkipLine(int count = 1) {
            for (int i = 0; i < count; ++i) {
                if (ReadLine() == null) {
                    InvalidData("Unexpected end of file");
                }
            }
        }

        public string[] ReadFields(int count, bool allowMore = true) {
            var s = ReadLine();
            if (s == null) {
                InvalidData("Unexpected end of file");
            }

            var fields = s.Split(fieldSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (allowMore) {
                if (fields.Length < count) {
                    InvalidData($"Expected at least {count} fields, but got {fields.Length}");
                }
            } else {
                if (fields.Length != count) {
                    InvalidData($"Expected exactly {count} fields, but got {fields.Length}");
                }
            }

            return fields;
        }

        private T GetValue<T>(string[] fields, int i) {
            var s = fields[i];
            var t = typeof(T);
            if (t.IsEnum) {
                t = Enum.GetUnderlyingType(t);
            }
            try { 
                return (T)ChangeType(s, t, CultureInfo.InvariantCulture);
            } catch (Exception) {
                InvalidData($"Field #{i + 1} must be of type {typeof(T).Name}");
                return default;
            }
        }

        public T ReadFields<T>(bool allowMore = true) {
            var f = ReadFields(1, allowMore);
            return GetValue<T>(f, 0);
        }

        public (T0, T1) ReadFields<T0, T1>(bool allowMore = true) {
            var f = ReadFields(2, allowMore);
            return (GetValue<T0>(f, 0), GetValue<T1>(f, 1));
        }

        public (T0, T1, T2) ReadFields<T0, T1, T2>(bool allowMore = true) {
            var f = ReadFields(3, allowMore);
            return (GetValue<T0>(f, 0), GetValue<T1>(f, 1), GetValue<T2>(f, 2));
        }

        public (T0, T1, T2, T3) ReadFields<T0, T1, T2, T3>(bool allowMore = true) {
            var f = ReadFields(4, allowMore);
            return (GetValue<T0>(f, 0), GetValue<T1>(f, 1), GetValue<T2>(f, 2), GetValue<T3>(f, 3));
        }

        public (T0, T1, T2, T3, T4) ReadFields<T0, T1, T2, T3, T4>(bool allowMore = true) {
            var f = ReadFields(5, allowMore);
            return (GetValue<T0>(f, 0), GetValue<T1>(f, 1), GetValue<T2>(f, 2), GetValue<T3>(f, 3), GetValue<T4>(f, 4));
        }

        public void Expected(string expected) {
            var s = ReadLine().Trim();
            if (s != expected) {
                InvalidData($"Expected '{expected}'");
            }
        }

        public void InvalidData(string message) {
            message += $" in {FileName} at line {CurrentLine}";
            throw new InvalidDataException(message);
        }
    }
}
