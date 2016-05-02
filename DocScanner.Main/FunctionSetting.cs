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
    public class FunctionSetting : AbstractSetting<FunctionSetting>, IPropertiesSetting
    {
        [Browsable(false)]
        public override string Name
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
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "ReserveFile").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "ReserveFile", value.ToString());
            }
        }

        public bool AllowRightPanePropertyGrid
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "AllowRightPanePropertyGrid").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "AllowRightPanePropertyGrid", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用文字识别")]
        public bool AllowOCR
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "FuncAllowOCR").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "FuncAllowOCR", value.ToString());
            }
        }

        [Category("功能设置"), Description("记录本地提交记录")]
        public bool AllowLogUploaded
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "AllowLogUploaded").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "AllowLogUploaded", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用批次模版")]
        public bool AllowTempalte
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "FuncAllowTempalte").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "FuncAllowTempalte", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用审核")]
        public bool AllowShenhe
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "FuncAllowShenhe").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "FuncAllowShenhe", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用发票校验")]
        public bool AllowFaPiao
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "FuncAllowFaPiaoCheck").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "FuncAllowFaPiaoCheck", value.ToString());
            }
        }

        [Category("功能设置"), Description("启用马赛克")]
        public bool AllowMasaic
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("FunctionSetting", "FuncAllowMasaic").ToBool();
            }
            set
            {
                LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("FunctionSetting", "FuncAllowMasaic", value.ToString());
            }
        }

        public override bool Equals(FunctionSetting other)
        {
            return false;
        }
    }
}
