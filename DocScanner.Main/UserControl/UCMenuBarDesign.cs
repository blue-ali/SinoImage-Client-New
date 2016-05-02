using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCMenuBarDesign : UserControl
    {
        private GroupItems groupitems;

        private IContainer components = null;

        private TableLayoutPanel tableLayoutPanel1;

        private TableLayoutPanel tableLayoutPanel2;

        private Panel panel1;

        private RadTreeView radTreeView1;

        private PropertyGrid propertyGrid1;

        private PictureBox pictureBox1;

        private Panel panel2;

        private Button btn_Save;

        private Button btn_OpenFile;

        public string Title
        {
            get
            {
                return "工具栏配置";
            }
        }

        public UCMenuBarDesign()
        {
            this.InitializeComponent();
            this.radTreeView1.SelectedNodeChanged += new RadTreeView.RadTreeViewEventHandler(this.radTreeView1_SelectedNodeChanged);
            this.radTreeView1.ItemHeight = 40;
            this.groupitems = GroupItems.UnSerializeFromXML(SystemHelper.GetAssemblesDirectory() + "toolitems.xml");
            this.radTreeView1.Visible = true;
            RadTreeNode radTreeNode = this.radTreeView1.Nodes.Add("顶部工具栏");
            radTreeNode.Tag = this.groupitems.TopBarItems;
            foreach (GroupItem current in this.groupitems.TopBarItems)
            {
                RadTreeNode radTreeNode2 = radTreeNode.Nodes.Add(current.name);
                radTreeNode2.Tag = current;
                foreach (ToolItem current2 in current.ToolItems)
                {
                    RadTreeNode radTreeNode3 = radTreeNode2.Nodes.Add(current2.name);
                    radTreeNode3.Tag = current2;
                }
            }
            RadTreeNode radTreeNode4 = this.radTreeView1.Nodes.Add("右边工具栏");
            foreach (GroupItem current3 in this.groupitems.RightPaneItems)
            {
                RadTreeNode radTreeNode5 = radTreeNode4.Nodes.Add(current3.name);
                radTreeNode5.Tag = current3;
                foreach (ToolItem current4 in current3.ToolItems)
                {
                    RadTreeNode radTreeNode6 = radTreeNode5.Nodes.Add(current4.name);
                    radTreeNode6.Tag = current4;
                }
            }
        }

        public static void ShowSetting(object param = null)
        {
            FormContainer formContainer = new FormContainer();
            formContainer.SetControl(new UCMenuBarDesign());
            formContainer.ShowDialog();
        }

        private void radTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            this.propertyGrid1.SelectedObject = e.Node.Tag;
            bool flag = e.Node.Tag is ToolItem;
            if (flag)
            {
                ToolItem toolItem = e.Node.Tag as ToolItem;
                string text = SystemHelper.ResourceDir + toolItem.image;
                bool flag2 = File.Exists(text);
                if (flag2)
                {
                    this.pictureBox1.Image = ImageHelper.LoadSizedImage(text, 128, 128, "");
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            this.groupitems.SerializeToXML(SystemHelper.GetAssemblesDirectory() + "toolitems.xml");
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            string arguments = SystemHelper.GetAssemblesDirectory() + "toolitems.xml";
            Process.Start("notepad.exe", arguments);
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
            this.components = new Container();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.propertyGrid1 = new PropertyGrid();
            this.pictureBox1 = new PictureBox();
            this.panel2 = new Panel();
            this.btn_OpenFile = new Button();
            this.btn_Save = new Button();
            this.panel1 = new Panel();
            this.radTreeView1 = new RadTreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)this.radTreeView1).BeginInit();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.06061f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.93939f));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel1.Size = new Size(775, 714);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel2.Controls.Add(this.propertyGrid1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new Point(329, 4);
            this.tableLayoutPanel2.Margin = new Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 66.55405f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.44595f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 104f));
            this.tableLayoutPanel2.Size = new Size(442, 706);
            this.tableLayoutPanel2.TabIndex = 0;
            this.propertyGrid1.CategoryForeColor = SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = DockStyle.Fill;
            this.propertyGrid1.Location = new Point(5, 5);
            this.propertyGrid1.Margin = new Padding(4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(432, 389);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.Location = new Point(5, 403);
            this.pictureBox1.Margin = new Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(432, 192);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.panel2.Controls.Add(this.btn_OpenFile);
            this.panel2.Controls.Add(this.btn_Save);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(4, 603);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(434, 99);
            this.panel2.TabIndex = 2;
            this.btn_OpenFile.Location = new Point(74, 21);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new Size(123, 65);
            this.btn_OpenFile.TabIndex = 1;
            this.btn_OpenFile.Text = "打开配置文件";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new EventHandler(this.btn_OpenFile_Click);
            this.btn_Save.Location = new Point(247, 21);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new Size(123, 65);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new EventHandler(this.btn_Save_Click);
            this.panel1.Controls.Add(this.radTreeView1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(4, 4);
            this.panel1.Margin = new Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(317, 706);
            this.panel1.TabIndex = 1;
            this.radTreeView1.BackColor = SystemColors.ControlLightLight;
            this.radTreeView1.Dock = DockStyle.Fill;
            this.radTreeView1.Location = new Point(0, 0);
            this.radTreeView1.Margin = new Padding(4);
            this.radTreeView1.Name = "RadTreeView";
            this.radTreeView1.RootElement.ControlBounds = new Rectangle(0, 0, 150, 250);
            this.radTreeView1.Size = new Size(317, 706);
            this.radTreeView1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Margin = new Padding(3, 2, 3, 2);
            base.Name = "UCMenuBarDesign";
            base.Size = new Size(775, 714);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize)this.radTreeView1).EndInit();
            base.ResumeLayout(false);
        }
    }
}
