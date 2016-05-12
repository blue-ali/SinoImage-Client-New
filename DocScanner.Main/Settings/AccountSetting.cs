using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.ComponentModel;

namespace DocScanner.Main
{
    public class AccountSetting : IPropertiesSetting
    {
        private static readonly AccountSetting instance = new AccountSetting();

        private string _currentOrgId;

        private string _currentUser;

        private AccountSetting() { }

        public static AccountSetting GetInstance()
        {
            return instance;
        }

        [Browsable(false)]
        public string Name
        {
            get
            {
                return "账户设置";
            }
        }

        [Category("账户设置"), Description("当前用户")]
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(this._currentUser))
                {
                    this._currentUser = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue(AppContext.AccountTitle, AppContext.AccountName);
                }
                return this._currentUser;
            }
            set
            {
                if (value != this._currentUser)
                {
                    this._currentUser = value;
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AccountSetting", "AccountName", this._currentUser);
                }
            }
        }

        [Category("账户设置"), Description("当前机构")]
        public string AccountOrgID
        {
            get
            {
                if (string.IsNullOrEmpty(this._currentOrgId))
                {
                    this._currentOrgId = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AccountSetting", "AccountOrgID");
                }
                return this._currentOrgId;
            }
            set
            {
                if (value != this._currentOrgId)
                {
                    this._currentOrgId = value;
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AccountSetting", "AccountOrgID", this._currentOrgId);
                }
            }
        }

    }
}
