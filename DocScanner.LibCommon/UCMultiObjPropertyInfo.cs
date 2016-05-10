using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    /// <summary>
    /// 参数--参数设置界面
    /// </summary>
    public class UCMultiObjPropertyInfo : UserControl
    {
        // Fields
        private Dictionary<string, object> _map = new Dictionary<string, object>();
        private Button btn_Close;
        private ComboBox comboBox_Objs;
        private IContainer components = null;
        private PropertyGrid propertyGrid1;
        private TableLayoutPanel tableLayoutPanel1;

        // Methods
        public UCMultiObjPropertyInfo()
        {
            this.InitializeComponent();
            this.comboBox_Objs.SelectedIndexChanged += new EventHandler(this.comboBox_Objs_SelectedIndexChanged);
        }

        public void AddObjs(string txt, object obs)
        {
            this._map[txt] = obs;
            this.comboBox_Objs.Items.Add(txt);
            this.comboBox_Objs.SelectedIndex = 0;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            for (Control control = base.Parent; control != null; control = control.Parent)
            {
                if (control is Form)
                {
                    (control as Form).Close();
                    break;
                }
            }
        }

        private void comboBox_Objs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = this._map[this.comboBox_Objs.SelectedItem.ToString()];
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBox_Objs = new ComboBox();
            this.propertyGrid1 = new PropertyGrid();
            this.btn_Close = new Button();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.comboBox_Objs.Anchor = AnchorStyles.Left;
            this.comboBox_Objs.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Objs.FormattingEnabled = true;
            this.comboBox_Objs.Location = new Point(4, 0x17);
            this.comboBox_Objs.Margin = new Padding(4, 3, 4, 3);
            this.comboBox_Objs.Name = "comboBox_Objs";
            this.comboBox_Objs.Size = new Size(0x19d, 0x17);
            this.comboBox_Objs.TabIndex = 0;
            this.propertyGrid1.CategoryForeColor = SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = DockStyle.Fill;
            this.propertyGrid1.Location = new Point(4, 0x48);
            this.propertyGrid1.Margin = new Padding(4, 3, 4, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(0x2a9, 0x1e7);
            this.propertyGrid1.TabIndex = 1;
            this.btn_Close.Location = new Point(4, 0x235);
            this.btn_Close.Margin = new Padding(4, 3, 4, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new Size(100, 0x1b);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_Close, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Objs, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 69f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 81f));
            this.tableLayoutPanel1.Size = new Size(0x2b1, 0x283);
            this.tableLayoutPanel1.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Margin = new Padding(4, 3, 4, 3);
            base.Name = "UCMultiObjPropertyInfo";
            base.Size = new Size(0x2b1, 0x283);
            this.tableLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        // Properties
        public string Title
        {
            get
            {
                return "程序对象属性浏览(可直接修改-如果属性直接关联配置文件，则会直接修改配置文件信息)";
            }
        }
    }

}
