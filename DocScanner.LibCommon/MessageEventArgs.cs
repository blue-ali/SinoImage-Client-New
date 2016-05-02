using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class MessageEventArgs : EventArgs
    {
        // Fields
        public EMessageType MsgType { get; set; }

        // Methods
        public MessageEventArgs(EMessageType type, string msg)
        {
            this.MsgType = type;
            this.Msg = msg;
        }

        // Properties
        public string Msg { get; set; }

        
    }


}
