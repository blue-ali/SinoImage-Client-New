using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class InsureUserInfo
    {
        [NonSerialized]
        private Bitmap _signatureimg;

        [Category("详细信息"), DisplayName("用户名")]
        public string Name
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("证件号码")]
        public string IDNO
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("联系电话")]
        public string Telphone
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("地址")]
        public string Address
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("邮编")]
        public string PostCode
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("备注")]
        public string Comment
        {
            get;
            set;
        }

        [Category("详细信息"), DisplayName("文件名称")]
        public string SignatureImgName
        {
            get;
            set;
        }

        [Browsable(false)]
        public Bitmap SignatureImg
        {
            get
            {
                return this._signatureimg;
            }
            set
            {
                this._signatureimg = value;
            }
        }
    }
}
