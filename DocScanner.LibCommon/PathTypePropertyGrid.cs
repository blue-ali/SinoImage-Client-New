using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Logos.DocScaner.Common
{
    public class PathTypeAttribute : Attribute
    {
        public string RootPath
        {
            get;
            set;
        }
    }

    public class PathTypePropertyGrid
    {
    }

    public class PathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;
            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    FolderBrowserDialog dirDlg = new FolderBrowserDialog();
                    dirDlg.SelectedPath = value.ToString();
                    if (dirDlg.ShowDialog() == DialogResult.OK)
                    {
                        value = dirDlg.SelectedPath;
                    }
                }
            }
            return value;
        }
    }
}