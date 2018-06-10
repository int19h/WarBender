using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace WarBender.UI.Design {
    internal class RecordEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) =>
            UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (value is IRecord record) {
                MainForm.Instance.ShowPropertyGrid(new[] { record });
            }
            return value;
        }
    }
}
