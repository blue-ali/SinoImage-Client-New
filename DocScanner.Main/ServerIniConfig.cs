using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace DocScanner.Main
{
    public class ServerIniConfig : AbstractSetting<ServerIniConfig>, IPropertiesSetting
    {
        private const string DBSETTING = "DBSETTING";

        private const string DBDRIVER = "DBDRIVER";

        private const string DBName = "DBNAME";

        private const string DBUser = "DBUSER";

        private const string DBPwd = "DBPWD";

        private const string SERVERSETTING = "SERVERSETTING";

        private const string CACHEDIR = "CACHEDIR";

        private const string STOREDIR = "STOREDIR";

        private const string SUPPORTHISVER = "SUPPORTHISVER";

        private const string REPLYINCDATA = "REPLYINCDATA";

        private const string SERVERLIST = "SERVERLIST";

        private const string DOWNLOADWEBDIR = "DOWNLOADWEBDIR";

        private const string UPDATEFILEDIR = "UPDATEFILEDIR";

        private const string TOMCATDIR = "TOMCATDIR";

        private IniConfigSetting _servercfg;

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string TomcatPath
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "TomcatPath");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "TomcatPath", value.ToString());
            }
        }

        public string TomcatDir
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "TOMCATDIR");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "TOMCATDIR", value.ToString());
            }
        }

        public string DownloadWebDir
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "DOWNLOADWEBDIR");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "DOWNLOADWEBDIR", value.ToString());
            }
        }

        public string ServerList
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "SERVERLIST");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "SERVERLIST", value.ToString());
            }
        }

        public bool ReplyINCDATA
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "REPLYINCDATA").ToBool();
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "REPLYINCDATA", value.ToString());
            }
        }

        public bool SupportHISVER
        {
            get
            {
                return this.GetCfgValue("DBSETTING", "DBDRIVER").ToBool();
            }
            set
            {
                this.SetCfgValue("DBSETTING", "DBDRIVER", value.ToString());
            }
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string DataBaseDriverName
        {
            get
            {
                return this.GetCfgValue("DBSETTING", "DBDRIVER");
            }
            set
            {
                this.SetCfgValue("DBSETTING", "DBDRIVER", value);
            }
        }

        public string DataBaseName
        {
            get
            {
                return this.GetCfgValue("DBSETTING", "DBNAME");
            }
            set
            {
                this.SetCfgValue("DBSETTING", "DBNAME", value);
            }
        }

        public string DataBaseUser
        {
            get
            {
                return this.GetCfgValue("DBSETTING", "DBUSER");
            }
            set
            {
                this.SetCfgValue("DBSETTING", "DBUSER", value);
            }
        }

        public string DataBasePassword
        {
            get
            {
                return this.GetCfgValue("DBSETTING", "DBPWD");
            }
            set
            {
                this.SetCfgValue("DBSETTING", "DBPWD", value);
            }
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string CacheDir
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "CACHEDIR");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "CACHEDIR", value);
            }
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string StoreDir
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "STOREDIR");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "STOREDIR", value);
            }
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string UpdateFileDir
        {
            get
            {
                return this.GetCfgValue("SERVERSETTING", "UPDATEFILEDIR");
            }
            set
            {
                this.SetCfgValue("SERVERSETTING", "UPDATEFILEDIR", value);
            }
        }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string _ServerSettingIniPath
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("ServerIniSetting", "ServerSettingIniPath");
            }
            set
            {
                bool flag = this._ServerSettingIniPath != value;
                if (flag)
                {
                    this._servercfg = IniConfigSetting.CreateNew();
                    bool flag2 = File.Exists(value);
                    if (flag2)
                    {
                        this._servercfg.ConfigFileName = value;
                    }
                    IniConfigSetting.Cur.SetConfigParamValue("ServerIniSetting", "ServerSettingIniPath", value.ToString());
                }
            }
        }

        public override string Name
        {
            get
            {
                return "服务器Ini配置文件设置";
            }
        }

        public ServerIniConfig()
        {
            this._servercfg = IniConfigSetting.CreateNew();
            bool flag = File.Exists(this._ServerSettingIniPath);
            if (flag)
            {
                this._servercfg.ConfigFileName = this._ServerSettingIniPath;
            }
        }

        private string GetCfgValue(string sec, string key)
        {
            bool flag = this._servercfg != null;
            string result;
            if (flag)
            {
                result = this._servercfg.GetConfigParamValue(sec, key);
            }
            else
            {
                result = "";
            }
            return result;
        }

        private void SetCfgValue(string sec, string key, string val)
        {
            bool flag = this._servercfg != null;
            if (flag)
            {
                this._servercfg.SetConfigParamValue(sec, key, val);
            }
        }

        public override bool Equals(ServerIniConfig other)
        {
            throw new NotImplementedException();
        }
    }
}
