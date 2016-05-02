using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public static class InvoiceChecker
    {
        // Fields
        public static string WebCheckURL
        {
            get;
            set;
        }

        // Methods
        public static bool Check(string fpdm, string fphm, string pwd = null)
        {
            if ((string.IsNullOrEmpty(fpdm) || (fpdm.Length != 12)) || !IsDigitStr(fpdm))
            {
                return false;
            }
            if ((string.IsNullOrEmpty(fphm) || (fphm.Length != 8)) || !IsDigitStr(fphm))
            {
                return false;
            }
            return true;
        }

        private static bool IsDigitStr(string input)
        {
            foreach (char ch in input)
            {
                if ((ch < '0') || (ch > '9'))
                {
                    return false;
                }
            }
            return true;
        }

    }

}
