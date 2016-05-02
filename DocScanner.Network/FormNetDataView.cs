using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DocScaner.Network
{
	public class FormNetDataView : Form
	{
		private static FormNetDataView _form;

		private IContainer components = null;

		private PropertyGrid propertyGrid1;

		private TextBox textBox1;

		private CheckBox checkBox1;

		public bool BlockThread
		{
			get;
			set;
		}

		public object SelObject
		{
			set
			{
				this.propertyGrid1.SelectedObject = value;
			}
		}

		public FormNetDataView()
		{
			this.InitializeComponent();
			this.checkBox1.DataBindings.Add("checked", this, "BlockThread");
		}

		public static void ViewTransferData(object data)
		{
			bool flag = FormNetDataView._form == null || FormNetDataView._form.IsDisposed;
			if (flag)
			{
				FormNetDataView._form = new FormNetDataView();
				FormNetDataView._form.TopLevel = true;
				FormNetDataView._form.SelObject = data;
				FormNetDataView._form.Show();
			}
			FormNetDataView._form.SelObject = data;
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
			this.propertyGrid1 = new PropertyGrid();
			this.textBox1 = new TextBox();
			this.checkBox1 = new CheckBox();
			base.SuspendLayout();
			this.propertyGrid1.CategoryForeColor = SystemColors.InactiveCaptionText;
			this.propertyGrid1.Location = new Point(70, 29);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new Size(765, 395);
			this.propertyGrid1.TabIndex = 0;
			this.textBox1.Location = new Point(60, 468);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(775, 132);
			this.textBox1.TabIndex = 1;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(275, 28);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(117, 19);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "BlockThread";
			this.checkBox1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(947, 627);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.propertyGrid1);
			base.Name = "FormNetDataView";
			this.Text = "FormNetDataView";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
