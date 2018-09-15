namespace WarBender.UI {
    partial class EntityDropDown {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.treeListView = new BrightIdeasSoftware.TreeListView();
            this.treeListViewNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.textBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.modelImageList = new WarBender.UI.SharedImageLists(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListView
            // 
            this.treeListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.treeListView.AllColumns.Add(this.treeListViewNameColumn);
            this.treeListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeListView.CellEditUseWholeCell = false;
            this.treeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.treeListViewNameColumn});
            this.treeListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView.EmptyListMsg = "No items";
            this.treeListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.treeListView.HeaderUsesThemes = true;
            this.treeListView.HideSelection = false;
            this.treeListView.IsSimpleDragSource = true;
            this.treeListView.Location = new System.Drawing.Point(0, 0);
            this.treeListView.Margin = new System.Windows.Forms.Padding(2);
            this.treeListView.MultiSelect = false;
            this.treeListView.Name = "treeListView";
            this.treeListView.ShowGroups = false;
            this.treeListView.Size = new System.Drawing.Size(257, 261);
            this.treeListView.TabIndex = 6;
            this.treeListView.UseCompatibleStateImageBehavior = false;
            this.treeListView.UseFiltering = true;
            this.treeListView.UseNotifyPropertyChanged = true;
            this.treeListView.View = System.Windows.Forms.View.Details;
            this.treeListView.VirtualMode = true;
            this.treeListView.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.treeListView_CellToolTipShowing);
            this.treeListView.ItemActivate += new System.EventHandler(this.treeListView_ItemActivate);
            // 
            // treeListViewNameColumn
            // 
            this.treeListViewNameColumn.FillsFreeSpace = true;
            this.treeListViewNameColumn.Hideable = false;
            this.treeListViewNameColumn.IsEditable = false;
            this.treeListViewNameColumn.Text = "Name";
            // 
            // searchTimer
            // 
            this.searchTimer.Interval = 500;
            this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textBoxSearch});
            this.statusStrip.Location = new System.Drawing.Point(0, 261);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(257, 23);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Resize += new System.EventHandler(this.statusStrip_Resize);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.AutoSize = false;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(100, 23);
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // EntityDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeListView);
            this.Controls.Add(this.statusStrip);
            this.Name = "EntityDropDown";
            this.Size = new System.Drawing.Size(257, 284);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.TreeListView treeListView;
        private BrightIdeasSoftware.OLVColumn treeListViewNameColumn;
        private System.Windows.Forms.Timer searchTimer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripTextBox textBoxSearch;
        private SharedImageLists modelImageList;
    }
}
