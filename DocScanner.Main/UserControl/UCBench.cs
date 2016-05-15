using DocScanner.AdapterFactory;
using DocScanner.Bean;
using DocScanner.Common;
using DocScanner.LibCommon;
using DocScanner.Main;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[ComVisible(true), ProgId("TigEraDocProcessor"), Guid("17E03D90-5299-474b-A5F0-9CCC0BB094F3")]
public class UCBench : UserControl, IObjectSafety
{
    // Fields
    private bool _fSafeForInitializing = true;
    private bool _fSafeForScripting = true;
    private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
    private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
    private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";
    private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
    private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
    private Splitter _splitterLeft;
    private Splitter _splitterRight;
    private IContainer components = null;
    private const int E_FAIL = -2147467259;
    private const int E_NOINTERFACE = -2147467262;
    private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 1;
    private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 2;
    internal Panel MiddlePane;
    private const int S_OK = 0;
    private TabControl tabControlLeft;
    internal TableLayoutPanel tableLayoutPanel1;
    private TabPage tabPage3;
    private UCStatusBar ucbottomStatusBar1;
    internal UCCenterView ucCenterview;
    private UCLeftPane ucLeftPane;
    private UCRightPane ucRightPane;
    private UCTopMenuBubbleBar ucTopMenuBubbleBar1;

