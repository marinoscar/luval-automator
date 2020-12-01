
namespace Luval.Automator.UI
{
    partial class WindowInspect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.topPanel = new System.Windows.Forms.Panel();
            this.cboWindows = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.elementTree = new System.Windows.Forms.TreeView();
            this.splitLeft = new System.Windows.Forms.Splitter();
            this.panelRight = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.DataGridView();
            this.gridBottonPanel = new System.Windows.Forms.Panel();
            this.spltGridBottom = new System.Windows.Forms.Splitter();
            this.elementPicture = new System.Windows.Forms.PictureBox();
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.elementDS = new System.Windows.Forms.BindingSource(this.components);
            this.cmnuImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSaveImageAs = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).BeginInit();
            this.gridBottonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.elementPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementDS)).BeginInit();
            this.cmnuImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.cboWindows);
            this.topPanel.Controls.Add(this.label1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 51);
            this.topPanel.TabIndex = 2;
            // 
            // cboWindows
            // 
            this.cboWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboWindows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWindows.FormattingEnabled = true;
            this.cboWindows.Location = new System.Drawing.Point(69, 12);
            this.cboWindows.Name = "cboWindows";
            this.cboWindows.Size = new System.Drawing.Size(719, 21);
            this.cboWindows.TabIndex = 3;
            this.cboWindows.SelectedIndexChanged += new System.EventHandler(this.cboWindows_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Windows";
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.elementTree);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 51);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(262, 399);
            this.leftPanel.TabIndex = 3;
            // 
            // elementTree
            // 
            this.elementTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementTree.Location = new System.Drawing.Point(0, 0);
            this.elementTree.Name = "elementTree";
            this.elementTree.Size = new System.Drawing.Size(262, 399);
            this.elementTree.TabIndex = 0;
            this.elementTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.elementTree_BeforeExpand);
            this.elementTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.elementTree_NodeMouseClick);
            this.elementTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.elementTree_NodeMouseDoubleClick);
            // 
            // splitLeft
            // 
            this.splitLeft.Location = new System.Drawing.Point(262, 51);
            this.splitLeft.Name = "splitLeft";
            this.splitLeft.Size = new System.Drawing.Size(3, 399);
            this.splitLeft.TabIndex = 4;
            this.splitLeft.TabStop = false;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.spltGridBottom);
            this.panelRight.Controls.Add(this.gridBottonPanel);
            this.panelRight.Controls.Add(this.propertyGrid);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(265, 51);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(535, 399);
            this.panelRight.TabIndex = 5;
            // 
            // propertyGrid
            // 
            this.propertyGrid.AutoGenerateColumns = false;
            this.propertyGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propertyGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedDataGridViewCheckBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.propertyGrid.DataSource = this.elementDS;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(535, 399);
            this.propertyGrid.TabIndex = 0;
            // 
            // gridBottonPanel
            // 
            this.gridBottonPanel.Controls.Add(this.elementPicture);
            this.gridBottonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridBottonPanel.Location = new System.Drawing.Point(0, 338);
            this.gridBottonPanel.Name = "gridBottonPanel";
            this.gridBottonPanel.Size = new System.Drawing.Size(535, 61);
            this.gridBottonPanel.TabIndex = 1;
            // 
            // spltGridBottom
            // 
            this.spltGridBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltGridBottom.Location = new System.Drawing.Point(0, 335);
            this.spltGridBottom.Name = "spltGridBottom";
            this.spltGridBottom.Size = new System.Drawing.Size(535, 3);
            this.spltGridBottom.TabIndex = 2;
            this.spltGridBottom.TabStop = false;
            // 
            // elementPicture
            // 
            this.elementPicture.ContextMenuStrip = this.cmnuImage;
            this.elementPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementPicture.Location = new System.Drawing.Point(0, 0);
            this.elementPicture.Name = "elementPicture";
            this.elementPicture.Size = new System.Drawing.Size(535, 61);
            this.elementPicture.TabIndex = 0;
            this.elementPicture.TabStop = false;
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "Selected";
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // elementDS
            // 
            this.elementDS.DataSource = typeof(Luval.Automator.Core.ElementProperty);
            // 
            // cmnuImage
            // 
            this.cmnuImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveImageAs});
            this.cmnuImage.Name = "cmnuImage";
            this.cmnuImage.Size = new System.Drawing.Size(181, 48);
            // 
            // mnuSaveImageAs
            // 
            this.mnuSaveImageAs.Name = "mnuSaveImageAs";
            this.mnuSaveImageAs.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveImageAs.Text = "Save Image As";
            this.mnuSaveImageAs.Click += new System.EventHandler(this.mnuSaveImageAs_Click);
            // 
            // WindowInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.splitLeft);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "WindowInspect";
            this.Text = "Windows Inspect";
            this.Load += new System.EventHandler(this.WindowInspect_Load);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).EndInit();
            this.gridBottonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.elementPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementDS)).EndInit();
            this.cmnuImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.ComboBox cboWindows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.TreeView elementTree;
        private System.Windows.Forms.Splitter splitLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.DataGridView propertyGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource elementDS;
        private System.Windows.Forms.Panel gridBottonPanel;
        private System.Windows.Forms.Splitter spltGridBottom;
        private System.Windows.Forms.PictureBox elementPicture;
        private System.Windows.Forms.ContextMenuStrip cmnuImage;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveImageAs;
    }
}

