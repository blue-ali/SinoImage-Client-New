using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCTopMenuBubbleBar : UserControl
    {

        private BubbleBar _bubbleBar1;

        private IContainer components = null;

        public event EventHandler<TEventArg<string>> OnActionClick;

        public UCTopMenuBubbleBar()
        {
            this.DoubleBuffered = true;
            this.InitializeComponent();
            this.InitializeChild();
            this.OnActionClick += new EventHandler<TEventArg<string>>(this.UCTopMenuBubbleBar_OnActionClick);
            bool flag = this._bubbleBar1 != null;
            if (flag)
            {
                this._bubbleBar1.MouseClick += new MouseEventHandler(this.UCTopMenuBubbleBar_MouseClick);
            }
        }

        private void UCTopMenuBubbleBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (AppSetting.GetInstance().ShowAdvanceSetting)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItem = new MenuItem();
                    menuItem.Text = "刷新";
                    menuItem.Click += delegate (object newsender, EventArgs newe)
                    {
                        base.SuspendLayout();
                        bool flag3 = this._bubbleBar1 != null;
                        if (flag3)
                        {
                            this._bubbleBar1.Items.Clear();
                            base.Controls.Remove(this._bubbleBar1);
                            this._bubbleBar1.Dispose();
                            this._bubbleBar1 = null;
                        }
                        this.InitializeChild();
                        base.ResumeLayout();
                        this.Refresh();
                    };
                    contextMenu.MenuItems.Add(menuItem);
                    contextMenu.Show(this, e.Location);
                }
            }
        }

        private void UCTopMenuBubbleBar_OnActionClick(object sender, TEventArg<string> e)
        {
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(e.Arg, null);
        }

        public void InitializeChild()
        {
            bool designMode = base.DesignMode;
            string str;
            if (designMode)
            {
                str = SystemHelper.GetDesignDirectory();
            }
            else
            {
                str = SystemHelper.GetAssemblesDirectory();
            }
            GroupItems groupItems = GroupItems.UnSerializeFromXML(str + "toolitems.xml");
            bool flag = groupItems == null;
            if (!flag)
            {
                List<GroupItem> topBarItems = groupItems.TopBarItems;
                bool flag2 = topBarItems == null;
                if (!flag2)
                {
                    List<ToolItem> toolItems = topBarItems[0].ToolItems;
                    this._bubbleBar1 = new BubbleBar();
                    this._bubbleBar1.Name = "bubbleBar1";
                    ((ISupportInitialize)this._bubbleBar1).BeginInit();
                    this._bubbleBar1.ForeColor = Color.Green;
                    this._bubbleBar1.BackColor = Color.Transparent;
                    this._bubbleBar1.Element.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
                    this._bubbleBar1.Dock = DockStyle.Fill;
                    foreach (ToolItem current in toolItems)
                    {
                        bool flag3 = !current.visable;
                        if (!flag3)
                        {
                            RadButtonElement radButtonElement = new RadButtonElement();
                            radButtonElement.CanFocus = true;
                            string text = SystemHelper.ResourceDir + current.image;
                            bool flag4 = File.Exists(text);
                            if (flag4)
                            {
                                radButtonElement.Image = DocScanner.ImgUtils.ImageHelper.LoadSizedImage(text, UISetting.GetInstance().ButtonSize, UISetting.GetInstance().ButtonSize, "");
                            }
                            radButtonElement.Name = current.name;
                            radButtonElement.Padding = new Padding(2, 2, 2, 8);
                            radButtonElement.ScaleTransform = new SizeF(0.65f, 0.65f);
                            radButtonElement.ShowBorder = false;
                            radButtonElement.ToolTipText = current.tip;
                            radButtonElement.Text = (UISetting.GetInstance().ShowButtonText ? current.text : "");
                            radButtonElement.TextAlignment = ContentAlignment.BottomCenter;
                            radButtonElement.Tag = current.action;
                            radButtonElement.MouseDown += new MouseEventHandler(this.radButtonElement_MouseDown);
                            ((FillPrimitive)radButtonElement.GetChildAt(0)).Visibility = ElementVisibility.Hidden;
                            this._bubbleBar1.Items.Add(radButtonElement);
                        }
                    }
                    string barBackImage = UISetting.GetInstance().BarBackImage;
                    bool flag5 = File.Exists(barBackImage);
                    if (flag5)
                    {
                        this._bubbleBar1.BackgroundImage = DocScanner.ImgUtils.ImageHelper.LoadLocalImage(barBackImage, true);
                    }
                    else
                    {
                        this._bubbleBar1.Element.FillWithColor(UISetting.GetInstance().BarBackColor);
                    }
                    ((ISupportInitialize)this._bubbleBar1).EndInit();
                    base.Controls.Add(this._bubbleBar1);
                }
            }
        }

        private void radButtonElement_MouseDown(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                string input = (sender as RadButtonElement).Tag as string;
                bool flag2 = this.OnActionClick != null;
                if (flag2)
                {
                    this.OnActionClick(this, new TEventArg<string>(input));
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
            base.SuspendLayout();
            base.Name = "UCMenuPane";
            base.Size = new Size(717, 143);
            base.ResumeLayout(false);
        }
    }
}
