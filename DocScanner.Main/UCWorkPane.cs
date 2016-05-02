using DocScanner.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCWorkPane : Panel
    {
        private UCBillInfoEdit _ucbillinfoedit;

        private NFileInfo _curfileinfo;

        public NFileInfo CurFileInfo
        {
            get
            {
                return this._curfileinfo;
            }
            set
            {
                bool flag = this._curfileinfo != value;
                if (flag)
                {
                    this._curfileinfo = value;
                    bool flag2 = this._ucbillinfoedit != null;
                    if (flag2)
                    {
                        this._ucbillinfoedit.CurFileInfo = value;
                    }
                }
            }
        }

        public UCWorkPane()
        {
            this.AddUC2WorkPane(null);
        }

        public void AddUC2WorkPane(UserControl uc)
        {
            this._ucbillinfoedit = new UCBillInfoEdit();
            base.Controls.Add(this._ucbillinfoedit);
            this._ucbillinfoedit.Dock = DockStyle.Fill;
        }
    }
}
