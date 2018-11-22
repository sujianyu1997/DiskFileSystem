﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DiskFileSystem
{
    /*
     * *
     * 这是个单实例类,用来存储各种文件操作函数
     * 
     * */
    class FileFunction
    {
        private static FileFunction instance = new FileFunction();
        private FileFunction() { }
        public static FileFunction GetInstance()
        {
            return instance;
        }

        /*
        * 
        * 该方法用于在Fat表分配给文件空余的磁盘块，并且返回第一个磁盘号.
        */
        public int setFat(int size,int[] fat)
        {
            if (fat[0] < size)
            {
                //MessageBox.Show("没有更多的磁盘空间","磁盘空间分配失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return -1;//不能分配
            }
            int[] startNum = new int[128];
            int i = 4;
            for (int j = 0; j < size; i++)
            {
                if (i > 127)
                {
                    break;//到文件尾巴了
                }
                if (fat[i] == 0)
                {
                    startNum[j] = i; //纪录空闲磁盘块的下标
                    if (j > 0)
                    {
                        fat[startNum[j - 1]] = i; //fat上一磁盘块指向下一磁盘块地址
                    }
                    j++;
                }
            }
            fat[i - 1] = -1;
            return startNum[0]; //返回该文件起始块盘号
        }
        /*
	 * 
	 * 该方法用于删除时释放FAT表的空间
	 */
        public void delFat(int startNum,int[] fat)
        {
            int nextPoint = fat[startNum];
            int nowPoint = startNum;
            int count = 0;
            while (fat[nowPoint] != 0 && fat[nowPoint] != 128)
            {
                nextPoint = fat[nowPoint];
                if (nextPoint == -1)
                {
                    fat[nowPoint] = 0;
                    count++;
                    break;
                }
                else
                {
                    fat[nowPoint] = 0;
                    count++;
                    nowPoint = nextPoint;
                }
            }
            fat[0] += count;
            MessageBox.Show("Fat删除空间成功", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*
	 * 
	 * 以下为追加内容时修改fat表
	 * 
	 */

        public bool reAddFat(int startNum, int addSize,int[] fat)
        {
            int nowPoint = startNum;
            int nextPoint = fat[startNum];
            while (fat[nowPoint] != -1)
            {
                nowPoint = nextPoint;
                nextPoint = fat[nowPoint];
            }//找到该文件终结盘块
            nextPoint = setFat(addSize,fat);
            if (nextPoint != -1)
            {
                fat[nowPoint] = nextPoint;
                return true;
            }
            else
            {
                return false;
            }
        }

        //创建文件夹
        public bool createCatolog(String name,BasicFile fatherFileSet, int[] fat, FileShow fileShow)
        {
            //可以创建
            if (fat[0] >= 1)
            {
                //判断是否重命名
                if (fatherFileSet.childFile.ContainsKey(name))
                {
                    BasicFile value = fatherFileSet.childFile[name];
                    //不同类型，创建成功
                    if (value.getAttr() == 2)
                    {
                        int startNum = this.setFat(1, fat);
                        BasicFile catalog = new BasicFile(name, startNum);
                        //设置父亲
                        //catalog.setFather(fatherFileSet);
                        //添加到父文件夹下
                        fatherFileSet.childFile.Add(catalog.getName(), catalog);
                        //空间减减
                        fat[0]--;
                        //添加到界面
                        fileShow.getFileView().Items.Add(catalog.getItem());
                        Console.WriteLine("文件夹创建成功");
                    }
                    //相同类型，则帮改为默认命名
                    else if (value.getAttr() == 3)
                    {
                        Console.WriteLine("存在重复命名");
                        //以默认命名创建文件夹
                        int i = 1;
                        String newName;
                        do
                        {
                            newName = name + Convert.ToString(i++);
                        } while (fatherFileSet.childFile.ContainsKey(newName));

                        int startNum = this.setFat(1, fat);
                        BasicFile catalog = new BasicFile(newName, startNum);
                        //设置父亲
                        catalog.setFather(fatherFileSet);
                        //添加到父文件夹下
                        fatherFileSet.childFile.Add(catalog.getName(), catalog);
                        //空间减减
                        fat[0]--;
                        //添加到界面
                        fileShow.getFileView().Items.Add(catalog.getItem());
                        Console.WriteLine("文件夹创建成功");
                    }
                }
                //不存在同名的文件夹
                else
                {
                    int startNum = this.setFat(1, fat);
                    BasicFile catalog = new BasicFile(name, startNum);
                    //设置父亲
                    catalog.setFather(fatherFileSet);
                    //添加到父文件夹下
                    fatherFileSet.childFile.Add(catalog.getName(), catalog);
                    //空间减减
                    fat[0]--;
                    //添加到界面
                    fileShow.getFileView().Items.Add(catalog.getItem());
                    Console.WriteLine("文件夹创建成功");
                }
                return true;
            }
            else
            {
                //以false为信号弹出失败窗口
                return false;
            }
        }
        //打开时显示
        public void showFile(BasicFile nowFile,FileShow fileShow)
        {
            if(nowFile.childFile.Count != 0)
            {
                foreach (KeyValuePair<String, BasicFile> file in nowFile.childFile)
                {
                    if (file.Value.getAttr() == 3)
                    { //目录文件
                        Console.WriteLine("文件名 : " + file.Value.getName());
                        Console.WriteLine("操作类型 ： " + "文件夹");
                        Console.WriteLine("起始盘块 ： " + file.Value.getStartNum());
                        Console.WriteLine("大小 : " + file.Value.getSize());
                        Console.WriteLine("<-------------------------------------->");
                    }
                    else if (file.Value.getAttr() == 2)
                    {
                        Console.WriteLine("文件名 : " + file.Value.getName());
                        Console.WriteLine("操作类型 ： " + "文件夹");
                        Console.WriteLine("起始盘块 ： " + file.Value.getStartNum());
                        Console.WriteLine("大小 : " + file.Value.getSize());
                        Console.WriteLine("<-------------------------------------->");
                    }
                }
            }
            //if(nowFile.getAttr() == 2)
            //{
            //    //新开窗口显示文件内容
            //}
            //else if(nowFile.getAttr() == 3)
            //{
            //    //刷新窗口显示文件夹里的文件
            //    //清除原view里的所有东西
            //    //FileShow f = new FileShow();
            //    //f.MdiParent = file;
            //    //SetParent((int)f.Handle, (int)this.Handle);
            //    //添加到fileShow数组里
            //    if (nowFile.childFile.Count == 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        foreach (string key in nowFile.childFile.Keys)
            //        {
            //            //添加到这个数组里就会自动显示文件夹
            //            fileShow.FileListToShow.Add(nowFile.childFile[key]);
            //        }
            //    }
            //    //fileShow.Show();
            //}
        }

        //删除某个父目录下的某个文件
        public void deleteFile(String name,BasicFile fatherFile)
        {
            if (fatherFile.childFile.ContainsKey(name))
            {
                BasicFile file;
                fatherFile.childFile.TryGetValue(name, out file);
                fatherFile.childFile.Remove(name);
                if (file.getAttr() == 3)
                {
                    Console.WriteLine("删除文件夹成功: " + file.getName());
                }
                else if(file.getAttr() == 2)
                {
                    Console.WriteLine("删除文件成功: " + file.getName());
                }
            }
            else
            {
                Console.WriteLine("删除失败，不存在此文件");
            }
           
        }


        /*
         * 
         * 以下为文件或文件夹重命名方法
         * 
         */

        public void reName(String name, String newName,BasicFile fatherFile)
        {
            if (fatherFile.childFile.ContainsKey(name))
            {
                if (fatherFile.childFile.ContainsKey(newName))
                {
                   Console.WriteLine("重命名失败，同名文件已存在！");
                    //showFile();
                }
                else
                {
                    BasicFile file;
                    fatherFile.childFile.TryGetValue(name, out file);
                    file.setName(newName);
                    fatherFile.childFile.Remove(name);
                    fatherFile.childFile.Add(name,file);
                    Console.WriteLine("重命名成功！");
                }
            }
            else
            {
                Console.WriteLine("重命名失败，没有该文件！");
            }
        }


        public void changeType(String name, String type, BasicFile fatherFile)
        {
            if (fatherFile.childFile.ContainsKey(name))
            {
                BasicFile file;
                fatherFile.childFile.TryGetValue(name, out file);
                if(file.getType() == type)
                {
                    Console.WriteLine("改类型失败，相同的类型");
                }
                else
                {
                    file.setType(type);
                    fatherFile.childFile.Remove(name);
                    fatherFile.childFile.Add(name, file);
                    Console.WriteLine("更改类型成功！");
                }
            }
            else
            {
                Console.WriteLine("改类型失败，没有该文件！");
            }
        }

        //打开文件夹时
        public void openFile(String name,BasicFile fatherFile, FileShow fileShow)
        {
            if (fatherFile.childFile.ContainsKey(name))
            {
                BasicFile file; //即将打开的文件
                fatherFile.childFile.TryGetValue(name, out file);
                if (file.getAttr() == 2)
                {
                    //新建文本窗口

                    Console.WriteLine("文件已打开，文件大小为 : " + file.getSize());
                }
                else if (file.getAttr() == 3)
                {
                    //清空fileShow
                    fileShow.getFileView().Items.Clear();
                    //设置FileShow里的father
                    //fileShow.setFather(fatherFile) ;
                    //添加到fileShow的待显示数组里
                    if (file.childFile.Count() != 0)
                    {
                        foreach (KeyValuePair<String, BasicFile> childFile in file.childFile)
                        {
                            fileShow.FileListToShow.Add(childFile);
                        }
                    }
                    
                    //
                    Console.WriteLine("文件夹已打开！");
                }
            }
            else
            {
                Console.WriteLine("打开失败，文件不存在！");
            }
        }

        /*
	 * 
	 * 以下为返回上一层目录
	 * 
	 */

        public void backFile(BasicFile nowFatherFile, FileShow fileShow)
        {
            if (nowFatherFile.getFather() == null)
            {
                Console.WriteLine("已经是最上层目录");
            }
            else
            {
                //打开父文件夹
                openFile(nowFatherFile.getName(), nowFatherFile.getFather(), fileShow);
            }
        }

    }
}