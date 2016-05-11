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
    public class BusinessSetting : AbstractSetting<BusinessSetting>, IPropertiesSetting
    {
        private string _busno = "";

        private string _bustype = "";

        [Browsable(false)]
        public string busno
        {
            get
            {
                return this._busno;
            }
            set
            {
                this._busno = value;
            }
        }

        [Browsable(false)]
        public override string Name
        {
            get
            {
                return "业务参数设置";
            }
        }

        [Browsable(false)]
        public string bustype
        {
            get
            {
                return this._bustype;
            }
            set
            {
                this._bustype = value;
            }
        }

        public override bool Equals(BusinessSetting other)
        {
            throw new NotImplementedException();
        }
    }
}