    // Methods
    public UCBench()
    {
        try
        {
            //if (this.GetConponentMode() == EControlMode.DesignMode)
            //{
            //    SystemHelper.SetAssemblesDirectory(@"D:\tigera\imgmgr\TigEra.DocScaner.Main\");
            //}
            string path = SystemHelper.GetAssemblesDirectory() + "AppConfig.ini";
            if (!File.Exists(path))
            {
                throw new Exception("配置文件" + path + "不存在");
            }
            IniConfigSetting.Cur.ConfigFileName = path;
            if (DebugSetting.ExceptionThrowable)
            {
            }
            this.BeforeUILoad(null, null);
            this.InitializeComponent();
            this.AfterUILoad(null, null);
            base.HandleDestroyed += new EventHandler(this.UCBench_HandleDestroyed);
            this.tabControlLeft.Width = IniConfigSetting.Cur.GetConfigParamValue("UISetting", "UILeftPaneWidth").ToInt();
            this.ucRightPane.Width = IniConfigSetting.Cur.GetConfigParamValue("UISetting", "UIRightPaneWidth").ToInt();
            this._splitterLeft.SplitterMoved += (sender, e) => IniConfigSetting.Cur.SetConfigParamValue("UISetting", "UILeftPaneWidth", this.tabControlLeft.Width.ToString());
            this._splitterRight.SplitterMoved += (sender, e) => IniConfigSetting.Cur.SetConfigParamValue("UISetting", "UIRightPaneWidth", this.ucRightPane.Width.ToString());
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString());
        }
    }

    private void AfterUILoad(object sender, EventArgs e)
    {
        AppContext.GetInstance().SetVal(typeof(UCBench), this);
        AppContext.GetInstance().MS.OnMessage += (senderx, arg) => this.ucbottomStatusBar1.OnMessage(senderx, arg);
        CmdDispatcher val = AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher));
        val.SetDispatchObj(this.ucCenterview);
        val.SetDispatchObj(this.ucLeftPane.GetBar());
        val.SetDispatchObj(this.ucTopMenuBubbleBar1);
        val.SetDispatchObj(this.ucbottomStatusBar1);
        val.SetDispatchObj(this.ucRightPane.GetToolBar());
        val.SetDispatchObj(this.ucRightPane);
        val.SetDispatchObj(this);
        DurTimeJob.SetStatusBar(this.ucbottomStatusBar1);
        ExFunctionsRegister.AutoRegisterProcessCmd();
    }

    private void BeforeUILoad(object sender, EventArgs e)
    {
        AppContext.GetInstance().SetVal(typeof(CmdDispatcher), new CmdDispatcher());
        AppContext.GetInstance().SetVal(typeof(AppSetting), AppSetting.GetInstance());
        AppContext.GetInstance().SetVal(typeof(NoteSetting), AbstractSetting<NoteSetting>.CurSetting);
        AppContext.GetInstance().SetVal(typeof(SharpAcquirerFactory), new SharpAcquirerFactory());
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    [ComVisible(true)]
    public void EmptyRubbish()
    {
        TmpGC.EmptyRubbish();
    }

    public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
    {
        int num = -2147467259;
        string str = riid.ToString("B");
        pdwSupportedOptions = 3;
        string str2 = str;
        if (!(str2 == "{00020400-0000-0000-C000-000000000046}") && !(str2 == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}"))
        {
            if (((str2 != "{0000010A-0000-0000-C000-000000000046}") && (str2 != "{00000109-0000-0000-C000-000000000046}")) && (str2 != "{37D84F60-42CB-11CE-8135-00AA004BB851}"))
            {
                return -2147467262;
            }
        }
        else
        {
            num = 0;
            pdwEnabledOptions = 0;
            if (this._fSafeForScripting)
            {
                pdwEnabledOptions = 1;
            }
            return num;
        }
        num = 0;
        pdwEnabledOptions = 0;
        if (this._fSafeForInitializing)
        {
            pdwEnabledOptions = 2;
        }
        return num;
    }

    public static bool HasNewVer(string webver)
    {
        if (!string.IsNullOrEmpty(webver))
        {
            webver = webver.Replace(",", ".");
            if (string.Compare(UpdateSetting.GetInstance().AppVersion, webver) < 0)
            {
                MessageBox.Show("服务器有新版本" + webver + "新下载" + Environment.NewLine + "本地版本" + UpdateSetting.GetInstance().AppVersion, "更新提示", MessageBoxButtons.OK);
                return true;
            }
        }
        return false;
    }

    [ComVisible(true)]
    public bool Initialize(string url)
    {
        return ParseURL(url);
    }

    private void InitializeComponent()
    {
        this.tableLayoutPanel1 = new TableLayoutPanel();
        this.MiddlePane = new Panel();
        this._splitterRight = new Splitter();
        this._splitterLeft = new Splitter();
        this.tabControlLeft = new TabControl();
        this.tabPage3 = new TabPage();
        this.ucCenterview = UCCenterView.GetInstance();
        this.ucRightPane = UCRightPane.GetIntstance();
        this.ucLeftPane = new UCLeftPane();
        this.ucbottomStatusBar1 = new UCStatusBar();
        this.ucTopMenuBubbleBar1 = new UCTopMenuBubbleBar();
        this.tableLayoutPanel1.SuspendLayout();
        this.MiddlePane.SuspendLayout();
        this.tabControlLeft.SuspendLayout();
        this.tabPage3.SuspendLayout();
        base.SuspendLayout();
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        this.tableLayoutPanel1.Controls.Add(this.MiddlePane, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.ucbottomStatusBar1, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.ucTopMenuBubbleBar1, 0, 0);
        this.tableLayoutPanel1.Dock = DockStyle.Fill;
        this.tableLayoutPanel1.Location = new Point(0, 0);
        this.tableLayoutPanel1.Margin = new Padding(5);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 3;
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 138f));
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
        this.tableLayoutPanel1.Size = new Size(0x500, 720);
        this.tableLayoutPanel1.TabIndex = 0;
        this.MiddlePane.Controls.Add(this.ucCenterview);
        this.MiddlePane.Controls.Add(this._splitterRight);
        this.MiddlePane.Controls.Add(this._splitterLeft);
        this.MiddlePane.Controls.Add(this.ucRightPane);
        this.MiddlePane.Controls.Add(this.tabControlLeft);
        this.MiddlePane.Dock = DockStyle.Fill;
        this.MiddlePane.Location = new Point(5, 0x8f);
        this.MiddlePane.Margin = new Padding(5);
        this.MiddlePane.Name = "MiddlePane";
        this.MiddlePane.Size = new Size(0x4f6, 0x210);
        this.MiddlePane.TabIndex = 1;
        this._splitterRight.Dock = DockStyle.Right;
        this._splitterRight.Location = new Point(0x31d, 0);
        this._splitterRight.Margin = new Padding(4);
        this._splitterRight.Name = "_splitterRight";
        this._splitterRight.Size = new Size(10, 0x210);
        this._splitterRight.TabIndex = 3;
        this._splitterRight.TabStop = false;
        this._splitterLeft.Location = new Point(0xee, 0);
        this._splitterLeft.Margin = new Padding(3, 2, 3, 2);
        this._splitterLeft.Name = "_splitterLeft";
        this._splitterLeft.Size = new Size(10, 0x210);
        this._splitterLeft.TabIndex = 1;
        this._splitterLeft.TabStop = false;
        this.tabControlLeft.Controls.Add(this.tabPage3);
        this.tabControlLeft.Dock = DockStyle.Left;
        this.tabControlLeft.Location = new Point(0, 0);
        this.tabControlLeft.Margin = new Padding(5);
        this.tabControlLeft.Name = "tabControlLeft";
        this.tabControlLeft.SelectedIndex = 0;
        this.tabControlLeft.Size = new Size(0xee, 0x210);
        this.tabControlLeft.TabIndex = 4;
        this.tabPage3.Controls.Add(this.ucLeftPane);
        this.tabPage3.Location = new Point(4, 0x19);
        this.tabPage3.Margin = new Padding(5);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new Padding(5);
        this.tabPage3.Size = new Size(230, 0x1f3);
        this.tabPage3.TabIndex = 0;
        this.tabPage3.Text = "文件管理";
        this.tabPage3.UseVisualStyleBackColor = true;
        this.ucCenterview.CurFileInfo = null;
        this.ucCenterview.Dock = DockStyle.Fill;
        this.ucCenterview.Location = new Point(0xf8, 0);
        this.ucCenterview.Margin = new Padding(1);
        this.ucCenterview.Name = "ucCenterview";
        this.ucCenterview.Realview = null;
        this.ucCenterview.ShowedWorkPane = false;
        this.ucCenterview.Size = new Size(0x225, 0x210);
        this.ucCenterview.TabIndex = 1;
        this.ucRightPane.Dock = DockStyle.Right;
        this.ucRightPane.Location = new Point(0x327, 0);
        this.ucRightPane.Margin = new Padding(5);
        this.ucRightPane.Name = "ucRightPane";
        this.ucRightPane.Size = new Size(0x1cf, 0x210);
        this.ucRightPane.TabIndex = 0;
        this.ucLeftPane.Dock = DockStyle.Fill;
        this.ucLeftPane.Location = new Point(5, 5);
        this.ucLeftPane.Margin = new Padding(7, 6, 7, 6);
        this.ucLeftPane.Name = "ucLeftPane";
        this.ucLeftPane.Size = new Size(220, 0x1e9);
        this.ucLeftPane.TabIndex = 3;
        this.ucbottomStatusBar1.BackColor = Color.White;
        this.ucbottomStatusBar1.Dock = DockStyle.Fill;
        this.ucbottomStatusBar1.Location = new Point(7, 0x2aa);
        this.ucbottomStatusBar1.Margin = new Padding(7, 6, 7, 6);
        this.ucbottomStatusBar1.Name = "ucbottomStatusBar1";
        this.ucbottomStatusBar1.Size = new Size(0x4f2, 0x20);
        this.ucbottomStatusBar1.TabIndex = 3;
        this.ucTopMenuBubbleBar1.Dock = DockStyle.Fill;
        this.ucTopMenuBubbleBar1.Location = new Point(5, 5);
        this.ucTopMenuBubbleBar1.Margin = new Padding(5);
        this.ucTopMenuBubbleBar1.Name = "ucTopMenuBubbleBar1";
        this.ucTopMenuBubbleBar1.Size = new Size(0x4f6, 0x80);
        this.ucTopMenuBubbleBar1.TabIndex = 4;
        base.AutoScaleDimensions = new SizeF(8f, 15f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.tableLayoutPanel1);
        base.Margin = new Padding(4);
        base.Name = "UCBench";
        base.Size = new Size(0x500, 720);
        this.tableLayoutPanel1.ResumeLayout(false);
        this.MiddlePane.ResumeLayout(false);
        this.tabControlLeft.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        base.ResumeLayout(false);
    }

    public void LeftPaneShift(object param)
    {
        this.tabControlLeft.Visible = !this.tabControlLeft.Visible;
    }

    public static bool ParseURL(string url)
    {
        url = url.ToLower();
        UrlParser parser = new UrlParser();
        parser.Parse(url, '&');
        if ((parser.GetKey("ocxversion") != string.Empty) && HasNewVer(parser.GetKey("ocxversion")))
        {
            return false;
        }
        AccountSetting.GetInstance().AccountName = parser.GetKey("currentuser");
        BusinessSetting.GetInstance().bustype = parser.GetKey("bustype");
        BusinessSetting.GetInstance().busno = parser.GetKey("busno");
        AccountSetting.GetInstance().AccountOrgID = parser.GetKey("unitno");
        if (parser.GetKey("mode") == "update")
        {
            string key = parser.GetKey("barno");
            AppContext.GetInstance().GetVal<UCBench>(typeof(UCBench)).QueryMode(key.ToUpper());
        }
        return true;
    }

    [ComVisible(true)]
    public bool QueryMode(string code)
    {
        code = BatchNoMaker.Cur.FromInputDialog(code);
        UCQueryBatch ctrl = new UCQueryBatch
        {
            CurrentBatchNo = code
        };
        FormQBContainer container = new FormQBContainer();
        container.AddControl(ctrl);
        container.TopLevel = true;
        container.ShowDialog();
        ctrl.DownloadGroup.Update2NoneMode();
        NBatchInfoGroup group = ctrl.DownloadGroup.MyClone();
        this.ucLeftPane.GetBar().GetNavigateTree().FromBatch(group);
        return true;
    }

    public void RightPaneShift(object param)
    {
        this.ucRightPane.Visible = !this.ucRightPane.Visible;
    }

    public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
    {
        int num = -2147467259;
        string str2 = riid.ToString("B");
        if (!(str2 == "{00020400-0000-0000-C000-000000000046}") && !(str2 == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}"))
        {
            if (((str2 != "{0000010A-0000-0000-C000-000000000046}") && (str2 != "{00000109-0000-0000-C000-000000000046}")) && (str2 != "{37D84F60-42CB-11CE-8135-00AA004BB851}"))
            {
                return -2147467262;
            }
        }
        else
        {
            if (((dwEnabledOptions & dwOptionSetMask) == 1) && this._fSafeForScripting)
            {
                num = 0;
            }
            return num;
        }
        if (((dwEnabledOptions & dwOptionSetMask) == 2) && this._fSafeForInitializing)
        {
            num = 0;
        }
        return num;
    }

    public void ShowHelp(object param = null)
    {
        string url = SystemHelper.GetAssemblesDirectory() + @"Resources\TigEraHlp.CHM";
        Help.ShowHelp(this, url);
        Help.ShowHelpIndex(this, url);
    }

    private void UCBench_HandleDestroyed(object sender, EventArgs e)
    {
        //AppContext.GetInstance().Dispose();
    }

}


