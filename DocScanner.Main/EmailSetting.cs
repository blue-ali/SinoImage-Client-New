using DocScanner.Bean;
using DocScanner.Common;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class EmailSetting : AbstractSetting<EmailSetting>, IPropertiesSetting
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return "邮件设置";
            }
        }

        [Category("邮件设置"), DisplayName("允许email汇报bug")]
        public bool EnableReportBugFromEmail
        {
            get;
            set;
        }

        [Category("邮件设置"), DisplayName("账户")]
        public string MailAccount
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("EmailSetting", "EmailSender");
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("EmailSetting", "EmailSender", value);
            }
        }

        [Category("邮件设置"), DisplayName("密码")]
        public string MailPassword
        {
            get
            {
                string configParamValue = IniConfigSetting.Cur.GetConfigParamValue("EmailSetting", "EmailPwd");
                bool flag = string.IsNullOrEmpty(configParamValue);
                string result;
                if (flag)
                {
                    result = "";
                }
                else
                {
                    result = "******";
                }
                return result;
            }
            set
            {
                value = EncryptUtils.Base64Encrypt(value);
                IniConfigSetting.Cur.SetConfigParamValue("EmailSetting", "EmailPwd", value);
            }
        }

        [Category("邮件设置"), DisplayName("邮件服务器")]
        public string MailServer
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("EmailSetting", "EmailHost");
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("EmailSetting", "EmailHost", value);
            }
        }

        [Browsable(false), Category("邮件设置"), DisplayName("邮件服务器端口")]
        public int MailServerPort
        {
            get;
            set;
        }

        [Category("邮件设置"), DisplayName("收件人")]
        public string Recieptant
        {
            get
            {
                return IniConfigSetting.Cur.GetConfigParamValue("EmailSetting", "EmailReciever");
            }
            set
            {
                IniConfigSetting.Cur.SetConfigParamValue("EmailSetting", "EmailReciever", value);
            }
        }

        public string GetMailPassword()
        {
            string configParamValue = IniConfigSetting.Cur.GetConfigParamValue("EmailSetting", "EmailPwd");
            return EncryptUtils.Base64Decrypt(configParamValue);
        }

        public override bool Equals(EmailSetting other)
        {
            return false;
        }
    }
}
