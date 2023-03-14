namespace EnpassJSONViewer
{
    partial class ItemDetailsForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDetailsForm));
            groupBox1 = new System.Windows.Forms.GroupBox();
            linkLabellURL = new System.Windows.Forms.LinkLabel();
            label2 = new System.Windows.Forms.Label();
            lblUpdatedAt = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            lblID = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lvFields = new System.Windows.Forms.ListView();
            cmsFields = new System.Windows.Forms.ContextMenuStrip(components);
            tsmiFieldCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFieldCopyNameValueToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFieldCopyValueToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            groupBox3 = new System.Windows.Forms.GroupBox();
            lvAttachments = new System.Windows.Forms.ListView();
            cmsAttachments = new System.Windows.Forms.ContextMenuStrip(components);
            tsmiSaveAttachment = new System.Windows.Forms.ToolStripMenuItem();
            imglstAttachments = new System.Windows.Forms.ImageList(components);
            groupBox4 = new System.Windows.Forms.GroupBox();
            rtbNote = new System.Windows.Forms.RichTextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBox2.SuspendLayout();
            cmsFields.SuspendLayout();
            groupBox3.SuspendLayout();
            cmsAttachments.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(linkLabellURL);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(lblUpdatedAt);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(lblID);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(lblSubtitle);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(lblTitle);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(5, 5);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(870, 98);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Basics";
            // 
            // linkLabellURL
            // 
            linkLabellURL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            linkLabellURL.Location = new System.Drawing.Point(71, 69);
            linkLabellURL.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            linkLabellURL.Name = "linkLabellURL";
            linkLabellURL.Size = new System.Drawing.Size(788, 21);
            linkLabellURL.TabIndex = 9;
            linkLabellURL.TabStop = true;
            linkLabellURL.Text = "[URL]";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(29, 69);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(42, 21);
            label2.TabIndex = 8;
            label2.Text = "URL:";
            // 
            // lblUpdatedAt
            // 
            lblUpdatedAt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblUpdatedAt.Location = new System.Drawing.Point(639, 48);
            lblUpdatedAt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUpdatedAt.Name = "lblUpdatedAt";
            lblUpdatedAt.Size = new System.Drawing.Size(220, 21);
            lblUpdatedAt.TabIndex = 7;
            lblUpdatedAt.Text = "[UpdatedAt]";
            lblUpdatedAt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(540, 48);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(91, 21);
            label5.TabIndex = 6;
            label5.Text = "Updated At:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblID
            // 
            lblID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblID.Location = new System.Drawing.Point(639, 26);
            lblID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblID.Name = "lblID";
            lblID.Size = new System.Drawing.Size(220, 21);
            lblID.TabIndex = 5;
            lblID.Text = "[ID]";
            lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(603, 26);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(28, 21);
            label4.TabIndex = 4;
            label4.Text = "ID:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblSubtitle.Location = new System.Drawing.Point(71, 48);
            lblSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(461, 21);
            lblSubtitle.TabIndex = 3;
            lblSubtitle.Text = "[Subtitle]";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 48);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(66, 21);
            label3.TabIndex = 2;
            label3.Text = "Subtitle:";
            // 
            // lblTitle
            // 
            lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblTitle.Location = new System.Drawing.Point(71, 27);
            lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(461, 21);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "[Title]";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(29, 27);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 21);
            label1.TabIndex = 0;
            label1.Text = "Title:";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(5, 103);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox4);
            splitContainer1.Size = new System.Drawing.Size(870, 567);
            splitContainer1.SplitterDistance = 346;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox3);
            splitContainer2.Size = new System.Drawing.Size(870, 346);
            splitContainer2.SplitterDistance = 650;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lvFields);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox2.Size = new System.Drawing.Size(650, 346);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Fields";
            // 
            // lvFields
            // 
            lvFields.AllowColumnReorder = true;
            lvFields.ContextMenuStrip = cmsFields;
            lvFields.Dock = System.Windows.Forms.DockStyle.Fill;
            lvFields.FullRowSelect = true;
            lvFields.GridLines = true;
            lvFields.Location = new System.Drawing.Point(2, 25);
            lvFields.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            lvFields.MultiSelect = false;
            lvFields.Name = "lvFields";
            lvFields.Size = new System.Drawing.Size(646, 318);
            lvFields.TabIndex = 0;
            lvFields.UseCompatibleStateImageBehavior = false;
            lvFields.View = System.Windows.Forms.View.Details;
            lvFields.SelectedIndexChanged += OnFieldsSelectedIndexChanged;
            // 
            // cmsFields
            // 
            cmsFields.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFieldCopyToClipboard, tsmiFieldCopyNameValueToClipboard, tsmiFieldCopyValueToClipboard });
            cmsFields.Name = "cmsFields";
            cmsFields.Size = new System.Drawing.Size(238, 70);
            // 
            // tsmiFieldCopyToClipboard
            // 
            tsmiFieldCopyToClipboard.Name = "tsmiFieldCopyToClipboard";
            tsmiFieldCopyToClipboard.Size = new System.Drawing.Size(237, 22);
            tsmiFieldCopyToClipboard.Text = "Copy to Clipboard";
            // 
            // tsmiFieldCopyNameValueToClipboard
            // 
            tsmiFieldCopyNameValueToClipboard.Name = "tsmiFieldCopyNameValueToClipboard";
            tsmiFieldCopyNameValueToClipboard.Size = new System.Drawing.Size(237, 22);
            tsmiFieldCopyNameValueToClipboard.Text = "Copy name/value to Clipboard";
            // 
            // tsmiFieldCopyValueToClipboard
            // 
            tsmiFieldCopyValueToClipboard.Name = "tsmiFieldCopyValueToClipboard";
            tsmiFieldCopyValueToClipboard.Size = new System.Drawing.Size(237, 22);
            tsmiFieldCopyValueToClipboard.Text = "Copy value to Clipboard";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lvAttachments);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox3.Location = new System.Drawing.Point(0, 0);
            groupBox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox3.Size = new System.Drawing.Size(217, 346);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Attachments";
            // 
            // lvAttachments
            // 
            lvAttachments.AllowColumnReorder = true;
            lvAttachments.ContextMenuStrip = cmsAttachments;
            lvAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            lvAttachments.FullRowSelect = true;
            lvAttachments.GridLines = true;
            lvAttachments.Location = new System.Drawing.Point(2, 25);
            lvAttachments.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            lvAttachments.MultiSelect = false;
            lvAttachments.Name = "lvAttachments";
            lvAttachments.Size = new System.Drawing.Size(213, 318);
            lvAttachments.SmallImageList = imglstAttachments;
            lvAttachments.TabIndex = 1;
            lvAttachments.UseCompatibleStateImageBehavior = false;
            lvAttachments.View = System.Windows.Forms.View.SmallIcon;
            lvAttachments.SelectedIndexChanged += OnAttachmentsSelectedIndexChanged;
            lvAttachments.MouseDoubleClick += OnAttachmentsMouseDoubleClick;
            // 
            // cmsAttachments
            // 
            cmsAttachments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiSaveAttachment });
            cmsAttachments.Name = "cmsAttachments";
            cmsAttachments.Size = new System.Drawing.Size(174, 26);
            // 
            // tsmiSaveAttachment
            // 
            tsmiSaveAttachment.Name = "tsmiSaveAttachment";
            tsmiSaveAttachment.Size = new System.Drawing.Size(173, 22);
            tsmiSaveAttachment.Text = "Save Attachment...";
            // 
            // imglstAttachments
            // 
            imglstAttachments.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imglstAttachments.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imglstAttachments.ImageStream");
            imglstAttachments.TransparentColor = System.Drawing.Color.Transparent;
            imglstAttachments.Images.SetKeyName(0, "BinaryFile.png");
            imglstAttachments.Images.SetKeyName(1, "Image.png");
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(rtbNote);
            groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox4.Location = new System.Drawing.Point(0, 0);
            groupBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            groupBox4.Size = new System.Drawing.Size(870, 218);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Note";
            // 
            // rtbNote
            // 
            rtbNote.Dock = System.Windows.Forms.DockStyle.Fill;
            rtbNote.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rtbNote.Location = new System.Drawing.Point(2, 25);
            rtbNote.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            rtbNote.Name = "rtbNote";
            rtbNote.ReadOnly = true;
            rtbNote.Size = new System.Drawing.Size(866, 190);
            rtbNote.TabIndex = 1;
            rtbNote.Text = "";
            rtbNote.WordWrap = false;
            // 
            // ItemDetailsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(880, 675);
            Controls.Add(splitContainer1);
            Controls.Add(groupBox1);
            Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            MinimizeBox = false;
            Name = "ItemDetailsForm";
            Padding = new System.Windows.Forms.Padding(5);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "[Title]";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            cmsFields.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            cmsAttachments.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblUpdatedAt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabellURL;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox rtbNote;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ImageList imglstAttachments;
        private System.Windows.Forms.ListView lvAttachments;
        private System.Windows.Forms.ContextMenuStrip cmsFields;
        private System.Windows.Forms.ToolStripMenuItem tsmiFieldCopyToClipboard;
        private System.Windows.Forms.ToolStripMenuItem tsmiFieldCopyNameValueToClipboard;
        private System.Windows.Forms.ContextMenuStrip cmsAttachments;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAttachment;
        private System.Windows.Forms.ToolStripMenuItem tsmiFieldCopyValueToClipboard;
    }
}