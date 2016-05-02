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

namespace DocScanner.Main
{
    public class UCCenterView : UserControl
    {
        private NFileInfo _curfileinfo;

        private IUCView _curview;

        private IContainer components = null;

        private Panel panel1;

        private UCWorkPane workpane;

        private Button btnCloseWorkPane;

        private Splitter splitter1;

        public bool ShowedWorkPane
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("UISetting", "UIShowWorkPane").ToBool();
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("UISetting", "UIShowWorkPane", value.ToString());
            }
        }

        public NFileInfo CurFileInfo
        {
            get
            {
                return this._curfileinfo;
            }
            set
            {
                bool flag = this._curfileinfo != value;
                if (flag)
                {
                    this._curfileinfo = value;
                    string ext = (this._curfileinfo == null) ? null : this._curfileinfo.LocalPath;
                    IUCView customView = ExtAssociation.GetCustomView(ext);
                    bool flag2 = customView != null;
                    if (flag2)
                    {
                        customView.CurFileInfo = value;
                    }
                    this.SelectCustomView(customView);
                    this.workpane.CurFileInfo = value;
                }
            }
        }

        public IUCView Realview
        {
            get
            {
                return this._curview;
            }
            set
            {
                this._curview = value;
            }
        }

        public UCCenterView()
        {
            this.InitializeComponent();
            this.splitter1.SplitterMoved += delegate (object sender, SplitterEventArgs e)
            {
                IniConfigSetting.Cur.SetConfigParamValue("UISetting", "UIWorkPaneHeight", this.workpane.Height.ToString());
            };
            this.HideWorkPane();
        }

        private void SelectCustomView(IUCView uc)
        {
            base.SuspendLayout();
            bool flag = this._curview != uc;
            if (flag)
            {
                bool flag2 = this._curview != null;
                if (flag2)
                {
                    this.panel1.Controls.Remove(this._curview as UserControl);
                }
                bool flag3 = uc != null;
                if (flag3)
                {
                    this.panel1.Controls.Add(uc as UserControl);
                    (uc as UserControl).Dock = DockStyle.Fill;
                }
                this._curview = uc;
            }
            base.ResumeLayout();
        }

        public void ProcessCMD(string cmd)
        {
            bool flag = this._curview != null;
            if (flag)
            {
                this._curview.ProcessCMD(cmd);
            }
        }

        private void HideWorkPane()
        {
            bool flag = base.Controls.Contains(this.workpane);
            if (flag)
            {
                base.Controls.Remove(this.workpane);
            }
            this.splitter1.BorderStyle = BorderStyle.None;
        }

        private void ShowWorkPane()
        {
            this.splitter1.BorderStyle = BorderStyle.FixedSingle;
            bool flag = !base.Controls.Contains(this.workpane);
            if (flag)
            {
                base.Controls.Add(this.workpane);
            }
            this.workpane.Height = IniConfigSetting.Cur.GetConfigParamValue("UISetting", "UIWorkPaneHeight").ToInt();
        }

        private void btnCloseWorkPane_Click(object sender, EventArgs e)
        {
            this.HideWorkPane();
        }

        public void ShowWorkPane(object param)
        {
            bool flag = base.Controls.Contains(this.workpane);
            if (flag)
            {
                this.HideWorkPane();
            }
            else
            {
                this.ShowWorkPane();
            }
        }

        public void Get(object param)
        {
            bool flag = base.Controls.Contains(this.workpane);
            if (flag)
            {
                this.HideWorkPane();
            }
            else
            {
                this.ShowWorkPane();
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
            this.panel1 = new Panel();
            this.workpane = new UCWorkPane();
            this.btnCloseWorkPane = new Button();
            this.splitter1 = new Splitter();
            this.workpane.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(927, 571);
            this.panel1.TabIndex = 0;
            this.workpane.Controls.Add(this.btnCloseWorkPane);
            this.workpane.Dock = DockStyle.Bottom;
            this.workpane.Location = new Point(0, 347);
            this.workpane.Name = "workpane";
            this.workpane.Size = new Size(927, 227);
            this.workpane.TabIndex = 1;
            this.btnCloseWorkPane.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            this.btnCloseWorkPane.Location = new Point(905, 2);
            this.btnCloseWorkPane.Name = "btnClosePane2";
            this.btnCloseWorkPane.Size = new Size(20, 20);
            this.btnCloseWorkPane.TabIndex = 1;
            this.btnCloseWorkPane.Text = "x";
            this.btnCloseWorkPane.UseVisualStyleBackColor = true;
            this.btnCloseWorkPane.Click += new EventHandler(this.btnCloseWorkPane_Click);
            this.splitter1.BorderStyle = BorderStyle.FixedSingle;
            this.splitter1.Dock = DockStyle.Bottom;
            this.splitter1.Location = new Point(0, 571);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(927, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.splitter1);
            base.Margin = new Padding(4);
            base.Name = "UCCenterView";
            base.Size = new Size(927, 574);
            this.workpane.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
