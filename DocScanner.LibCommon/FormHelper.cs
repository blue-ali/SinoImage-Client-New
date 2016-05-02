using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public static class FormHelper
    {
        // Methods
        public static string GetCtrlTitle(Control _holdctrl)
        {
            if (_holdctrl != null)
            {
                object propertyValue = ReflectHelper.GetPropertyValue(_holdctrl, "Title");
                if (propertyValue != null)
                {
                    return (string)propertyValue;
                }
                propertyValue = ReflectHelper.GetPropertyValue(_holdctrl, "Text");
                if (propertyValue != null)
                {
                    return (string)propertyValue;
                }
            }
            return "";
        }

        public static Form GetHostForm(this Control ctl)
        {
            Control parent = ctl;
            while (!(parent is Form) && (parent != null))
            {
                parent = parent.Parent;
            }
            return (parent as Form);
        }

        public static void LeftMouseMoveForm(this Control form)
        {
            Point mouseOff = new Point();
            bool leftFlag = false;
            form.MouseDown += delegate (object sender, MouseEventArgs e) {
                if (e.Button == MouseButtons.Left)
                {
                    mouseOff = new Point(-e.X, -e.Y);
                    leftFlag = true;
                }
            };
            form.MouseMove += delegate (object sender, MouseEventArgs e) {
                if (leftFlag)
                {
                    Point mousePosition = Control.MousePosition;
                    mousePosition.Offset(mouseOff.X, mouseOff.Y);
                    Form hostForm = form.GetHostForm();
                    if (hostForm != null)
                    {
                        hostForm.Location = mousePosition;
                    }
                }
            };
            form.MouseUp += delegate (object sender, MouseEventArgs e) {
                if (leftFlag)
                {
                    leftFlag = false;
                }
            };
        }

        public static void SetKeyEscCloseForm(this Form form, bool IncludeAllCtrols = true)
        {
            //KeyEventHandler <> 9__1;
            form.KeyDown += delegate (object sender, KeyEventArgs e) {
                if (e.KeyCode == Keys.Escape)
                {
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                }
            };
            foreach (Control control in form.Controls)
            {
                //control.KeyDown += (<> 9__1 ?? (<> 9__1 = delegate (object sender, KeyEventArgs e) {
                control.KeyDown +=  delegate (object sender, KeyEventArgs e) {
                    if (e.KeyCode == Keys.Escape)
                    {
                        form.DialogResult = DialogResult.Cancel;
                        form.Close();
                    }
                };
            }
        }
    }

}