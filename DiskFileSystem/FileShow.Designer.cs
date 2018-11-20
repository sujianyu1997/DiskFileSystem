﻿namespace DiskFileSystem
{
    partial class FileShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileShow));
            this.pathShow = new System.Windows.Forms.TextBox();
            this.searchText = new System.Windows.Forms.TextBox();
            this.fileView = new System.Windows.Forms.ListView();
            this.RightClick_View = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.search = new System.Windows.Forms.Button();
            this.showPath = new System.Windows.Forms.Button();
            this.RightClick_File = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.属性RToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.重命名MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClick_View.SuspendLayout();
            this.RightClick_File.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathShow
            // 
            this.pathShow.Location = new System.Drawing.Point(61, 10);
            this.pathShow.Name = "pathShow";
            this.pathShow.Size = new System.Drawing.Size(534, 21);
            this.pathShow.TabIndex = 0;
            // 
            // searchText
            // 
            this.searchText.Location = new System.Drawing.Point(614, 10);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(119, 21);
            this.searchText.TabIndex = 2;
            // 
            // fileView
            // 
            this.fileView.ContextMenuStrip = this.RightClick_View;
            this.fileView.LargeImageList = this.imageList2;
            this.fileView.Location = new System.Drawing.Point(0, 53);
            this.fileView.Name = "fileView";
            this.fileView.Size = new System.Drawing.Size(802, 400);
            this.fileView.SmallImageList = this.imageList1;
            this.fileView.TabIndex = 4;
            this.fileView.UseCompatibleStateImageBehavior = false;
            this.fileView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.fileView_ItemSelectionChanged);
            this.fileView.SelectedIndexChanged += new System.EventHandler(this.fileView_SelectedIndexChanged);
            this.fileView.Enter += new System.EventHandler(this.fileView_Enter);
            this.fileView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fileView_MouseUp);
            // 
            // RightClick_View
            // 
            this.RightClick_View.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem});
            this.RightClick_View.Name = "RightClick_View";
            this.RightClick_View.Size = new System.Drawing.Size(121, 26);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件夹ToolStripMenuItem,
            this.文件ToolStripMenuItem});
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.新建ToolStripMenuItem.Text = "新建(&W)";
            // 
            // 文件夹ToolStripMenuItem
            // 
            this.文件夹ToolStripMenuItem.Name = "文件夹ToolStripMenuItem";
            this.文件夹ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.文件夹ToolStripMenuItem.Text = "文件夹(&F)";
            this.文件夹ToolStripMenuItem.Click += new System.EventHandler(this.文件夹ToolStripMenuItem_Click);
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.文件ToolStripMenuItem.Text = "文件(&B)";
            this.文件ToolStripMenuItem.Click += new System.EventHandler(this.文件ToolStripMenuItem_Click);
            // 
            // search
            // 
            this.search.BackColor = System.Drawing.SystemColors.Control;
            this.search.BackgroundImage = global::DiskFileSystem.Properties.Resources.search;
            this.search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.search.Location = new System.Drawing.Point(728, 10);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(30, 21);
            this.search.TabIndex = 3;
            this.search.UseVisualStyleBackColor = false;
            // 
            // showPath
            // 
            this.showPath.BackColor = System.Drawing.SystemColors.Control;
            this.showPath.BackgroundImage = global::DiskFileSystem.Properties.Resources.Beat_Hazard_文件夹图标_icon_folder__爱给网_aigei_com;
            this.showPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.showPath.Location = new System.Drawing.Point(34, 10);
            this.showPath.Name = "showPath";
            this.showPath.Size = new System.Drawing.Size(30, 21);
            this.showPath.TabIndex = 1;
            this.showPath.UseVisualStyleBackColor = false;
            // 
            // RightClick_File
            // 
            this.RightClick_File.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开OToolStripMenuItem,
            this.删除DToolStripMenuItem,
            this.重命名MToolStripMenuItem,
            this.属性RToolStripMenuItem1});
            this.RightClick_File.Name = "RightClick";
            this.RightClick_File.Size = new System.Drawing.Size(133, 92);
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.打开OToolStripMenuItem.Text = "打开(&O)";
            this.打开OToolStripMenuItem.Click += new System.EventHandler(this.打开OToolStripMenuItem_Click);
            // 
            // 删除DToolStripMenuItem
            // 
            this.删除DToolStripMenuItem.Name = "删除DToolStripMenuItem";
            this.删除DToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.删除DToolStripMenuItem.Text = "删除(&D)";
            this.删除DToolStripMenuItem.Click += new System.EventHandler(this.删除DToolStripMenuItem_Click);
            // 
            // 属性RToolStripMenuItem1
            // 
            this.属性RToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1});
            this.属性RToolStripMenuItem1.Name = "属性RToolStripMenuItem1";
            this.属性RToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.属性RToolStripMenuItem1.Text = "属性(&R)";
            this.属性RToolStripMenuItem1.Click += new System.EventHandler(this.属性RToolStripMenuItem1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "58-html5版植物大战僵尸源代码-攻击(Attack)_爱给网_aigei_com.gif");
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Beat Hazard-文件夹图标(icon folder)_爱给网_aigei_com.png");
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "只读",
            "读写"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // 重命名MToolStripMenuItem
            // 
            this.重命名MToolStripMenuItem.Name = "重命名MToolStripMenuItem";
            this.重命名MToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.重命名MToolStripMenuItem.Text = "重命名(&M)";
            // 
            // FileShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.search);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.showPath);
            this.Controls.Add(this.pathShow);
            this.Controls.Add(this.fileView);
            this.Name = "FileShow";
            this.Text = "FileShow";
            this.RightClick_View.ResumeLayout(false);
            this.RightClick_File.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pathShow;
        private System.Windows.Forms.Button showPath;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.ListView fileView;
        private System.Windows.Forms.ContextMenuStrip RightClick_View;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip RightClick_File;
        private System.Windows.Forms.ToolStripMenuItem 删除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 属性RToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem 重命名MToolStripMenuItem;
    }
}