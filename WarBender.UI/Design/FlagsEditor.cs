using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WarBender.UI.Design {
    internal class FlagsEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) =>
            UITypeEditorEditStyle.DropDown;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, dynamic value) {
            var listBox = new CheckedListBox {
                CheckOnClick = true,
            };

            var converter = context.PropertyDescriptor.Converter;
            dynamic[] flags = converter.GetStandardValues(context).OfType<Enum>()
                .Where(x => ((IConvertible)x).ToUInt64(null) != 0).ToArray();

            // Happens when editing multiple objects with different values.
            if (value == null) {
                value = converter.ConvertFromInvariantString("0");
            }

            foreach (var flag in flags) {
                var text = converter.ConvertToString(context, CultureInfo.InvariantCulture, (object)flag);
                bool isChecked = (value & flag) == flag;
                listBox.Items.Add(text, isChecked);
            }

            listBox.ItemCheck += (sender, e) => {
                if (e.NewValue == CheckState.Checked) {
                    value |= flags[e.Index];
                } else {
                    value &= ~flags[e.Index];
                }
            };

            var service = ((IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService)));
            service.DropDownControl(listBox);
            return value;
        }
    }
}
