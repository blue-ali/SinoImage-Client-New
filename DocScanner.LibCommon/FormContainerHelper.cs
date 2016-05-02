using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public static class FormContainerHelper
    {
        // Methods
        public static DialogResult ShowInContainer(this UserControl control)
        {
            FormContainer container = new FormContainer();
            container.SetControl(control);
            return container.ShowDialog();
        }
    }

}
