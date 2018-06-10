using System.Windows.Forms;
using WarBender.UI.Properties;

namespace WarBender.UI {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();
            propertyGrid.SelectedObject = Settings.Default;
        }
    }
}
