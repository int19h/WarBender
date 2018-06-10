namespace WarBender.UI {
    partial class PropertyGridForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Splitter splitter;
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.listView = new BrightIdeasSoftware.ObjectListView();
            this.columnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.sharedImageLists = new WarBender.UI.SharedImageLists(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showInTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInNewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromThisWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            splitter = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.listView)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter
            // 
            splitter.Dock = System.Windows.Forms.DockStyle.Top;
            splitter.Location = new System.Drawing.Point(0, 23);
            splitter.Margin = new System.Windows.Forms.Padding(2);
            splitter.Name = "splitter";
            splitter.Size = new System.Drawing.Size(453, 2);
            splitter.TabIndex = 2;
            splitter.TabStop = false;
            splitter.SplitterMoving += new System.Windows.Forms.SplitterEventHandler(this.splitter_SplitterMoving);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 25);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(453, 270);
            this.propertyGrid.TabIndex = 0;
            // 
            // listView
            // 
            this.listView.AllColumns.Add(this.columnName);
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.CellEditUseWholeCell = false;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName});
            this.listView.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView.EmptyListMsg = "Drag objects here to inspect them";
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView.IsSimpleDragSource = true;
            this.listView.IsSimpleDropSink = true;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(2);
            this.listView.Name = "listView";
            this.listView.ShowItemCountOnGroups = true;
            this.listView.Size = new System.Drawing.Size(453, 23);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.SmallIcon;
            this.listView.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.listView_CellRightClick);
            this.listView.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.listView_CellToolTipShowing);
            this.listView.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.listView_ModelCanDrop);
            this.listView.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.listView_ModelDropped);
            this.listView.ItemActivate += new System.EventHandler(this.listView_ItemActivate);
            this.listView.SizeChanged += new System.EventHandler(this.listView_SizeChanged);
            // 
            // columnName
            // 
            this.columnName.Groupable = false;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInTreeToolStripMenuItem,
            this.openInNewWindowToolStripMenuItem,
            this.removeFromThisWindowToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(219, 70);
            // 
            // showInTreeToolStripMenuItem
            // 
            this.showInTreeToolStripMenuItem.Name = "showInTreeToolStripMenuItem";
            this.showInTreeToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.showInTreeToolStripMenuItem.Text = "&Show in Tree";
            this.showInTreeToolStripMenuItem.Click += new System.EventHandler(this.showInTreeToolStripMenuItem_Click);
            // 
            // openInNewWindowToolStripMenuItem
            // 
            this.openInNewWindowToolStripMenuItem.Name = "openInNewWindowToolStripMenuItem";
            this.openInNewWindowToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.openInNewWindowToolStripMenuItem.Text = "&Open in New Window";
            this.openInNewWindowToolStripMenuItem.Click += new System.EventHandler(this.openInNewWindowToolStripMenuItem_Click);
            // 
            // removeFromThisWindowToolStripMenuItem
            // 
            this.removeFromThisWindowToolStripMenuItem.Name = "removeFromThisWindowToolStripMenuItem";
            this.removeFromThisWindowToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.removeFromThisWindowToolStripMenuItem.Text = "&Remove from This Window";
            this.removeFromThisWindowToolStripMenuItem.Click += new System.EventHandler(this.removeFromThisWindowToolStripMenuItem_Click);
            // 
            // PropertyGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 295);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(splitter);
            this.Controls.Add(this.listView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PropertyGridForm";
            this.ShowIcon = false;
            this.Text = "Objects";
            ((System.ComponentModel.ISupportInitialize)(this.listView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private BrightIdeasSoftware.ObjectListView listView;
        private BrightIdeasSoftware.OLVColumn columnName;
        private SharedImageLists sharedImageLists;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showInTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInNewWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFromThisWindowToolStripMenuItem;
    }
}