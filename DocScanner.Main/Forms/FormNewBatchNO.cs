using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class FormNewBatchNO : FormBase
    {


    private IContainer components = null;

    private Button btn_OK;

    private Label label1;

    private TextBox textBox_BatchNO;

    private Button button1;

    private ComboBox comboBoxTemplate;

    private Label label2;

    private Button btnTempalteEdit;

    public string BatchNO
    {
        get
        {
            return this.textBox_BatchNO.Text;
        }
        set
        {
            this.textBox_BatchNO.Text = value;
        }
    }

    internal BatchTemplatedef SelectedBatchtemplate
    {
        get
        {
            BatchTemplatedef result;
            if (!string.IsNullOrEmpty(this.comboBoxTemplate.Text))
            {
                result = BatchTemplateMgr.GetTempalte(this.comboBoxTemplate.Text);
            }
            else
            {
                result = null;
            }
            return result;
        }
    }

    public FormNewBatchNO()
    {
        this.InitializeComponent();
        this.Text = "新建批次号";
        base.Load += delegate (object sender, EventArgs e)
        {
            this.textBox_BatchNO.Focus();
        };
        this.textBox_BatchNO.KeyUp += new KeyEventHandler(this.TextBox_BatchNO_KeyUp);
        this.setuptemplateui();
    }

    private void setuptemplateui()
    {
        bool allowTempalte = FunctionSetting.GetInstance().AllowTempalte;
        this.label2.Visible = allowTempalte;
        this.comboBoxTemplate.Visible = allowTempalte;
        this.btnTempalteEdit.Visible = allowTempalte;
        this.comboBoxTemplate.Items.Clear();
            if (allowTempalte)
            {
                this.comboBoxTemplate.Items.AddRange(BatchTemplateMgr.GetTemplates().Select<BatchTemplatedef, string>(o => o.Name).ToArray<string>());
            }

        }

    private void TextBox_BatchNO_KeyUp(object sender, KeyEventArgs e)
    {
        bool flag = e.KeyCode == Keys.Return;
        if (flag)
        {
            this.btn_OK_Click(this, EventArgs.Empty);
        }
        bool flag2 = e.KeyCode == Keys.Escape;
        if (flag2)
        {
            this.btn_Cancel_Click(this, EventArgs.Empty);
        }
    }

    private void btn_OK_Click(object sender, EventArgs e)
    {
        bool flag = string.IsNullOrEmpty(this.textBox_BatchNO.Text.Trim());
        if (!flag)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }

    private void btn_Cancel_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.Cancel;
        base.Close();
    }

    private void btnTempalteEdit_Click(object sender, EventArgs e)
    {
        UCBatchTemplateEdit control = new UCBatchTemplateEdit();
        FormContainer formContainer = new FormContainer();
        formContainer.SetControl(control);
        formContainer.ShowDialog();
        this.setuptemplateui();
    }

    protected override void Dispose(bool disposing)
    {
        bool flag = disposing && this.components != null;
        if (flag)
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.btn_OK = new Button();
        this.label1 = new Label();
        this.textBox_BatchNO = new TextBox();
        this.button1 = new Button();
        this.comboBoxTemplate = new ComboBox();
        this.label2 = new Label();
        this.btnTempalteEdit = new Button();
        base.SuspendLayout();
        this.btn_OK.Location = new Point(84, 198);
        this.btn_OK.Name = "btn_OK";
        this.btn_OK.Size = new Size(83, 25);
        this.btn_OK.TabIndex = 6;
        this.btn_OK.Text = "确定&O";
        this.btn_OK.UseVisualStyleBackColor = true;
        this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 64);
        this.label1.Name = "label1";
        this.label1.Size = new Size(52, 15);
        this.label1.TabIndex = 0;
        this.label1.Text = "批次号";
        this.textBox_BatchNO.Location = new Point(77, 59);
        this.textBox_BatchNO.Name = "textBox_BatchNO";
        this.textBox_BatchNO.Size = new Size(348, 25);
        this.textBox_BatchNO.TabIndex = 1;
        this.button1.Location = new Point(298, 198);
        this.button1.Name = "button1";
        this.button1.Size = new Size(83, 25);
        this.button1.TabIndex = 7;
        this.button1.Text = "取消&C";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new EventHandler(this.btn_Cancel_Click);
        this.comboBoxTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
        this.comboBoxTemplate.FormattingEnabled = true;
        this.comboBoxTemplate.Location = new Point(77, 138);
        this.comboBoxTemplate.Name = "comboBoxTemplate";
        this.comboBoxTemplate.Size = new Size(168, 23);
        this.comboBoxTemplate.TabIndex = 4;
        this.label2.AutoSize = true;
        this.label2.Location = new Point(27, 142);
        this.label2.Name = "label2";
        this.label2.Size = new Size(37, 15);
        this.label2.TabIndex = 7;
        this.label2.Text = "模版";
        this.btnTempalteEdit.Location = new Point(251, 138);
        this.btnTempalteEdit.Name = "btnTempalteEdit";
        this.btnTempalteEdit.Size = new Size(34, 23);
        this.btnTempalteEdit.TabIndex = 5;
        this.btnTempalteEdit.Text = "...";
        this.btnTempalteEdit.UseVisualStyleBackColor = true;
        this.btnTempalteEdit.Click += new EventHandler(this.btnTempalteEdit_Click);
        base.AutoScaleDimensions = new SizeF(8f, 15f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(497, 270);
        base.Controls.Add(this.btnTempalteEdit);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.comboBoxTemplate);
        base.Controls.Add(this.button1);
        base.Controls.Add(this.textBox_BatchNO);
        base.Controls.Add(this.label1);
        base.Controls.Add(this.btn_OK);
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "FormNewBatchNO";
        this.Text = "FormNewBatchNOcs";
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}
}
