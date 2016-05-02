using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class FormMgr
    {
        private Dictionary<Type, Form> _formmgr = new Dictionary<Type, Form>();

        private FormContainer Create(UserControl uc)
        {
            return new FormContainer();
        }
    }
}
