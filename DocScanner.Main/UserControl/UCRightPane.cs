using DocScanner.Bean;
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
    public class UCRightPane : UserControl
    {
        private static readonly UCRightPane instance = new UCRightPane();

        private TabPage _pageshenhe;

        private TabControl _tabctrl;

        private UCSummary _ucsum;

        private UCItemToolBar _uctoolbar;

        private UCShenhe _ucshenhe;

        private IContainer components = null;

        private UCRightPane()
        {
            this.InitializeComponent();
            base.SuspendLayout();
            this._tabctrl = new TabControl();
            base.Controls.Add(this._tabctrl);
            this._tabctrl.Dock = DockStyle.Fill;
            this._uctoolbar = new UCItemToolBar();
            this._uctoolbar.Dock = DockStyle.Fill;
            TabPage tabPage = new TabPage();
            tabPage.Text = this._uctoolbar.Title;
            tabPage.Controls.Add(this._uctoolbar);
            this._tabctrl.TabPages.Add(tabPage);
            this._ucsum = UCSummary.GetInstance();
            this._ucsum.Dock = DockStyle.Fill;
            TabPage tabPage2 = new TabPage();
            tabPage2.Text = this._ucsum.Title;
            tabPage2.Controls.Add(this._ucsum);
            this._tabctrl.TabPages.Add(tabPage2);
            this._tabctrl.MouseClick += new MouseEventHandler(this._tabctrl_MouseClick);
            base.ResumeLayout();
        }

        public static UCRightPane GetIntstance()
        {
            return instance;
        }

        private void _tabctrl_MouseClick(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Right && this._tabctrl.SelectedTab == this._pageshenhe;
            if (flag)
            {
                ContextMenu contextMenu = new ContextMenu();
                new MenuItem
                {
                    Text = "关闭"
                }.Click += delegate (object xsender, EventArgs ex)
                {
                    this.HideShenhePage();
                };
            }
        }

        public void OnNodeChanged(RadTreeNode node, bool InvokedSumUI)
        {
            this._ucsum.ShowNodeInfo(node, InvokedSumUI);
            if (node != null && node.Tag is NFileInfo && (node.Tag as NFileInfo).HasExShenheInfo())
            {
                this.ShowShenHepage();
            }
        }

        public void ShowShenHepage()
        {
            base.SuspendLayout();
            bool flag = this._pageshenhe == null;
            if (flag)
            {
                this._ucshenhe = new UCShenhe();
                this._ucshenhe.Dock = DockStyle.Fill;
                this._pageshenhe = new TabPage();
                this._pageshenhe.Text = this._ucshenhe.Title;
                this._pageshenhe.Controls.Add(this._ucshenhe);
            }
            bool flag2 = !this._tabctrl.TabPages.Contains(this._pageshenhe);
            if (flag2)
            {
                this._tabctrl.TabPages.Add(this._pageshenhe);
            }
            this._tabctrl.SelectTab(this._pageshenhe);
            base.ResumeLayout();
        }

        public void HideShenhePage()
        {
            base.SuspendLayout();
            bool flag = this._pageshenhe != null;
            if (flag)
            {
                this._tabctrl.TabPages.Remove(this._pageshenhe);
            }
            base.ResumeLayout();
        }

        public UCSummary GetUCSummary()
        {
            return this._ucsum;
        }

        public UCItemToolBar GetToolBar()
        {
            return this._uctoolbar;
        }

        public UCShenhe GetShenhe()
        {
            return this._ucshenhe;
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Margin = new Padding(4, 4, 4, 4);
            base.Name = "UCRightPane";
            base.Size = new Size(304, 720);
            base.ResumeLayout(false);
        }
    }
}
