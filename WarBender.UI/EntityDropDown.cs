using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using static WarBender.UI.NativeMethods;

namespace WarBender.UI {
    public partial class EntityDropDown : UserControl {
        private readonly ModelGetters _modelGetters = new ModelGetters();

        public event EventHandler SelectObject;

        public EntityDropDown() {
            InitializeComponent();

            treeListView.SmallImageList = modelImageList.ModelImageList;
            treeListView.EmptyListMsgFont = Font;
            treeListViewNameColumn.AspectGetter = _modelGetters.GetName;
            treeListViewNameColumn.ImageGetter = _modelGetters.GetImage;

            var treeRenderer = treeListView.TreeColumnRenderer;
            treeRenderer.UseGdiTextRendering = true;
            treeRenderer.IsShowLines = false;

            SendMessage(textBoxSearch.Control.Handle, EM_SETCUEBANNER, 0, "Search");
        }

        public IEnumerable Objects {
            get => treeListView.Objects;
            set => treeListView.Objects = value;
        }

        public object SelectedObject {
            get => treeListView.SelectedObject;
            set => treeListView.SelectedObject = value;
        }

        private void searchTimer_Tick(object sender, EventArgs e) {
            searchTimer.Stop();
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text)) {
                treeListView.ModelFilter = null;
                treeListView.DefaultRenderer = null;
            } else {
                var textFilter = TextMatchFilter.Contains(treeListView, textBoxSearch.Text);
                var treeFilter = new TreeFilter(treeListView, textFilter);
                var filter = new CompositeAnyFilter(new List<IModelFilter> { textFilter, treeFilter });
                treeListView.DefaultRenderer = new HighlightTextRenderer();
                treeListView.ModelFilter = filter;
            }
            treeListView.Refresh();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e) {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void statusStrip_Resize(object sender, EventArgs e) {
            textBoxSearch.Width = statusStrip.DisplayRectangle.Width - textBoxSearch.Margin.Horizontal;
        }

        private void treeListView_ItemActivate(object sender, EventArgs e) {
            SelectObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
