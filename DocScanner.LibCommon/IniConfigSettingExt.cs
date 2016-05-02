using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public static class IniConfigSettingExt
{
    // Methods
    public static void SetupIniConfig(this IniConfigSetting cfg, Control con)
    {
        if ((con != null) && (con.GetConponentMode() == EControlMode.DesignMode))
        {
            SystemHelper.SetAssemblesDirectory("D:\\tigera\\imgmgr\\TigEra.DocScaner.Main\\");
        }
        string str = SystemHelper.GetAssemblesDirectory() + "AppConfig.ini";
        cfg.ConfigFileName = str;
    }
}
}
