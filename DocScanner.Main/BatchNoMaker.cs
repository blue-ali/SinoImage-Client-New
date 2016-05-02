using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class BatchNoMaker
    {
        // Fields
        [ThreadStatic]
        private static BatchNoMaker _cur;
        private BatchTemplatedef _selectedtempalte;

        // Methods
        public string FromInputDialog(string param = "")
        {
            FormNewBatchNO hno = new FormNewBatchNO();
            if (hno.ShowDialog() == DialogResult.OK)
            {
                this._selectedtempalte = hno.SelectedBatchtemplate;
                return hno.BatchNO;
            }
            this._selectedtempalte = null;
            return string.Empty;
        }

        // Properties
        public static BatchNoMaker Cur
        {
            get
            {
                if (_cur == null)
                {
                    _cur = new BatchNoMaker();
                }
                return _cur;
            }
        }

        public BatchTemplatedef SelectedTemplate
        {
            get
            {
                return this._selectedtempalte;
            }
        }
    }


}
