using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    public class MailHelper
    {
        public static string Sender
        {
            get
            {
                return "";
            }
        }

        static MailHelper()
        {
        }

        public static void SendMail(string topic, string body, string attatchment)
        {
        }
    }
}
