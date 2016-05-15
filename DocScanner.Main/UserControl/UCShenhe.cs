using DocScanner.Bean;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCShenhe : UserControl
    {
        private NBatchInfo _curbatchinfo;

        private IContainer components = null;

        private ListView listView1;

        private ColumnHeader columnHeaderItem;

        private ColumnHeader columnHeaderResult;

        private ColumnHeader columnHeaderRemark;

        public string Title
        {
            get
            {
                return "审核信息";
            }
        }

        public NBatchInfo CurBatch
        {
            get
            {
                return this._curbatchinfo;
            }
            set
            {
                bool flag = this._curbatchinfo != value;
                if (flag)
                {
                    this._curbatchinfo = value;
                }
            }
        }

        public UCShenhe()
        {
            this.InitializeComponent();
            this.listView1.DoubleClick += new EventHandler(this.ListView1_DoubleClick);
            this.listView1.ForeColor = Color.Red;
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            bool flag = this.listView1.SelectedItems.Count > 0;
            if (flag)
            {
                BatchTemplateDef BatchTemplateDef = this.listView1.SelectedItems[0].Tag as BatchTemplateDef;
                bool flag2 = BatchTemplateDef != null;
                if (flag2)
                {
                    LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("", null);
                }
            }
        }

        private void SetupShenHeList(NBatchInfo data)
        {
            this.listView1.Items.Clear();
            bool flag = data == null;
            if (!flag)
            {
                bool flag2 = data.ExShenheResult != 0;
                if (flag2)
                {
                    ListViewItem listViewItem = this.listView1.Items.Add(data.BatchNO);
                    listViewItem.SubItems.Add(data.ExShenheResult.ToString());
                    listViewItem.SubItems.Add(data.ExShenheRemark);
                    listViewItem.Tag = data;
                }
                foreach (NFileInfo current in data.FileInfos)
                {
                    bool flag3 = data.ExShenheResult != 0;
                    if (flag3)
                    {
                        ListViewItem listViewItem2 = this.listView1.Items.Add(current.FileName);
                        listViewItem2.SubItems.Add(current.ExShenheResult.ToString());
                        listViewItem2.SubItems.Add(current.ExShenheRemark);
                    }
                }
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
            this.columnHeaderItem = new ColumnHeader();
            this.columnHeaderResult = new ColumnHeader();
            this.columnHeaderRemark = new ColumnHeader();
            base.SuspendLayout();
            this.listView1.Columns.AddRange(new ColumnHeader[]
            {
                this.columnHeaderItem,
                this.columnHeaderResult,
                this.columnHeaderRemark
            });
            this.listView1.Dock = DockStyle.Top;
            this.listView1.Location = new Point(0, 0);
            this.listView1.Margin = new Padding(4, 4, 4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(302, 405);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.columnHeaderItem.Text = "条目";
            this.columnHeaderItem.Width = 88;
            this.columnHeaderResult.Text = "意见";
            this.columnHeaderResult.Width = 76;
            this.columnHeaderRemark.Text = "备注";
            this.columnHeaderRemark.Width = 126;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.listView1);
            base.Margin = new Padding(3, 2, 3, 2);
            base.Name = "UCShenhe";
            base.Size = new Size(302, 838);
            base.ResumeLayout(false);
        }
    }
}
