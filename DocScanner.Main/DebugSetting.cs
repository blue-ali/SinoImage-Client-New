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
    public class DebugSetting : AbstractSetting<DebugSetting>, IPropertiesSetting
    {
        [Category("调试设置"), Description("是否异常完全抛出")]
        public static bool ExceptionThrowable
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("DebugSetting", "ExceptionThrowable").ToBool();
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("DebugSetting", "ExceptionThrowable", value.ToString());
            }
        }

        public EMessageType LogLevel
        {
            get
            {
                return (EMessageType)IniConfigSetting.Cur.GetConfigParamValue("DebugSetting", "LogLevel").ToInt();
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("DebugSetting", "LogLevel", value.ToString());
            }
        }

        [Browsable(false)]
        public override string Name
        {
            get
            {
                return "调试设置";
            }
        }

        public override bool Equals(DebugSetting other)
        {
            return false;
        }
    }
}
