using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCDataInfo : UserControl
    {
        private IContainer components = null;

        private ListView listView1;

        private ColumnHeader columnHeader1;

        private ColumnHeader columnHeader2;

        public UCDataInfo()
        {
            this.InitializeComponent();
        }

        public void SelectObject(NFileInfo info)
        {
            this.listView1.Items.Clear();
            bool flag = info != null;
            if (flag)
            {
                this.AddItem("编号", info.FileNO);
                this.AddItem("文件", info.FileName);
                this.AddItem("创建时间", info.GetCreateTime().ToViewTime());
            }
        }

        public void SelectObject(RadTreeNode info)
        {
            bool flag = info == null;
            if (flag)
            {
                this.listView1.Items.Clear();
            }
            else
            {
                NBatchInfo nBatchInfo = info.Tag as NBatchInfo;
                bool flag2 = nBatchInfo != null;
                if (flag2)
                {
                    this.SelectObject(nBatchInfo);
                }
                else
                {
                    NFileInfo nFileInfo = info.Tag as NFileInfo;
                    bool flag3 = nFileInfo != null;
                    if (flag3)
                    {
                        this.SelectObject(nFileInfo);
                    }
                    else
                    {
                        this.SelectObject(nFileInfo);
                    }
                }
            }
        }

        public void AddItem(string key, string value)
        {
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = key;
            listViewItem.SubItems.Add(value);
            this.listView1.Items.Add(listViewItem);
        }

        public void SelectObject(NBatchInfo info)
        {
            this.listView1.Items.Clear();
            bool flag = info != null;
            if (flag)
            {
                this.AddItem("编号", info.BatchNO);
                this.AddItem("条形码", info.BarCode);
                this.AddItem("操作员", info.TellerNO);
                this.AddItem("提交时间", info.CreateTime.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listView1 = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            base.SuspendLayout();
            this.listView1.Columns.AddRange(new ColumnHeader[]
            {
                this.columnHeader1,
                this.columnHeader2
            });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(546, 160);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.listView1.Visible = true;
            this.columnHeader1.Text = "项";
            this.columnHeader1.Width = 294;
            this.columnHeader2.Text = "值";
            this.columnHeader2.Width = 420;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.listView1);
            base.Name = "UCDataInfo";
            base.Size = new Size(546, 160);
            base.ResumeLayout(false);
        }
    }
}
