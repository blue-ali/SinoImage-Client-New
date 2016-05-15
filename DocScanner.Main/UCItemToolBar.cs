using DocScanner.AdapterFactory;
using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCItemToolBar : RadPanelBar, IHasIPropertiesSetting
    {// Fields
        private NestIPropertiesSetting _setting;
        private ToolTip _tip = new ToolTip();
        private IContainer components = null;

        // Events
        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<TEventArg<string>> OnActionClick;

        // Methods
        public UCItemToolBar()
        {
            this.InitializeComponent();
            base.GroupStyle = PanelBarStyles.ExplorerBarStyle;
            this.OnActionClick += new EventHandler<TEventArg<string>>(this.UCItemToolBar_OnActionClick);
            this.InitToolItems();
            base.MouseClick += new MouseEventHandler(this.UCItemToolBar_MouseClick);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public NestIPropertiesSetting GetSetting()
        {
            if (this._setting == null)
            {
                this._setting = new NestIPropertiesSetting(this);
            }
            return this._setting;
        }

        public void InitializeChild(List<GroupItem> gitems)
        {
            base.SuspendLayout();
            if (gitems != null)
            {
                foreach (GroupItem item in gitems)
                {
                    if (item.visable)
                    {
                        RadPanelBarGroupElement element = new RadPanelBarGroupElement();
                        //RadPageViewElement element = RadPageViewElement.
                        if (this.GetSetting().ShowGroupTtile)
                        {
                            element.Caption = item.name;
                            element.Font = new Font(this.Font.Name, this.GetSetting().GroupTitleFontSize);
                        }
                        else
                        {
                            element.Caption = "";
                        }
                        element.ContentPanel.SuspendLayout();
                        element.ContentPanel.BackColor = Color.FromArgb(0xf8, 0xf8, 0xf8);
                        element.ContentPanel.CausesValidation = true;
                        element.EnableHostControlMode = true;
                        int num = 0;
                        foreach (ToolItem item2 in item.ToolItems)
                        {
                            string tip;
                            RadButton radButton;
                            if (item2.visable)
                            {
                                radButton = new RadButton();
                                radButton.BeginInit();
                                element.ContentPanel.Controls.Add(radButton);
                                radButton.AllowShowFocusCues = true;
                                radButton.ForeColor = Color.Black;
                                radButton.Location = new Point(this.GetSetting().SetButtonSize * (num % 4), 12 + (this.GetSetting().SetButtonSize * (num / 4)));
                                num++;
                                radButton.Margin = new Padding(5, 5, 5, 5);
                                radButton.Name = item2.name;
                                radButton.RootElement.ForeColor = Color.Black;
                                radButton.Size = new Size(this.GetSetting().SetButtonSize - 2, this.GetSetting().SetButtonSize - 2);
                                radButton.Text = item2.text;
                                string path = SystemHelper.ResourceDir + item2.image;
                                if (File.Exists(path))
                                {
                                    radButton.Image = DocScanner.ImgUtils.ImageHelper.LoadSizedImage(path, this.GetSetting().SetButtonSize - 5, this.GetSetting().SetButtonSize - 5, "");
                                    radButton.SmallImageScalingSize = new Size(this.GetSetting().SetButtonSize - 2, this.GetSetting().SetButtonSize - 2);
                                    radButton.Text = "";
                                }
                                radButton.MouseClick += new MouseEventHandler(this.radButton_MouseClick);
                                radButton.Tag = item2.action;
                                radButton.ShowItemToolTips = true;
                                tip = item2.tip;
                                radButton.ToolTipTextNeeded += (sender, e) => this._tip.Show(tip, radButton);
                                radButton.EndInit();
                            }
                        }
                        element.ContentPanel.ResumeLayout();
                        base.Items.Add(element);
                    }
                }
                foreach (RadPanelBarGroupElement element2 in base.Items)
                {
                    element2.Expanded = true;
                    element2.StretchHorizontally = true;
                }
            }
            base.ResumeLayout();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.Name = "UCItemToolBar";
            base.Size = new Size(290, 570);
            base.ResumeLayout(false);
        }

        private void InitToolItems()
        {
            string designDirectory;
            if (base.DesignMode)
            {
                designDirectory = SystemHelper.GetDesignDirectory();
            }
            else
            {
                designDirectory = SystemHelper.GetAssemblesDirectory();
            }
            GroupItems items = GroupItems.UnSerializeFromXML(designDirectory + "toolitems.xml");
            if (items != null)
            {
                this.InitializeChild(items.RightPaneItems);
            }
        }

        private void radButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                string tag = (sender as RadButton).Tag as string;
                if (this.OnActionClick != null)
                {
                    this.OnActionClick(this, new TEventArg<string>(tag));
                }
            }
        }

        public void ShowAdpaterSetting(object param = null)
        {
            SharpAcquirerFactory.ShowSetting(LibCommon.AppContext.GetInstance().GetVal<SharpAcquirerFactory>(typeof(SharpAcquirerFactory)));
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return this.GetSetting();
        }

        private void UCItemToolBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (AppSetting.GetInstance().ShowAdvanceSetting && (e.Button == MouseButtons.Right))
            {
                ContextMenu menu = new ContextMenu();
                MenuItem item = new MenuItem
                {
                    Text = "刷新"
                };
                item.Click += delegate (object newsender, EventArgs newe) {
                    base.Items.Clear();
                    this.InitToolItems();
                    this.Refresh();
                };
                menu.MenuItems.Add(item);
                menu.Show(this, e.Location);
            }
        }

        private void UCItemToolBar_OnActionClick(object sender, TEventArg<string> e)
        {
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(e.Arg, null);
        }

        // Properties
        public string Title
        {
            get
            {
                return "工具栏";
            }
        }

        // Nested Types
        public class NestIPropertiesSetting : IPropertiesSetting
        {
            // Fields
            private UCItemToolBar _bar;

            // Methods
            public NestIPropertiesSetting(UCItemToolBar bar)
            {
                this._bar = bar;
            }

            // Properties
            public float GroupTitleFontSize
            {
                get
                {
                    string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("RightPropertyPane", "GroupTitleFontSize");
                    if (string.IsNullOrEmpty(configParamValue))
                    {
                        return 8f;
                    }
                    return float.Parse(configParamValue);
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("RightPropertyPane", "GroupTitleFontSize", value.ToString());
                }
            }

            [Browsable(false)]
            public string Name
            {
                get
                {
                    return "界面设置-右边工具栏";
                }
            }

            public int SetButtonSize
            {
                get
                {
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("RightPropertyPane", "buttonSize").ToInt();
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("RightPropertyPane", "buttonSize", value.ToString());
                }
            }

            public bool ShowGroupTtile
            {
                get
                {
                    //TODO what is this;
                    if (LibCommon.AppContext.GetInstance() == null)
                    {
                        MessageBox.Show("cur");
                    }
                    if (LibCommon.AppContext.GetInstance().Config == null)
                    {
                        MessageBox.Show("AppContext.GetInstance().Config");
                    }
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("RightPropertyPane", "ShowGroupTtile").ToBool();
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("RightPropertyPane", "ShowGroupTtile", value.ToString());
                }
            }
        }

    }
}
