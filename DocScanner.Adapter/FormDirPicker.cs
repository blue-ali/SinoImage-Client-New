using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DocScanner.Adapter.SharpImportDir
{
    public class FormDirPicker : FormBase
	{

		private SharpImportDirAcquirer _parent;

		private List<string> _selectedfiles = new List<string>();

		private IContainer components = null;

		private Button BtCancel;

		private Button BtSure;

		private TextBox txtMatchedExt;

		private Label LbForm;

		private Button BtBrown;

		private TextBox txtPath;

		private Label LbPath;

		public List<string> SelectedFiles
		{
			get
			{
				return this._selectedfiles;
			}
		}

		public FormDirPicker(SharpImportDirAcquirer parent)
		{
			this.InitializeComponent();
			this._parent = parent;
			this.txtPath.DataBindings.Add("Text", this._parent.GetSetting(), "InitDir").DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
			this.txtMatchedExt.DataBindings.Add("Text", this._parent.GetSetting(), "MatchedFileExtensions").DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
			this.SetKeyEscCloseForm(true);
		}

		private void BtSure_Click(object sender, EventArgs e)
		{
			this._selectedfiles.Clear();
			List<string> list = this.txtMatchedExt.Text.ToLower().Split(new char[]
			{
				';'
			}).ToList<string>();
			bool flag = !Directory.Exists(this.txtPath.Text);
			if (flag)
			{
				MessageBox.Show(this, "路径" + this.txtPath.Text + "不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.txtPath.Focus();
			}
			else
			{
                DirectoryInfo info = new DirectoryInfo(this.txtPath.Text);
                if (list != null)
                {
                    FileInfo[] files = info.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo item = files[i];
                        if ((item.Extension != null) && list.Exists(o => o == item.Extension.ToLower()))
                        {
                            this._selectedfiles.Add(item.FullName);
                        }
                    }
                }
                else
                {
                    //this._selectedfiles.AddRange(info.GetFiles().Select<NFileInfo, string>(<> c.<> 9__2_1 ?? (<> c.<> 9__2_1 = new Func<NFileInfo, string>(<> c.<> 9.< BtSure_Click > b__2_1))));
                    this._selectedfiles.AddRange(info.GetFiles().Select<FileInfo, string>(x => x.FullName));
                }
                base.DialogResult = DialogResult.OK;

            }
        }

		private void BtBrown_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this._parent.GetSetting().InitDir;
			bool flag = folderBrowserDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				bool flag2 = !folderBrowserDialog.SelectedPath.EndsWith("\\");
				if (flag2)
				{
					folderBrowserDialog.SelectedPath += "\\";
				}
				this._parent.GetSetting().InitDir = folderBrowserDialog.SelectedPath;
				this.txtPath.Text = this._parent.GetSetting().InitDir;
			}
		}

		private void BtCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
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
			this.BtCancel = new Button();
			this.BtSure = new Button();
			this.txtMatchedExt = new TextBox();
			this.LbForm = new Label();
			this.BtBrown = new Button();
			this.txtPath = new TextBox();
			this.LbPath = new Label();
			base.SuspendLayout();
			this.BtCancel.Location = new Point(238, 97);
			this.BtCancel.Name = "BtCancel";
			this.BtCancel.Size = new Size(75, 22);
			this.BtCancel.TabIndex = 23;
			this.BtCancel.Text = "取 消";
			this.BtCancel.UseVisualStyleBackColor = true;
			this.BtCancel.Click += new EventHandler(this.BtCancel_Click);
			this.BtSure.Location = new Point(117, 97);
			this.BtSure.Name = "BtSure";
			this.BtSure.Size = new Size(75, 22);
			this.BtSure.TabIndex = 22;
			this.BtSure.Text = "确 定";
			this.BtSure.UseVisualStyleBackColor = true;
			this.BtSure.Click += new EventHandler(this.BtSure_Click);
			this.txtMatchedExt.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.txtMatchedExt.Location = new Point(102, 64);
			this.txtMatchedExt.Name = "txtMatchedExt";
			this.txtMatchedExt.Size = new Size(168, 19);
			this.txtMatchedExt.TabIndex = 21;
			this.txtMatchedExt.Text = ".JPG";
			this.LbForm.AutoSize = true;
			this.LbForm.Location = new Point(35, 67);
			this.LbForm.Name = "LbForm";
			this.LbForm.Size = new Size(65, 12);
			this.LbForm.TabIndex = 20;
			this.LbForm.Text = "文件格式：";
			this.BtBrown.Location = new Point(275, 33);
			this.BtBrown.Name = "BtBrown";
			this.BtBrown.Size = new Size(22, 21);
			this.BtBrown.TabIndex = 19;
			this.BtBrown.Text = "...";
			this.BtBrown.UseVisualStyleBackColor = true;
			this.BtBrown.Click += new EventHandler(this.BtBrown_Click);
			this.txtPath.Location = new Point(102, 33);
			this.txtPath.Name = "txtPath";
			this.txtPath.ReadOnly = true;
			this.txtPath.Size = new Size(168, 21);
			this.txtPath.TabIndex = 18;
			this.txtPath.Text = "C:\\";
			this.LbPath.AutoSize = true;
			this.LbPath.Location = new Point(33, 35);
			this.LbPath.Name = "LbPath";
			this.LbPath.Size = new Size(65, 12);
			this.LbPath.TabIndex = 17;
			this.LbPath.Text = "文件路径：";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(354, 143);
			base.Controls.Add(this.BtCancel);
			base.Controls.Add(this.BtSure);
			base.Controls.Add(this.txtMatchedExt);
			base.Controls.Add(this.LbForm);
			base.Controls.Add(this.BtBrown);
			base.Controls.Add(this.txtPath);
			base.Controls.Add(this.LbPath);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Margin = new Padding(2, 2, 2, 2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormDirPicker";
			this.Text = "目录浏览";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
