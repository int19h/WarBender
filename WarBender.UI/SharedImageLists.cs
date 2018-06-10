using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WarBender.UI {
    public partial class SharedImageLists : Component {
        public ImageList ModelImageList => modelImageList;

        public SharedImageLists() {
            InitializeComponent();
            CreateFolderIcons();
        }

        public SharedImageLists(IContainer container) {
            container.Add(this);
            InitializeComponent();
            CreateFolderIcons();
        }

        private void CreateFolderIcons() {
            var folder = modelImageList.Images["Folder"];
            var rect = new Rectangle(5, 4, 9, 9);
            var folderIcons = modelImageList.Images.Keys.Cast<string>().Select(key => {
                var img = modelImageList.Images[key];
                var bmp = new Bitmap(folder);
                using (var g = Graphics.FromImage(bmp)) {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, rect);
                }
                return (key + "Folder", bmp);
            });

            foreach (var (key, img) in folderIcons) {
                modelImageList.Images.Add(key, img);
            }
        }
    }
}
