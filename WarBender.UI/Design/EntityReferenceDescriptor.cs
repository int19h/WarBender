using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace WarBender.UI.Design {
    internal class EntityReferenceDescriptor : GameTypeDescriptor<IEntityReference, EntityReferenceConverter> {
        public EntityReferenceDescriptor(ICustomTypeDescriptor originalDescriptor)
            : base(originalDescriptor) {
        }

        public override AttributeCollection GetAttributes() {
            var attrs = base.GetAttributes().Cast<Attribute>().ToList();
            attrs.Add(new EditorAttribute(typeof(EntityReferenceEditor), typeof(UITypeEditor)));
            return new AttributeCollection(attrs.ToArray());
        }
    }
}
