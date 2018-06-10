using System;

namespace WarBender {
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ConditionalOnAttribute : Attribute {
        public readonly string MemberName;

        public ConditionalOnAttribute(string memberName) {
            MemberName = memberName;
        }
    }
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class GameVersionAttribute : Attribute {
        public readonly int MinVersion;
        public readonly int MaxVersion;

        public GameVersionAttribute(int minVersion = 0, int maxVersion = int.MaxValue) {
            MinVersion = minVersion;
            MaxVersion = maxVersion;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BitFieldAttribute : Attribute {
        public readonly uint Width;

        public BitFieldAttribute(uint width) {
            Width = width;
        }
    }
}
