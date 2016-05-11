using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Main.Setting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCSummary : UserControl//, IHasIPropertiesSetting
    {

        private RadTreeNode _selectedNode;

        private IContainer components = null;

        private Label lbFileCount;

        private Label lbCount;

        private RadioButton rbAfixx;

        private RadioButton rbMaster;

        private PropertyGrid propertyGrid1;

        private Button btnReflash;

        private readonly static UCSummary instance = new UCSummary();

        public string Title
        {
            get
            {
                return "内容";
            }
        }

        private UCSummary()
        {
            this.InitializeComponent();
            this.propertyGrid1.Visible = SummaryPropertiesSetting.GetInstance().ProperGridVisialbe;
            this.propertyGrid1.Enabled = AbstractSetting<FunctionSetting>.CurSetting.AllowRightPanePropertyGrid;
            this.btnReflash.Click += new EventHandler(this.Button1_Click);
        }

        public static UCSummary GetInstance()
        {
            return instance;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.propertyGrid1.Refresh();
        }

        public void ShowNodeInfo(RadTreeNode node, bool InvockUI)
        {
            this._selectedNode = node;
            if (this._selectedNode != null)
            {
                this._selectedNode.UpdateNodeNInfo();
                this.propertyGrid1.SelectedObject = this._selectedNode.Tag;
            }
            else
            {
                this.propertyGrid1.SelectedObject = null;
            }
            if (InvockUI)
            {
                (base.Parent.Parent as TabControl).SelectedTab = (base.Parent as TabPage);
            }
            bool flag2 = this._selectedNode != null;
            if (flag2)
            {
                bool flag3 = this._selectedNode.Tag is NFileInfo;
                if (flag3)
                {
                    NFileInfo nFileInfo = this._selectedNode.Tag as NFileInfo;
                    this.lbCount.Text = "批注数目";
                    this.lbFileCount.Text = nFileInfo.NotesList.Count.ToString();
                    this.rbAfixx.Visible = false;
                    this.rbMaster.Visible = false;
                }
                else
                {
                    bool flag4 = this._selectedNode.Tag is NBatchInfo;
                    if (flag4)
                    {
                        this.lbCount.Text = "附件数目";
                        int num = 0;
                        foreach (RadTreeNode current in this._selectedNode.Nodes)
                        {
                            NFileInfo nFileInfo2 = current.Tag as NFileInfo;
                            bool flag5 = nFileInfo2 != null;
                            if (flag5)
                            {
                                num++;
                            }
                        }
                        this.lbFileCount.Text = this._selectedNode.Nodes.Count.ToString() + "个,(主件" + num.ToString() + ")";
                        this.rbAfixx.Visible = false;
                        this.rbMaster.Visible = false;
                    }
                    bool flag6 = this._selectedNode.Tag is NCategoryInfo;
                    if (flag6)
                    {
                    }
                }
            }
        }

        //IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        //{
        //    return this.GetSetting();
        //}

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
            this.lbFileCount = new Label();
            this.lbCount = new Label();
            this.rbAfixx = new RadioButton();
            this.rbMaster = new RadioButton();
            this.propertyGrid1 = new PropertyGrid();
            this.btnReflash = new Button();
            base.SuspendLayout();
            this.lbFileCount.AutoSize = true;
            this.lbFileCount.Font = new Font("SimSun", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.lbFileCount.Location = new Point(132, 144);
            this.lbFileCount.Margin = new Padding(4, 0, 4, 0);
            this.lbFileCount.Name = "lbFileCount";
            this.lbFileCount.Size = new Size(41, 20);
            this.lbFileCount.TabIndex = 28;
            this.lbFileCount.Text = "0张";
            this.lbFileCount.Visible = false;
            this.lbCount.AutoSize = true;
            this.lbCount.Location = new Point(49, 146);
            this.lbCount.Margin = new Padding(4, 0, 4, 0);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new Size(67, 15);
            this.lbCount.TabIndex = 27;
            this.lbCount.Text = "附件数量";
            this.lbCount.Visible = false;
            this.rbAfixx.AutoSize = true;
            this.rbAfixx.Location = new Point(136, 97);
            this.rbAfixx.Margin = new Padding(4);
            this.rbAfixx.Name = "rbAfixx";
            this.rbAfixx.Size = new Size(58, 19);
            this.rbAfixx.TabIndex = 24;
            this.rbAfixx.Text = "附件";
            this.rbAfixx.UseVisualStyleBackColor = true;
            this.rbAfixx.Visible = false;
            this.rbMaster.AutoSize = true;
            this.rbMaster.Checked = true;
            this.rbMaster.Location = new Point(52, 97);
            this.rbMaster.Margin = new Padding(4);
            this.rbMaster.Name = "rbMaster";
            this.rbMaster.Size = new Size(58, 19);
            this.rbMaster.TabIndex = 23;
            this.rbMaster.TabStop = true;
            this.rbMaster.Text = "主件";
            this.rbMaster.UseVisualStyleBackColor = true;
            this.rbMaster.Visible = false;
            this.propertyGrid1.CategoryForeColor = SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = DockStyle.Fill;
            this.propertyGrid1.Location = new Point(0, 0);
            this.propertyGrid1.Margin = new Padding(4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(359, 825);
            this.propertyGrid1.TabIndex = 29;
            this.propertyGrid1.Visible = false;
            this.btnReflash.Dock = DockStyle.Bottom;
            this.btnReflash.Location = new Point(0, 784);
            this.btnReflash.Name = "button1";
            this.btnReflash.Size = new Size(359, 41);
            this.btnReflash.TabIndex = 30;
            this.btnReflash.Text = "刷新";
            this.btnReflash.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnReflash);
            base.Controls.Add(this.propertyGrid1);
            base.Controls.Add(this.lbFileCount);
            base.Controls.Add(this.lbCount);
            base.Controls.Add(this.rbAfixx);
            base.Controls.Add(this.rbMaster);
            base.Margin = new Padding(4);
            base.Name = "UCSummary";
            base.Size = new Size(359, 825);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
