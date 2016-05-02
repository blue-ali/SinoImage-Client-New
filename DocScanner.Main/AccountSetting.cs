using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.ComponentModel;

namespace DocScanner.Main
{
    public class AccountSetting : AbstractSetting<AccountSetting>, IPropertiesSetting
    {
        private string _curorgid;

        private string _curuser;

        public const string bindAccountName = "AccountName";

        public const string bindAccountOrgID = "AccountOrgID";

        [Browsable(false)]
        public override string Name
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
                bool flag = string.IsNullOrEmpty(this._curuser);
                if (flag)
                {
                    this._curuser = LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("AccountSetting", "AccountName");
                }
                return this._curuser;
            }
            set
            {
                bool flag = value != this._curuser;
                if (flag)
                {
                    this._curuser = value;
                    LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("AccountSetting", "AccountName", this._curuser);
                }
            }
        }

        [Category("账户设置"), Description("当前机构")]
        public string AccountOrgID
        {
            get
            {
                bool flag = string.IsNullOrEmpty(this._curorgid);
                if (flag)
                {
                    this._curorgid = LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("AccountSetting", "AccountOrgID");
                }
                return this._curorgid;
            }
            set
            {
                bool flag = value != this._curorgid;
                if (flag)
                {
                    this._curorgid = value;
                    LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("AccountSetting", "AccountOrgID", this._curorgid);
                }
            }
        }

        public object MyClone()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(AccountSetting other)
        {
            return false;
        }
    }
}
