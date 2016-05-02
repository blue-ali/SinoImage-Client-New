using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public static class RichBoxHelper
    {
        // Methods
        public static void PasteImage(this RichTextBox box, Image image)
        {
            Clipboard.SetImage(image);
            box.Paste();
        }

        public static void PasteText(this RichTextBox box, string text)
        {
            Clipboard.SetText(text);
            box.Paste();
        }
    }

}
