namespace EnpassJSONViewer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblCurrentFolder = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnLoadFile = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvCategories = new System.Windows.Forms.TreeView();
            this.imglstCategories = new System.Windows.Forms.ImageList(this.components);
            this.lvItems = new System.Windows.Forms.ListView();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.imglstItems = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tslblCurrentFolder});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabel1.Text = "Folder:";
            // 
            // tslblCurrentFolder
            // 
            this.tslblCurrentFolder.Name = "tslblCurrentFolder";
            this.tslblCurrentFolder.Size = new System.Drawing.Size(118, 17);
            this.tslblCurrentFolder.Text = "toolStripStatusLabel2";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnLoadFile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnLoadFile
            // 
            this.tsbtnLoadFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnLoadFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnLoadFile.Image")));
            this.tsbtnLoadFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnLoadFile.Name = "tsbtnLoadFile";
            this.tsbtnLoadFile.Size = new System.Drawing.Size(23, 22);
            this.tsbtnLoadFile.Text = "toolStripButton1";
            this.tsbtnLoadFile.Click += new System.EventHandler(this.tsbtnLoadFile_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvCategories);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvItems);
            this.splitContainer1.Size = new System.Drawing.Size(800, 403);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvCategories
            // 
            this.tvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCategories.ImageIndex = 0;
            this.tvCategories.ImageList = this.imglstCategories;
            this.tvCategories.Location = new System.Drawing.Point(0, 0);
            this.tvCategories.Name = "tvCategories";
            this.tvCategories.SelectedImageIndex = 0;
            this.tvCategories.Size = new System.Drawing.Size(266, 403);
            this.tvCategories.TabIndex = 0;
            // 
            // imglstCategories
            // 
            this.imglstCategories.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imglstCategories.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstCategories.ImageStream")));
            this.imglstCategories.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstCategories.Images.SetKeyName(0, "FolderClosed.png");
            this.imglstCategories.Images.SetKeyName(1, "FolderOpened.png");
            // 
            // lvItems
            // 
            this.lvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvItems.Location = new System.Drawing.Point(0, 0);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(530, 403);
            this.lvItems.SmallImageList = this.imglstItems;
            this.lvItems.TabIndex = 0;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "openFileDialog1";
            this.dlgOpenFile.Filter = "Enpass JSON File (*.json)|.json";
            this.dlgOpenFile.Title = "Open Enpass JSON File";
            // 
            // imglstItems
            // 
            this.imglstItems.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imglstItems.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstItems.ImageStream")));
            this.imglstItems.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstItems.Images.SetKeyName(0, "KeyVertical.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Enpass JSON Viewer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnLoadFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvCategories;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.ImageList imglstCategories;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tslblCurrentFolder;
        private System.Windows.Forms.ImageList imglstItems;
    }
}