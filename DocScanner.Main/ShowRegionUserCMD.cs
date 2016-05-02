using DocScanner.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class ShowRegionUserCMD
    {
        private NNoteInfo _note;

        public NNoteInfo Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
            }
        }
    }
}
