using DocScanner.Network;
using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Main.UC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class CmdDispatcher
    {
        private UCNavigatorBar _ucnavigatorbar;

        private UCTopMenuBubbleBar _uctopmenububblebar;

        private UCCenterView _uccenterview;

        private UCStatusBar _ucstatusbar;

        private UCItemToolBar _ucitemtoolbar;

        private UCBench _ucbench;

        private UCRightPane _ucrightpane;

        private Dictionary<string, Action<object>> _stringCMDs = new Dictionary<string, Action<object>>();

        private bool _initedcmds = false;

        private string[] _centerviewcmds;

        private void ShowSysSetting(object param = null)
        {
            FormSysSetting formSysSetting = new FormSysSetting();
            formSysSetting.ShowDialog();
        }

        public void SetDispatchObj(UCNavigatorBar bar)
        {
            this._ucnavigatorbar = bar;
            this._ucnavigatorbar.OnItemSelectChanged += this._ucnavigatorbar_OnItemSelectChanged;
        }

        public void SetDispatchObj(UCTopMenuBubbleBar bar)
        {
            this._uctopmenububblebar = bar;
        }

        public void SetDispatchObj(UCCenterView uc)
        {
            this._uccenterview = uc;
        }

        public void SetDispatchObj(UCStatusBar bar)
        {
            this._ucstatusbar = bar;
        }

        public void SetDispatchObj(UCItemToolBar bar)
        {
            this._ucitemtoolbar = bar;
        }

        public void SetDispatchObj(UCBench bar)
        {
            this._ucbench = bar;
        }

        public void SetDispatchObj(UCRightPane bar)
        {
            this._ucrightpane = bar;
        }

        private void _ucnavigatorbar_OnItemSelectChanged(object sender, TEventArg<RadTreeNode> e)
        {
            NFileInfo curFileInfo = null;
            bool flag = e == null || e.Arg == null || e.Arg.Tag == null;
            if (!flag)
            {
                curFileInfo = (e.Arg.Tag as NFileInfo);
            }
            this._uccenterview.CurFileInfo = curFileInfo;
            this._ucrightpane.OnNodeChanged(e.Arg, false);
        }

        public UCCenterView GetUCCenterView()
        {
            return this._uccenterview;
        }

        public void ProcessCMD(ShowRegionUserCMD cmd)
        {
        }

        public void ProcessCMD(ShowNodePropertyCMD cmd)
        {
            this._ucrightpane.OnNodeChanged(cmd.node, true);
        }

        public void RegisterCMD(string cmd, Action<object> act)
        {
            this._stringCMDs[cmd] = act;
        }

        private void InitCMDs()
        {
            if (!this._initedcmds)
            {
                this._initedcmds = true;
                this._stringCMDs["ScanBatchDoc"] = new Action<object>(this._ucnavigatorbar.ScanBatchDoc);
                this._stringCMDs["NavigateFirstItem"] = new Action<object>(this._ucnavigatorbar.NavigateFirstItem);
                this._stringCMDs["NavigateLastItem"] = new Action<object>(this._ucnavigatorbar.NavigateLastItem);
                this._stringCMDs["NavigateNextItem"] = new Action<object>(this._ucnavigatorbar.NavigateNextItem);
                this._stringCMDs["NavigatePrevItem"] = new Action<object>(this._ucnavigatorbar.NavigatePrevItem);
                this._stringCMDs["LeftPaneShift"] = new Action<object>(this._ucbench.LeftPaneShift);
                this._stringCMDs["ChangeView"] = new Action<object>(this._ucnavigatorbar.ChangeView);
                this._stringCMDs["UpdateThumbNail"] = new Action<object>(this._ucnavigatorbar.UpdateThumbNail);
                this._stringCMDs["DownloadBatch"] = new Action<object>(this._ucnavigatorbar.DownloadBatch);
                this._stringCMDs["UploadBatch"] = new Action<object>(this._ucnavigatorbar.UploadBatch);
                this._stringCMDs["NewBatch"] = new Action<object>(this._ucnavigatorbar.DoNewBatch);
                this._stringCMDs["ClearBatchs"] = new Action<object>(this._ucnavigatorbar.DoClearBatchs);
                this._stringCMDs["FilterImg"] = new Action<object>(this._ucnavigatorbar.DoFilterImg);
                this._stringCMDs["SetAppParams"] = new Action<object>(this.ShowAppParams);
                this._stringCMDs["SetAppStartParams"] = new Action<object>(this.ShowSysSetting);
                this._stringCMDs["RightPaneShift"] = new Action<object>(this._ucbench.RightPaneShift);
                this._stringCMDs["SetScanParams"] = new Action<object>(this._ucitemtoolbar.ShowAdpaterSetting);
                this._stringCMDs["AboutMe"] = new Action<object>(ExFunctionsRegister.AboutMe);
                this._stringCMDs["ShowHelp"] = new Action<object>(this._ucbench.ShowHelp);
                this._stringCMDs["ReportBug"] = new Action<object>(ExFunctionsRegister.ReportBug);
                this._stringCMDs["CheckUpdate"] = new Action<object>(ExFunctionsRegister.CheckUpdate);
                this._stringCMDs["ExplorerAppDir"] = new Action<object>(ExFunctionsRegister.ExplorerRootDir);
                this._stringCMDs["ChangeHistory"] = new Action<object>(ExFunctionsRegister.ShowChangeHistory);
                this._stringCMDs["test1"] = new Action<object>(UCMenuBarDesign.ShowSetting);
                this._stringCMDs["test2"] = new Action<object>(this._ucnavigatorbar.TestFromFileToTree);
                this._stringCMDs["WorkPane"] = new Action<object>(this._uccenterview.ShowWorkPane);
                this._stringCMDs["SelectNode"] = new Action<object>(this._ucnavigatorbar.DoSelectNode);
            }
            this._centerviewcmds = new string[]
            {
                "FullScreen",
                "DoAllShowNote",
                "DoZoomOut",
                "DoZoomIn",
                "ReLoadImage",
                "DoRotateImageL",
                "DoRotateImageR",
                "DoImageFlipHorizontal",
                "DoImageFlipVertical",
                "DoFitViewHeight",
                "DoFitViewWidth",
                "DoChangeLightness",
                "DoImageToComparison",
                "DoImageToGray",
                "DoImageToBlack",
                "DoImageSharp",
                "DoImageSoft",
                "AutoAmendBevel",
                "ImageClipCut",
                "ImageUndo",
                "RemoveBlackEdge",
                "RemoveBlock",
                "ShowAllNotes",
                "DoPrint"
            };
        }

        public void ProcessCMD(object cmd, object param = null)
        {
            this.InitCMDs();
            bool flag = cmd is string;
            if (flag)
            {
                string text = cmd as string;
                bool flag2 = this._stringCMDs.ContainsKey(text);
                if (flag2)
                {
                    this._stringCMDs[text](param);
                    return;
                }
                bool flag3 = this._centerviewcmds.Contains(text);
                if (flag3)
                {
                    this._uccenterview.ProcessCMD(text);
                    return;
                }
            }
            LibCommon.AppContext.GetInstance().MS.LogError("命令" + cmd + "没有找到");
        }

        private void ShowAppParams(object param = null)
        {
            FormContainer formContainer = new FormContainer();
            TabControl tabControl = new TabControl();
            TabPage tabPage = new TabPage();
            tabPage.Text = "快速设置";
            UCSetupWizard uCSetupWizard = new UCSetupWizard();
            uCSetupWizard.Dock = DockStyle.Fill;
            tabPage.Controls.Add(uCSetupWizard);
            tabControl.TabPages.Add(tabPage);
            TabPage tabPage2 = new TabPage();
            tabControl.TabPages.Add(tabPage2);
            UCMultiObjPropertyInfo uCMultiObjPropertyInfo = new UCMultiObjPropertyInfo();
            uCMultiObjPropertyInfo.AddObjs(AbstractSetting<UISetting>.CurSetting.Name, AbstractSetting<UISetting>.CurSetting);
            uCMultiObjPropertyInfo.AddObjs(this._ucbench.GetSetting().Name, this._ucbench.GetSetting());
            uCMultiObjPropertyInfo.AddObjs(this._uctopmenububblebar.GetSetting().Name, this._uctopmenububblebar.GetSetting());
            uCMultiObjPropertyInfo.AddObjs(this._ucnavigatorbar.GetSetting().Name, this._ucnavigatorbar.GetSetting());
            if (this._uccenterview.Realview != null && this._uccenterview.Realview is UCPictureView)
            {
                UCPictureView.NestSetting setting = (this._uccenterview.Realview as UCPictureView).GetSetting();
                uCMultiObjPropertyInfo.AddObjs(setting.Name, setting);
            }
            uCMultiObjPropertyInfo.AddObjs(this._ucitemtoolbar.GetSetting().Name, this._ucitemtoolbar.GetSetting());
            uCMultiObjPropertyInfo.AddObjs(this._ucrightpane.GetUCSummary().GetSetting().Name, this._ucrightpane.GetUCSummary().GetSetting());
            uCMultiObjPropertyInfo.AddObjs(this._ucstatusbar.GetSetting().Name, this._ucstatusbar.GetSetting());
            tabPage2.Controls.Add(uCMultiObjPropertyInfo);
            tabPage2.Text = "界面参数设置";
            uCMultiObjPropertyInfo.Dock = DockStyle.Fill;
            TabPage tabPage3 = new TabPage();
            tabControl.TabPages.Add(tabPage3);
            UCMultiObjPropertyInfo uCMultiObjPropertyInfo2 = new UCMultiObjPropertyInfo();
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<UpdateSetting>.CurSetting.Name, AbstractSetting<UpdateSetting>.CurSetting);
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<EmailSetting>.CurSetting.Name, AbstractSetting<EmailSetting>.CurSetting);
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<AccountSetting>.CurSetting.Name, AbstractSetting<AccountSetting>.CurSetting);
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<AppSetting>.CurSetting.Name, AbstractSetting<AppSetting>.CurSetting);
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<DebugSetting>.CurSetting.Name, AbstractSetting<DebugSetting>.CurSetting);
            uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<NetSetting>.CurSetting.Name, AbstractSetting<NetSetting>.CurSetting);
            bool showAdvanceSetting = AbstractSetting<AppSetting>.CurSetting.ShowAdvanceSetting;
            if (showAdvanceSetting)
            {
                uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<ServerIniConfig>.CurSetting.Name, AbstractSetting<ServerIniConfig>.CurSetting);
                uCMultiObjPropertyInfo2.AddObjs(AbstractSetting<FunctionSetting>.CurSetting.Name, AbstractSetting<FunctionSetting>.CurSetting);
                uCMultiObjPropertyInfo2.AddObjs(FormAboutMe.Setting.Name, FormAboutMe.Setting);
            }
            tabPage3.Controls.Add(uCMultiObjPropertyInfo2);
            tabPage3.Text = "系统参数参数设置";
            uCMultiObjPropertyInfo2.Dock = DockStyle.Fill;
            bool showAdvanceSetting2 = AbstractSetting<AppSetting>.CurSetting.ShowAdvanceSetting;
            if (showAdvanceSetting2)
            {
                TabPage tabPage4 = new TabPage();
                tabControl.TabPages.Add(tabPage4);
                UCMenuBarDesign uCMenuBarDesign = new UCMenuBarDesign();
                uCMenuBarDesign.Dock = DockStyle.Fill;
                tabPage4.Controls.Add(uCMenuBarDesign);
                tabPage4.Text = "菜单配置";
            }
            tabControl.Dock = DockStyle.Fill;
            formContainer.SetControl(tabControl);
            formContainer.Size = new Size(760, 540);
            formContainer.Text = "系统设置";
            formContainer.SetKeyEscCloseForm(true);
            formContainer.ShowDialog();
        }
    }
}
