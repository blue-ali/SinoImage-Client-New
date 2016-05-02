using System;
using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public partial class FormContainer : FormBase
    {
        // Fields
        private Control _holdctrl;

        // Methods

        public FormContainer(Control Parent)
        {
            this.components = null;
            this.InitializeComponent();
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
        }

        public FormContainer()
        {
            this.components = null;
            this.InitializeComponent();
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
        }

        public Control GetControl()
        {
            return this._holdctrl;
        }

        private Size GetCtrlSize()
        {
            if (this._holdctrl != null)
            {
                object propertyValue = ReflectHelper.GetPropertyValue(this._holdctrl, "Size");
                if (propertyValue != null)
                {
                    return (Size)propertyValue;
                }
            }
            return new Size(0, 0);
        }

        public bool SetControl(Type ctltype)
        {
            Control ctrl = (Control)ReflectHelper.Construct(ctltype);
            return this.SetControl(ctrl);
        }

        public bool SetControl(Control ctrl)
        {
            this._holdctrl = ctrl;
            base.SuspendLayout();
            this._holdctrl.Dock = DockStyle.Fill;
            base.Controls.Add(this._holdctrl);
            string ctrlTitle = FormHelper.GetCtrlTitle(ctrl);
            if (!string.IsNullOrEmpty(ctrlTitle))
            {
                this.Text = ctrlTitle;
            }
            Size ctrlSize = this.GetCtrlSize();
            if (ctrlSize != new Size(0, 0))
            {
                base.ClientSize = ctrlSize;
            }
            base.ResumeLayout(false);
            return true;
        }
    }

}