using DocScanner.Bean;
using DocScanner.LibCommon.Util;
using DocScanner.Main.UC;
using DocScanner.OCR;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCBillInfoEdit : UserControl
    {
        private NFileInfo _curinfo;

        private IContainer components = null;

        private DataGridView dataGridView1;

        private Button btnFapiaoCheck;

        private DataGridViewTextBoxColumn ColumnItemName;

        private DataGridViewTextBoxColumn ColumnItemValue;

        private DataGridViewComboBoxColumn ColumnItemOCRType;

        private DataGridViewButtonColumn ColumnItemOCR;

        public NFileInfo CurFileInfo
        {
            get
            {
                return this._curinfo;
            }
            set
            {
                bool flag = this._curinfo != value;
                if (flag)
                {
                    this._curinfo = value;
                }
                bool flag2 = this._curinfo != null;
                if (flag2)
                {
                    this.dataGridView1.Rows[0].Cells[1].Value = this._curinfo.ExFaPiaoCode;
                }
                else
                {
                    this.dataGridView1.Rows[0].Cells[1].Value = null;
                }
            }
        }

        public string Title
        {
            get
            {
                return "票据信息";
            }
        }

        public UCBillInfoEdit()
        {
            this.InitializeComponent();
            this.dataGridView1.CellContentClick += new DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[0].Cells[0].Value = "发票代码";
            this.dataGridView1.Rows[1].Cells[0].Value = "发票号码";
            this.dataGridView1.Rows[2].Cells[0].Value = "税务登记号";
            this.ColumnItemOCRType.Items.AddRange(Enum.GetNames(typeof(CnOCRType)).ToArray<string>());
            this.dataGridView1.Rows[0].Cells[2].Value = CnOCRType.数字.ToString();
            this.dataGridView1.Rows[1].Cells[2].Value = CnOCRType.数字.ToString();
            this.dataGridView1.Rows[2].Cells[2].Value = CnOCRType.数字.ToString();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.DataGridView1_EditingControlShowing);
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            bool flag = e.Control is DataGridViewTextBoxEditingControl;
            if (flag)
            {
                TextBox textBox = (DataGridViewTextBoxEditingControl)e.Control;
                textBox.TextChanged += delegate (object nsender, EventArgs ne)
                {
                    try
                    {
                        string text = ((TextBox)nsender).Text;
                        bool flag2 = this._curinfo != null;
                        if (flag2)
                        {
                            this._curinfo.ExFaPiaoCode = text;
                        }
                    }
                    catch
                    {
                    }
                };
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = e.ColumnIndex == this.dataGridView1.Columns[this.ColumnItemOCR.Name].Index;
            if (flag)
            {
                object value = this.dataGridView1.Rows[e.RowIndex].Cells[this.ColumnItemOCRType.Name].Value;
                bool flag2 = value == null;
                if (!flag2)
                {
                    CnOCRType type = (CnOCRType)Enum.Parse(typeof(CnOCRType), value.ToString());
                    bool flag3 = this._curinfo != null && this._curinfo.LocalPath != null && FileHelper.IsImageExt(this._curinfo.LocalPath);
                    if (flag3)
                    {
                        if (LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).GetUCCenterView().Realview is UCPictureView)
                        {
                            UCPictureView pictureView = LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).GetUCCenterView().Realview as UCPictureView;
                            //pictureView.getImage();
                            Rectangle rec = pictureView.GetSelectedRectangle();
                            string text = OCRMgr.Parse(type, new Bitmap(pictureView.getImage()), rec);
                            //string text = OCRMgr.Parse(type, ImageHelper.LoadCorectedImage(this._curinfo.LocalPath).ToBitmap(), rec);
                            this.dataGridView1.Rows[e.RowIndex].Cells[this.ColumnItemValue.Name].Value = text;
                            this._curinfo.ExFaPiaoCode = text;
                            this._curinfo.OnDataChanged();
                        }
                    }
                }
            }
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
            this.btnFapiaoCheck = new Button();
            this.ColumnItemName = new DataGridViewTextBoxColumn();
            this.ColumnItemValue = new DataGridViewTextBoxColumn();
            this.ColumnItemOCRType = new DataGridViewComboBoxColumn();
            this.ColumnItemOCR = new DataGridViewButtonColumn();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            base.SuspendLayout();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BorderStyle = BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                this.ColumnItemName,
                this.ColumnItemValue,
                this.ColumnItemOCRType,
                this.ColumnItemOCR
            });
            this.dataGridView1.Dock = DockStyle.Top;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new Size(838, 154);
            this.dataGridView1.TabIndex = 0;
            this.btnFapiaoCheck.Location = new Point(23, 160);
            this.btnFapiaoCheck.Name = "btnFapiaoCheck";
            this.btnFapiaoCheck.Size = new Size(91, 36);
            this.btnFapiaoCheck.TabIndex = 1;
            this.btnFapiaoCheck.Text = "发票验证";
            this.btnFapiaoCheck.UseVisualStyleBackColor = true;
            this.ColumnItemName.HeaderText = "名称";
            this.ColumnItemName.Name = "ColumnItemName";
            this.ColumnItemValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnItemValue.HeaderText = "值";
            this.ColumnItemValue.MinimumWidth = 280;
            this.ColumnItemValue.Name = "ColumnItemValue";
            this.ColumnItemValue.Width = 280;
            this.ColumnItemOCRType.HeaderText = "OCR识别类型";
            this.ColumnItemOCRType.Name = "ColumnItemOCRType";
            this.ColumnItemOCR.HeaderText = "识别";
            this.ColumnItemOCR.Name = "ColumnItemOCR";
            this.ColumnItemOCR.Resizable = DataGridViewTriState.True;
            this.ColumnItemOCR.SortMode = DataGridViewColumnSortMode.Automatic;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnFapiaoCheck);
            base.Controls.Add(this.dataGridView1);
            base.Margin = new Padding(3, 3, 30, 30);
            base.Name = "UCBillInfoEdit";
            base.Size = new Size(838, 197);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            base.ResumeLayout(false);
        }
    }
}
