using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using WarBender.CodeGeneration;

namespace WarBender {
    public interface IRecord : IDataObject, INotifyPropertyChanged {
        Type Type { get; }

        int Index { get; }
    }

    public abstract class Record : IRecord, IDataObjectChild {
        static Record() {
            new RecordTypes().Validate();
        }

        [Computed]
        [Browsable(false)]
        public IDataObject Parent { get; private set; }

        [Computed]
        [Browsable(false)]
        public int Index { get; private set; }

        [Computed]
        [Browsable(false)]
        public abstract Type Type { get; }

        private long? _sizeInBytes;

        [Computed]
        [ParenthesizePropertyName(true)]
        public long SizeInBytes {
            get {
                if (_sizeInBytes == null) {
                    using (var stream = new MemoryStream()) {
                        using (var writer = new BinaryWriter(stream, Encoding.Default, true)) {
                            WriteTo(writer);
                        }
                        stream.Flush();
                        _sizeInBytes = stream.Length;
                    }
                }
                return _sizeInBytes.Value;
            }
        }

        IDataObjectChild IDataObjectChild.WithParent(IDataObject parent, int index) {
            Parent = parent;
            Index = index;
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            PropertyChanged?.Invoke(this, e);

            _sizeInBytes = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SizeInBytes)));
        }

        protected void SetProperty<TValue>(string name, ref TValue field, TValue value) {
            if (Equals(field, value)) {
                return;
            }

            if (value is IDataObjectChild child) {
                value = (TValue)child.WithParent(this);
            }
            field = value;

            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        protected void SetProperty<TEntity, TIndex>(string name, ref EntityReference<TEntity, TIndex> field, EntityReference<TEntity, TIndex> value)
            where TEntity : class, IEntity
            where TIndex : struct, IConvertible, IEquatable<TIndex> {

            if (field.Equals(value) && field.Parent != null) {
                return;
            }

            field = new EntityReference<TEntity, TIndex>(this, value);
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        protected void SetProperty(string name, ref float field, float value) {
            // Takes care of preserving positive/negative zero, infinity and NaN.
            if (BitConverter.DoubleToInt64Bits(field) != BitConverter.DoubleToInt64Bits(value)) {
                field = value;
                OnPropertyChanged(new PropertyChangedEventArgs(name));
            }
        }

        protected virtual void ReadFields(BinaryReader reader) =>
            throw new NotImplementedException();

        protected virtual void WriteFields(BinaryWriter writer) =>
            throw new NotImplementedException();

        public virtual void ReadFrom(BinaryReader reader) =>
            ReadFields(reader);

        public virtual void WriteTo(BinaryWriter writer) =>
            WriteFields(writer);
    }

    public abstract class Record<T> : Record
        where T : Record<T> {
        public override Type Type => typeof(T);
    }

    public interface IOptionalRecord : IRecord {
        bool IsPresent { get; }
    }

    public abstract class OptionalRecord<T> : Record<T>, IOptionalRecord
        where T : OptionalRecord<T> {

        [Computed]
        [ParenthesizePropertyName(true)]
        public bool IsPresent { get; set; }

        public override void ReadFrom(BinaryReader reader) {
            IsPresent = reader.ReadInt32() != 0;
            if (IsPresent) {
                base.ReadFrom(reader);
            }
        }

        public override void WriteTo(BinaryWriter writer) {
            writer.Write(IsPresent ? 1 : 0);
            if (IsPresent) {
                base.WriteTo(writer);
            }
        }
    }
}
