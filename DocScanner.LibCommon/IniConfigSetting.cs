using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DocScanner.LibCommon
{
    public class IniConfigSetting
    {
        // Fields
        private Dictionary<Tuple<string, string>, string> _cache = new Dictionary<Tuple<string, string>, string>();
        private string _cfgname;
        private static IniConfigSetting _cur;

        // Methods
        protected IniConfigSetting()
        {
        }

        public static IniConfigSetting CreateNew()
        {
            return new IniConfigSetting();
        }

        public static string GetCfgValueFromFile(string ConFigName, string sesstion, string key)
        {
            string str;
            try
            {
                StringBuilder retVal = new StringBuilder(0x400);
                GetPrivateProfileString(sesstion, key, "", retVal, 0x400, ConFigName);
                str = retVal.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception("读取系统参数时出现异常，" + exception);
            }
            return str;
        }

        public string GetConfigParamValue(string section, string key)
        {
            Tuple<string, string> tuple = new Tuple<string, string>(section, key);
            if (this._cache.ContainsKey(tuple))
            {
                return this._cache[tuple];
            }
            string str3 = this.RGetConfigParamValue(section, key);
            this._cache[tuple] = str3;
            return str3;
        }

        public string GetConfigParamValue(string section, string key, string defaultVal)
        {
            string configParamValue = this.GetConfigParamValue(section, key);
            if (string.IsNullOrEmpty(configParamValue))
            {
                return defaultVal;
            }
            return configParamValue;
        }

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private string RGetConfigParamValue(string section, string key)
        {
            return GetCfgValueFromFile(this.ConfigFileName, section, key);
        }

        private void RSetConfigParamValue(string section, string key, string value)
        {
            SetConfigParamValueToFile(this.ConfigFileName, section, key, value);
        }

        public void SetConfigParamValue(string section, string key, string value)
        {
            Tuple<string, string> tuple = new Tuple<string, string>(section, key);
            if (!this._cache.ContainsKey(tuple) || (this._cache[tuple] != value))
            {
                this._cache[tuple] = value;
                this.RSetConfigParamValue(section, key, value);
            }
        }

        public static void SetConfigParamValueToFile(string ConFigName, string paramType, string paramName, string value)
        {
            try
            {
                WritePrivateProfileString(paramType, paramName, value, ConFigName);
            }
            catch (Exception exception)
            {
                throw new Exception("设置系统参数时出现异常，" + exception);
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // Properties
        public string ConfigFileName
        {
            get
            {
                return this._cfgname;
            }
            set
            {
                this._cfgname = Path.GetFullPath(value);
            }
        }

        public static IniConfigSetting Cur
        {
            get
            {
                if (_cur == null)
                {
                    _cur = new IniConfigSetting();
                }
                return _cur;
            }
        }
    }


}