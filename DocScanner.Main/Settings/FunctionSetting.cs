using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class FunctionSetting : IPropertiesSetting
    {

        private static readonly FunctionSetting instance = new FunctionSetting();

        private FunctionSetting() { }

        public static FunctionSetting GetInstance()
        {
            return instance;
        }

        [Browsable(false)]
        public string Name
        {
            get
            {
                return "可选功能设置";
            }
        }

        [Category("功能设置"), Description("上传成功后是否保留原始文件")]
        public bool KeepSuccessedUploadNodeInTree
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "ReserveFile").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "ReserveFile", value.ToString());
            }
        }

        public bool AllowRightPanePropertyGrid
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "AllowRightPanePropertyGrid").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "AllowRightPanePropertyGrid", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用文字识别")]
        public bool AllowOCR
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "FuncAllowOCR").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "FuncAllowOCR", value.ToString());
            }
        }

        [Category("功能设置"), Description("记录本地提交记录")]
        public bool AllowLogUploaded
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "AllowLogUploaded").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "AllowLogUploaded", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用批次模版")]
        public bool AllowTempalte
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "FuncAllowTempalte").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "FuncAllowTempalte", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用审核")]
        public bool AllowShenhe
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "FuncAllowShenhe").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "FuncAllowShenhe", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用发票校验")]
        public bool AllowFaPiao
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "FuncAllowFaPiaoCheck").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "FuncAllowFaPiaoCheck", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用马赛克")]
        public bool AllowMasaic
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("FunctionSetting", "FuncAllowMasaic").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("FunctionSetting", "FuncAllowMasaic", value.ToString());
            }
        }

    }
}
