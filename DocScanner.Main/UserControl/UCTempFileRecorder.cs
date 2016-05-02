using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCTempFileRecorder : UserControl
    {
        private IContainer components = null;

        private DataGridView dataGridView1;

        private Button btn_Del;

        private DataGridViewCheckBoxColumn ColumnToDel;

        private DataGridViewTextBoxColumn ColumnFilePath;

        private DataGridViewTextBoxColumn ColumnCreateTime;

        public string Title
        {
            get
            {
                return "临时文件清理";
            }
        }

        public UCTempFileRecorder()
        {
            this.InitializeComponent();
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
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
            this.dataGridView1 = new DataGridView();
            this.btn_Del = new Button();
            this.ColumnToDel = new DataGridViewCheckBoxColumn();
            this.ColumnFilePath = new DataGridViewTextBoxColumn();
            this.ColumnCreateTime = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            base.SuspendLayout();
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                this.ColumnToDel,
                this.ColumnFilePath,
                this.ColumnCreateTime
            });
            this.dataGridView1.Location = new Point(23, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new Size(662, 374);
            this.dataGridView1.TabIndex = 0;
            this.btn_Del.Location = new Point(472, 422);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new Size(163, 56);
            this.btn_Del.TabIndex = 1;
            this.btn_Del.Text = "删除";
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.Click += new EventHandler(this.btn_Del_Click);
            this.ColumnToDel.HeaderText = "";
            this.ColumnToDel.Name = "ColumnToDel";
            this.ColumnFilePath.HeaderText = "Path";
            this.ColumnFilePath.Name = "ColumnFilePath";
            this.ColumnFilePath.Resizable = DataGridViewTriState.True;
            this.ColumnFilePath.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ColumnCreateTime.HeaderText = "时间";
            this.ColumnCreateTime.Name = "ColumnCreateTime";
            this.ColumnCreateTime.Resizable = DataGridViewTriState.True;
            this.ColumnCreateTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btn_Del);
            base.Controls.Add(this.dataGridView1);
            base.Name = "UCTempFileRecorder";
            base.Size = new Size(734, 526);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            base.ResumeLayout(false);
        }
    }
}
