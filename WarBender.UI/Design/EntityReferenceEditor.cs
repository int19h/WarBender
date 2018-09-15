using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace WarBender.UI.Design {
    internal class EntityReferenceEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (context?.Instance == null || !(context.PropertyDescriptor.GetValue(context.Instance) is IEntityReference eref)) {
                return value;
            }

            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            var dropDown = new EntityDropDown { Objects = eref.Entities, SelectedObject = eref.Entity };
            dropDown.SelectObject += delegate {
                editorService.CloseDropDown();
            };
            editorService.DropDownControl(dropDown);

            if (dropDown.SelectedObject is IEntity entity) {
                return eref.WithIndex(entity.Index);
            }
            return value;
        }
    }
}
