using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public static class InsuranceData
    {
        private static List<string> _departs;

        private static List<string> _businesstype;

        public static List<string> BusinessType
        {
            get
            {
                return InsuranceData._businesstype;
            }
        }

        public static List<string> Departs
        {
            get
            {
                return InsuranceData._departs;
            }
        }

        static InsuranceData()
        {
            InsuranceData._departs = new List<string>();
            InsuranceData._businesstype = new List<string>();
            InsuranceData._businesstype.Add("busi1");
            InsuranceData._businesstype.Add("busi2");
            InsuranceData._businesstype.Add("busi3");
            InsuranceData._businesstype.Add("busi4");
            InsuranceData._businesstype.Add("busi5");
            InsuranceData._departs.Add("depart1");
            InsuranceData._departs.Add("depart2");
            InsuranceData._departs.Add("depart3");
            InsuranceData._departs.Add("depart4");
            InsuranceData._departs.Add("depart5");
        }
    }
}
