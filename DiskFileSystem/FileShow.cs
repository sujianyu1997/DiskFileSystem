﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DiskFileSystem
{
    public partial class FileShow : Form
    {
        //
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);
      
        //单实例函数
        FileFunction FileFun = FileFunction.GetInstance();
        //
        FileMangerSystem parent;
        //父文件夹,也就是当前目录
        public BasicFile father;
        //复制的文件夹
        public BasicFile[] copyFile_list = null;

        public FileShow(FileMangerSystem form)
        {
            InitializeComponent();
            parent = form;
            father = parent.root;
            upDateTreeView();
        }

        public FileShow()
        {
            InitializeComponent();
        }
        //点击新建文件夹
        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (father.ChildFile.Count >= father.Size * 8)
            {
                FileFun.reAddFat(father, 1, parent.fat);//追加目录磁盘块
            }
            BasicFile file = FileFun.createCatolog(father, parent.fat);
            //Console.WriteLine(father.getName());
            if (file != null)
            {
                fileView.Items.Add(file.Item);
                //树形视图的维护
                upDateTreeView();
                fileView_Activated(this, e);
            }
            else
            {
                MessageBox.Show("创建文件失败", "磁盘空间不足!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //点中文件发生的效果
        private void fileView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //点击删除文件
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //到时候还要获得名字
            for(int i=0;i< fileView.SelectedItems.Count;i++)
            {
                BasicFile clickedFile = getFileByItem(fileView.SelectedItems[i], fileView.View);
                bool flag = FileFun.deleteFile(clickedFile, clickedFile.Father, this.parent.Fat, this.parent.disk_Content);
                if (!flag)
                {
                    MessageBox.Show("删除文件失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.parent.OpenedFileList.Remove(clickedFile.Path);
                    //树形视图的维护
                    upDateTreeView();
                }
            }
            
            fileView_Activated(this, e);
        }
        //点击新建文件
        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (father.ChildFile.Count >= father.Size * 8)
            {
                FileFun.reAddFat(father, 1, parent.fat);//追加目录磁盘块
            }
            BasicFile file = FileFunction.GetInstance().createFile(father, parent.Fat);

            if (file != null)
            {
                fileView.Items.Add(file.Item);
                fileView_Activated(this, e);
            }
            else
            {
                MessageBox.Show("创建文件失败", "磁盘空间不足!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        //窗口被选中（生成，或形成焦点）
        private void fileView_Activated(object sender, EventArgs e)
        {
            //如果文件夹不为空，则显示文件
            if(radioButton2.Checked)
            {
                DetailsUpdate(this, e);
            }
            else
            {
                pathShow.Text = father.Path;
                fileView.View = View.LargeIcon;
                fileView.Items.Clear();
                if (father.ChildFile.Count != 0)
                {
                    foreach (var x in father.ChildFile)
                    {
                        fileView.Items.Add(x.Value.Item);
                    }

                }
            }
        }
        //右键点击事件
        private void fileView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fileView.ContextMenuStrip = null;
                if (fileView.SelectedItems.Count > 0)
                {
                    if(getFileByItem(fileView.SelectedItems[0],fileView.View).Attr == 2)
                    {
                        RightClick_File.Show(fileView, new Point(e.X, e.Y));
                    }
                    else
                    {
                        RightClick_FileSet.Show(fileView, new Point(e.X, e.Y));
                    }
                }
                else
                {
                    RightClick_View.Show(fileView, new Point(e.X, e.Y));
                    if(copyFile_list == null)
                    {
                        for (int i = 0; i < RightClick_View.Items.Count; i++)
                        {
                            if (RightClick_View.Items[i].Text == "粘贴(&V)")
                            {
                                RightClick_View.Items[i].Enabled = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < RightClick_View.Items.Count; i++)
                        {
                            if (RightClick_View.Items[i].Text == "粘贴(&V)")
                            {
                                RightClick_View.Items[i].Enabled = true;
                                break;
                            }
                        }
                    }

                }
            }
        }
        //item选中效果(重命名用到)
        private void fileView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        //双击事件
        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //得到改文件夹，以及该文件夹的父亲
            BasicFile clickedFile = getFileByItem(fileView.SelectedItems[0],fileView.View);
            Form FileFrom = FileFun.openFile(clickedFile, ref father, fileView, parent.fat, parent.openedFileList, parent.disk_Content);
            if(FileFrom != null)
            {
                TXTFrom FileFrom1 = (TXTFrom)FileFrom;

                SetParent((int)FileFrom1.Handle, (int)this.parent.Handle);

                FileFrom1.Show();
                if(clickedFile.Attr == 2)
                {
                    this.parent.OpenedFileList.Add(clickedFile.Path, clickedFile);
                }
                
            }
            if(clickedFile.Attr==3)
            {
               pathShow.Text += @"\" + clickedFile.Name;
               fileView_Activated(this, e);
            }
            
        }

        private BasicFile getFileByItem(ListViewItem item,View view)
        {
           if(view==View.LargeIcon)
            {
                foreach (var childFile in father.ChildFile)
                {
                    if (childFile.Value.Item == item)
                    {
                        return childFile.Value;
                    }
                }
            }
           else if(view==View.Details)
            {
                return FileFun.searchFile(item.SubItems[1].Text,parent.root);//用路径寻找文件
            }
            return null;
        }

        private void FileShow_FormClosed(object sender, EventArgs e)
        {
            this.parent.root.IsOpening = false;
        }

        private void pathShow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar ==(char)Keys.Enter)
            {
                BasicFile value = null;
                if (!pathShow.Text.StartsWith("root:"))//相对路径
                {
                    //MessageBox.Show(father.Name);
                    value = FileFun.searchFile(pathShow.Text, parent.root,father);
                }
                else//绝对路径
                {
                    value = FileFun.searchFile(pathShow.Text, parent.root);
                }
                if(value == null)
                {
                    MessageBox.Show("非法路径", "非法!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if(value.Attr == 2)//是文件则打开,不跳转
                    {
                        Form form = FileFun.openFile(value, ref father, fileView, parent.fat, parent.openedFileList, parent.disk_Content);
                        if (form != null)
                        {
                            SetParent((int)form.Handle, (int)this.parent.Handle);

                            form.Show();
                            this.parent.OpenedFileList.Add(value.Path, value);
                        }
                    }
                    else if(value.Attr == 3)//是目录
                    {
                        father = value;
                    }
                    fileView_Activated(this, e);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fatherpath;
            father = FileFun.backFile(father.Path, parent.root,out fatherpath);
            pathShow.Text = fatherpath;
            fileView_Activated(this, e);
        }

        private void 重命名MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFile clickedFile = getFileByItem(fileView.SelectedItems[0],fileView.View);
            string s = Interaction.InputBox("请输入一个名称", "重命名", clickedFile.Name, -1, -1);
            if(s == "")
            {
                return;
            }
            var regex = new Regex(@"^[^\/\:\*\?\""\<\>\|\,]+$");
            var m = regex.Match(s);
            if (!m.Success)
            {
                MessageBox.Show("请勿在文件名中包含\\ / : * ？ \" < > |等字符，请重新输入有效文件名！");
                return;
            }
            bool flag=FileFun.reName(clickedFile, s, clickedFile.Father);
            if (!flag)
            {
                MessageBox.Show("重命名失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            upDateTreeView();
            fileView_Activated(this, e);

        }

        private void 属性RToolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            //要根据文件里面的type决定
            BasicFile clickedFile = getFileByItem(fileView.SelectedItems[0],fileView.View);
            toolStripComboBox1.Text = clickedFile.Type;
        }

        private void search_Click(object sender, EventArgs e)
        {
            if (searchText.Text.Length == 0)
            {
                return;
            }
            fileView.Clear();
            fileView.View = View.Details;
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "文件名";   //设置列标题

            ch.Width = 250;    //设置列宽度

            ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式

            fileView.Columns.Add(ch);    //将列头添加到ListView控件。

            ColumnHeader ch1 = new ColumnHeader();

            ch1.Text = "路径";   //设置列标题

            ch1.Width = 642;    //设置列宽度

            ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式

            fileView.Columns.Add(ch1);    //将列头添加到ListView控件。
            List<BasicFile> fileArray = new List<BasicFile>();
            FileFun.searchFile(parent.root, searchText.Text, ref fileArray);
            foreach (var x in fileArray)
            {
                ListViewItem lv = new ListViewItem();
                lv.Font = new Font("Tahoma", 26);
                lv.Text = x.Name;
                lv.ImageIndex = 0;
                lv.SubItems.Add(x.Path);
                fileView.Items.Add(lv);
            }
        }

        private void FileShow_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicFile clickedFile = getFileByItem(fileView.SelectedItems[0], fileView.View);
            if(clickedFile.Attr==2)
            {
                Dictionary<string, BasicFile> list = new Dictionary<string, BasicFile>();
                list.Add(clickedFile.Name,clickedFile);
                File_information of = new File_information(list,parent.fat);
                of.Text = "详细信息";
                SetParent((int)of.Handle, (int)this.parent.Handle);
                of.Show();
            }
            else
            {
                File_information of = new File_information(clickedFile.ChildFile,parent.fat);
                SetParent((int)of.Handle, (int)this.parent.Handle);
                of.Text = "详细信息";
                of.Show();
            }
            
        }
        //属性更改时发现的事件
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BasicFile clickedFile = getFileByItem(fileView.SelectedItems[0], fileView.View);
            clickedFile.Type = toolStripComboBox1.Text;
            if(clickedFile.Type == "只读")
            {
                clickedFile.ReadOnly = true;
            }
            else
            {
                clickedFile.ReadOnly = false;
            }
        }
        //树形图的构建
        private void makeTreeViewWithFile(BasicFile file, TreeNode lastNode)
        {
            if(lastNode.Text == "root")
            {
                lastNode.ImageIndex = 0;
                lastNode.SelectedImageIndex = 0;
                //lastNode.ContextMenuStrip = RightClick_View;
                treeView.Nodes.Add(lastNode);
            }

            foreach (var x in file.ChildFile)
            {
                BasicFile cFile = x.Value;
                if(cFile.Attr == 3)
                {
                    //是文件夹则创建节点
                    TreeNode node = new TreeNode(cFile.Name);
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    //lastNode.ContextMenuStrip = RightClick_View;
                    //添加到上一层
                    lastNode.Nodes.Add(node);
                    makeTreeViewWithFile(cFile, node);
                }
            }
            treeView.ExpandAll();
        }
        //树形图的维护
        private void upDateTreeView()
        {
            treeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode(parent.root.Name);
            makeTreeViewWithFile(this.parent.root, rootNode);
        }
        //点击就可以进入该文件夹
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String path = "";
            TreeNode node = e.Node;
            while(node.Text != "root")
            {
                path = node.Text + @"\" + path;
                node = node.Parent;
            }
            path = @"root:\" + path;
            //跳转
            BasicFile value = FileFun.searchFile(path, this.parent.root);
            if (value == null)
            {
                MessageBox.Show("非法路径", "非法!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if(value.Attr == 3)//是目录
            {
                father = value;
            }
            fileView_Activated(this, e);

            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
                RightClick_Tree.Show(treeView.PointToScreen(new Point(e.X,e.Y)));
                if (copyFile_list == null)
                {
                    for (int i = 0; i < RightClick_Tree.Items.Count; i++)
                    {
                        if (RightClick_Tree.Items[i].Text == "粘贴(&V)")
                        {
                            RightClick_Tree.Items[i].Enabled = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < RightClick_Tree.Items.Count; i++)
                    {
                        if (RightClick_Tree.Items[i].Text == "粘贴(&V)")
                        {
                            RightClick_Tree.Items[i].Enabled = true;
                            break;
                        }
                    }
                }
            }
        }

        private void 删除DToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            String path = "";
            TreeNode node = treeView.SelectedNode;
            if (node.Text == "root")
            {
                MessageBox.Show("根目录无法删除", "违规操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
                
            while (node.Text != "root")
            {
                node = node.Parent;
                path = node.Text + @"\" + path;
            }
            path = path.Replace("root",@"root:");
            //跳转到父亲
            BasicFile value = FileFun.searchFile(path, this.parent.root);
            
            if (value == null)
            {
                MessageBox.Show("非法路径", "非法!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (value.Attr == 3)//是目录
            {
                father = value;
            }

            //到时候还要获得名字
            BasicFile clickedFile = getFileByName(treeView.SelectedNode.Text);
            bool flag = FileFun.deleteFile(clickedFile, clickedFile.Father, this.parent.Fat,this.parent.disk_Content);
            if (!flag)
            {
                MessageBox.Show("删除文件失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.parent.OpenedFileList.Remove(clickedFile.Path);
                //树形视图的维护
                upDateTreeView();
            }
            fileView_Activated(this, e);
        }

        private BasicFile getFileByName(String name)
        {
            foreach(var x in father.ChildFile)
            {
                if(x.Value.Name == name)
                {
                    return x.Value;
                }
            }
            return null;
        }

        private void DetailsUpdate(object sender, EventArgs e)
        {
            fileView.Items.Clear();
            fileView.View = View.Details;
            //ColumnHeader ch = new ColumnHeader();
            //ch.Text = "文件名";   //设置列标题

            //ch.Width = 250;    //设置列宽度

            //ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式

            //fileView.Columns.Add(ch);    //将列头添加到ListView控件。


            ////设置路径列头
            //ColumnHeader ch1 = new ColumnHeader();

            //ch1.Text = "路径";   //设置列标题

            //ch1.Width = 500;    //设置列宽度

            //ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式

            //fileView.Columns.Add(ch1);    //将列头添加到ListView控件。

            ////设置大小列头
            //ColumnHeader ch2 = new ColumnHeader();

            //ch2.Text = "大小";   //设置列标题

            //ch2.Width = 50;    //设置列宽度

            //ch2.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式
            //fileView.Columns.Add(ch2);

            ////设置类型列头
            //ColumnHeader ch3 = new ColumnHeader();

            //ch3.Text = "类型";   //设置列标题

            //ch3.Width = 100;    //设置列宽度

            //ch3.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式
            //fileView.Columns.Add(ch3);


            
            
            foreach (var x in father.ChildFile.Values)
            {
                ListViewItem lv = new ListViewItem();
                lv.Font = new Font("Tahoma", 20);
                lv.Text = x.Name;
                lv.ImageIndex = 0;
                lv.SubItems.Add(x.Path);
                lv.SubItems.Add(x.Size.ToString());
                lv.SubItems.Add(x.Attr==2?"文件":"目录");
                fileView.Items.Add(lv);
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileView.SelectedItems.Count > 0)
            {
                copyFile_list = new BasicFile[fileView.SelectedItems.Count];

                for (int i = 0; i < fileView.SelectedItems.Count; i++)
                {
                    copyFile_list[i] = new BasicFile(getFileByItem(fileView.SelectedItems[i], fileView.View));
                }
                
            }
        }

        private void 粘贴VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //确保父目录在第一个被复制
            for (int i = 0; i < copyFile_list.Count(); i++)
            {
                if(copyFile_list[i] == father)
                {
                    BasicFile temp = copyFile_list[0];
                    copyFile_list[0] = copyFile_list[i];
                    copyFile_list[i] = temp;
                    break;
                }
            }

            for (int i = 0; i < copyFile_list.Count(); i++) 
            {
                BasicFile copyFile = copyFile_list[i];

                if (copyFile.Attr == 2)
                {
                    if (father.ChildFile.Count >= father.Size * 8)
                    {
                        FileFun.reAddFat(father, 1, parent.fat);//追加目录磁盘块
                    }
                    BasicFile file = FileFunction.GetInstance().createFile_(father, parent.Fat, copyFile.Name, copyFile.Type, copyFile.Size, copyFile.Suffix, copyFile.Content);

                    if (file != null)
                    {
                        fileView.Items.Add(file.Item);
                        fileView_Activated(this, e);

                        FileFun.setDiskContent(parent.disk_Content, file, parent.fat);
                    }
                    else
                    {
                        MessageBox.Show("复制文件失败", "磁盘空间不足!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (copyFile.Attr == 3)
                {
                    if (parent.fat[0] < countDisk(copyFile))
                    {
                        MessageBox.Show("复制文件失败", "磁盘空间不足!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //创建第一层
                    if (father.ChildFile.Count >= father.Size * 8)
                    {
                        FileFun.reAddFat(father, 1, parent.fat);//追加目录磁盘块
                    }
                    //第一层文件夹
                    BasicFile file = FileFun.createCatolog_(father, parent.fat, copyFile, copyFile.Name);
                    if (file != null)
                    {
                        fileView.Items.Add(file.Item);

                        foreach (var a in copyFile.ChildFile)
                        {
                            if (a.Value == file)
                            {
                                continue;
                            }
                            //递归创建文件
                            copyCatolog(a.Value, file);
                        }
                    }
                    //树形视图的维护
                    upDateTreeView();
                    fileView_Activated(this, e);
                }
            }
            
           
        }
        //递归复制
        private void copyCatolog(BasicFile copy_file, BasicFile file_father)
        {
            if(copy_file.Attr == 2)
            {
                //分配空间
                if (file_father.ChildFile.Count >= file_father.Size * 8)
                {
                    FileFun.reAddFat(file_father, 1, parent.fat);//追加目录磁盘块
                }
                BasicFile file = FileFunction.GetInstance().createFile_(file_father, parent.Fat, copy_file.Name, copy_file.Type, copy_file.Size, copy_file.Suffix, copy_file.Content);
                //创建成功
                if (file != null)
                {
                    FileFun.setDiskContent(parent.disk_Content, file, parent.fat);
                }
                else
                {
                    MessageBox.Show("复制文件失败", "磁盘空间不足!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            //文件夹
            else if(copy_file.Attr == 3)
            {
                if (file_father.ChildFile.Count >= file_father.Size * 8)
                {
                    FileFun.reAddFat(file_father, 1, parent.fat);//追加目录磁盘块
                }
                //创建1层文件夹
                BasicFile file = FileFun.createCatolog_(file_father, parent.fat, file_father, copy_file.Name);

                if (file != null)
                {
                    foreach (var a in copy_file.ChildFile)
                    {
                        //递归创建文件
                        copyCatolog(a.Value, file);
                    }
                }
            }
        }

        private int countDisk(BasicFile file)
        {
            int SUM = file.Size;
            foreach(var a in file.ChildFile)
            {
                 SUM += countDisk(a.Value);
            }
            return SUM; 
        }
    }
}
