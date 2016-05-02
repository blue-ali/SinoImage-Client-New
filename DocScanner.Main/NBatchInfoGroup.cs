using DocScanner.Bean;
using DocScanner.Bean.pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class NBatchInfoGroup
    {
        private List<NBatchInfo> _batchs = new List<NBatchInfo>();

        public List<NBatchInfo> Batchs
        {
            get
            {
                return this._batchs;
            }
            set
            {
                this._batchs = value;
            }
        }

        public NBatchInfoGroup MyClone()
        {
            NBatchInfoGroup nBatchInfoGroup = new NBatchInfoGroup();
            foreach (NBatchInfo current in this.Batchs)
            {
                nBatchInfoGroup.Batchs.Add(current.MyClone());
            }
            return nBatchInfoGroup;
        }

        public void Update2NoneMode()
        {
            foreach (NBatchInfo current in this._batchs)
            {
                current.Operation = EOperType.eFROM_SERVER_NOTCHANGE;
                foreach (NFileInfo current2 in current.FileInfos)
                {
                    current2.Operation = EOperType.eFROM_SERVER_NOTCHANGE;
                    foreach (NNoteInfo current3 in current2.NotesList)
                    {
                        current3.Operation = EOperType.eFROM_SERVER_NOTCHANGE;
                    }
                }
            }
        }
    }
}
