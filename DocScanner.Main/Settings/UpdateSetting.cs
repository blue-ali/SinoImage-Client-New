using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class UpdateSetting : IPropertiesSetting
    {
        public const string bindAppVersion = "AppVersion";
        private static readonly UpdateSetting instance = new UpdateSetting();

        private UpdateSetting() { }

        public static UpdateSetting GetInstance()
        {
            return instance;
        }

        [Category("升级设置"), Description("程序版本")]
        public string AppVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        [Browsable(false)]
        public string Name
        {
            get
            {
                return "升级设置";
            }
        }

        [Category("升级设置"), Description("升级文件根目录")]
        public string UpdateFilesURL
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("UpdateSetting", "UpdateFilesURL");
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("UpdateSetting", "UpdateFilesURL", value);
            }
        }

        [Category("升级设置"), Description("升级版本信息文件URL")]
        public string UpdateVerInfoURL
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("UpdateSetting", "UpdateVerInfoURL");
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("UpdateSetting", "UpdateVerInfoURL", value);
            }
        }

    }
}
