using DocScaner.CodeUtils;
using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.CodeUtils;
using DocScanner.Common;
using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using DocScanner.OCR;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCNote : UserControl
    {
        private NFileInfo _info;

        private IContainer components = null;

        private ListView _listnote;

        private ColumnHeader ColumName;

        private ColumnHeader ColumTime;

        private ColumnHeader ColumRegion;

        private ColumnHeader ColumMsg;

        private ColumnHeader ColumItemName;

        private Button btnOCR;

        private Button btnShow;

        private Button BtYes;

        private Label LbMsg;

        private TextBox txtMsg;

        private TextBox txtUser;

        private Label LnUser;

        private Button btn_Close;

        private Button btnShowAll;

        private ComboBox CombSelectOCRLan;

        public OCRMgr.Lang SelectOCRLan
        {
            get;
            set;
        }

        public bool AllowTextOCR
        {
            get
            {
                return AbstractSetting<FunctionSetting>.CurSetting.AllowOCR;
            }
        }

        public NFileInfo ImgInfo
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
                bool flag = this._info != null;
                if (flag)
                {
                    bool flag2 = this._info.NotesList != null && this._info.NotesList.Count > 0;
                    if (flag2)
                    {
                        foreach (NNoteInfo current in this._info.NotesList)
                        {
                            bool flag3 = current != null;
                            if (flag3)
                            {
                                this.AddItem(current);
                            }
                        }
                    }
                }
            }
        }

        public Rectangle SelectRegion
        {
            get;
            set;
        }

        public Image CutImg
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return "批注信息";
            }
        }

        public UCNote()
        {
            this.InitializeComponent();
            this.txtUser.Text = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AccountSetting", "AccountName");
            this._listnote.MouseDoubleClick += new MouseEventHandler(this._listnot_MouseDoubleClick);
            this._listnote.MouseClick += new MouseEventHandler(this._listnot_MouseClick);
            this.CombSelectOCRLan.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CombSelectOCRLan.DataBindings.Add("visible", this, "AllowTextOCR");
            EnumManager<OCRMgr.Lang>.SetComboxControl(this.CombSelectOCRLan);
            this.CombSelectOCRLan.DataBindings.Add("SelectedValue", this, "SelectOCRLan").Parse += new ConvertEventHandler(this.UCFilterImg_Parse);
        }

        private void UCFilterImg_Parse(object sender, ConvertEventArgs e)
        {
            e.Value = (OCRMgr.Lang)e.Value;
        }

        private void _listnot_MouseClick(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Right;
            if (flag)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem();
                menuItem.Text = "删除";
                menuItem.Click += new EventHandler(this.menuDelItem_Click);
                contextMenu.MenuItems.Add(menuItem);
                contextMenu.Show(this._listnote, e.Location);
            }
        }

        private void menuDelItem_Click(object sender, EventArgs e)
        {
            bool flag = this._listnote.SelectedItems.Count > 0;
            if (flag)
            {
                int index = this._listnote.SelectedIndices[0];
                this._listnote.Items.RemoveAt(index);
                bool flag2 = this._info.Operation == EOperType.eFROM_SERVER_NOTCHANGE || this._info.Operation == EOperType.eUPD;
                if (!flag2)
                {
                    this._info.NotesList.RemoveAt(index);
                }
            }
        }

        private void _listnot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Left && this._listnote.SelectedItems.Count > 0;
            if (flag)
            {
                ListViewItem listViewItem = this._listnote.SelectedItems[0];
                this.txtUser.Text = listViewItem.Text;
                this.txtMsg.Text = listViewItem.SubItems[3].Text;
            }
        }

        private void BtAdd_Click(object sender, EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(this.txtMsg.Text) && !string.IsNullOrEmpty(this.txtUser.Text);
            if (flag)
            {
                NNoteInfo nNoteInfo = new NNoteInfo();
                nNoteInfo.NoteName = this.txtUser.Text;
                nNoteInfo.NoteMsg = this.txtMsg.Text;
                nNoteInfo.SetRegion(this.SelectRegion);
                nNoteInfo.SetDateTime(DateTime.Now);
                nNoteInfo.FileLink = this._info.FileName;
                nNoteInfo.FileMD5Link = MD5Helper.GetFileMD5(this._info.LocalPath);
                this.AddItem(nNoteInfo);
                bool flag2 = this._info.Operation == EOperType.eFROM_SERVER_NOTCHANGE || this._info.Operation == EOperType.eUPD;
                if (flag2)
                {
                    this._info.Operation = EOperType.eUPD;
                    this._info.NotesList.Add(nNoteInfo);
                }
                else
                {
                    this._info.NotesList.Add(nNoteInfo);
                }
            }
        }

        private void AddItem(NNoteInfo note)
        {
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = note.NoteName;
            listViewItem.SubItems.Add(note.GetCreateTime().ToShortTimeString());
            listViewItem.SubItems.Add(note.GetRegion().RectToStr());
            listViewItem.SubItems.Add(note.NoteMsg);
            listViewItem.SubItems.Add(note.FileLink);
            this._listnote.Items.Add(listViewItem);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(new ShowRegionUserCMD());
            Form form = base.Parent as Form;
            bool flag = form != null;
            if (flag)
            {
                form.Close();
            }
        }

        private void btnOCR_Click(object sender, EventArgs e)
        {
            bool flag = this.SelectOCRLan == OCRMgr.Lang.barcode;
            if (flag)
            {
                this.txtMsg.Text = "";
                this.txtMsg.Text = OCRMgr.Bar_Parse(this.CutImg.ToBitmap());
            }
            else
            {
                Rectangle rc = default(Rectangle);
                rc.X = 0;
                rc.Y = 0;
                rc.Width = this.CutImg.Width;
                rc.Height = this.CutImg.Height;
                string text = OCRMgr.TSORC_Parse(this.CutImg.ToBitmap(), this.SelectOCRLan, rc);
                bool flag2 = text != null;
                if (flag2)
                {
                    this.txtMsg.Text = text;
                }
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowRegionUserCMD showRegionUserCMD = new ShowRegionUserCMD();
            showRegionUserCMD.Note = new NNoteInfo();
            showRegionUserCMD.Note.NoteMsg = this.txtMsg.Text;
            showRegionUserCMD.Note.SetRegion(this.SelectRegion);
            showRegionUserCMD.Note.NoteName = this.txtUser.Text;
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(showRegionUserCMD);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("ShowAllNotes", null);
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
            this._listnote = new ListView();
            this.ColumName = new ColumnHeader();
            this.ColumTime = new ColumnHeader();
            this.ColumRegion = new ColumnHeader();
            this.ColumMsg = new ColumnHeader();
            this.ColumItemName = new ColumnHeader();
            this.btnOCR = new Button();
            this.btnShow = new Button();
            this.BtYes = new Button();
            this.LbMsg = new Label();
            this.txtMsg = new TextBox();
            this.txtUser = new TextBox();
            this.LnUser = new Label();
            this.btn_Close = new Button();
            this.btnShowAll = new Button();
            this.CombSelectOCRLan = new ComboBox();
            base.SuspendLayout();
            this._listnote.Columns.AddRange(new ColumnHeader[]
            {
                this.ColumName,
                this.ColumTime,
                this.ColumRegion,
                this.ColumMsg,
                this.ColumItemName
            });
            this._listnote.Dock = DockStyle.Top;
            this._listnote.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this._listnote.FullRowSelect = true;
            this._listnote.Location = new Point(0, 0);
            this._listnote.Margin = new Padding(5, 6, 5, 6);
            this._listnote.Name = "_listnote";
            this._listnote.Size = new Size(684, 218);
            this._listnote.TabIndex = 21;
            this._listnote.UseCompatibleStateImageBehavior = false;
            this._listnote.View = View.Details;
            this.ColumName.Text = "作者";
            this.ColumName.Width = 113;
            this.ColumTime.Text = "时间";
            this.ColumTime.Width = 106;
            this.ColumRegion.Text = "区域";
            this.ColumRegion.Width = 89;
            this.ColumMsg.Text = "内容";
            this.ColumMsg.Width = 232;
            this.ColumItemName.Text = "关联文件";
            this.ColumItemName.Width = 90;
            this.btnOCR.Location = new Point(535, 280);
            this.btnOCR.Margin = new Padding(3, 2, 3, 2);
            this.btnOCR.Name = "btnOCR";
            this.btnOCR.Size = new Size(75, 25);
            this.btnOCR.TabIndex = 48;
            this.btnOCR.Text = "自动识别";
            this.btnOCR.UseVisualStyleBackColor = true;
            this.btnOCR.Click += new EventHandler(this.btnOCR_Click);
            this.btnShow.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.btnShow.Location = new Point(186, 388);
            this.btnShow.Margin = new Padding(4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new Size(87, 32);
            this.btnShow.TabIndex = 47;
            this.btnShow.Text = "展示(&S)";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Visible = false;
            this.btnShow.Click += new EventHandler(this.btnShow_Click);
            this.BtYes.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.BtYes.Location = new Point(63, 343);
            this.BtYes.Margin = new Padding(4);
            this.BtYes.Name = "BtYes";
            this.BtYes.Size = new Size(89, 32);
            this.BtYes.TabIndex = 46;
            this.BtYes.Text = "添加(&A)";
            this.BtYes.UseVisualStyleBackColor = true;
            this.BtYes.Click += new EventHandler(this.BtAdd_Click);
            this.LbMsg.AutoSize = true;
            this.LbMsg.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.LbMsg.Location = new Point(1, 283);
            this.LbMsg.Margin = new Padding(4, 0, 4, 0);
            this.LbMsg.Name = "LbMsg";
            this.LbMsg.Size = new Size(57, 20);
            this.LbMsg.TabIndex = 44;
            this.LbMsg.Text = "信息(&I):";
            this.txtMsg.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.txtMsg.Location = new Point(69, 280);
            this.txtMsg.Margin = new Padding(4);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new Size(455, 27);
            this.txtMsg.TabIndex = 43;
            this.txtUser.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.txtUser.Location = new Point(75, 244);
            this.txtUser.Margin = new Padding(4);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new Size(109, 27);
            this.txtUser.TabIndex = 42;
            this.LnUser.AutoSize = true;
            this.LnUser.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.LnUser.Location = new Point(1, 247);
            this.LnUser.Margin = new Padding(4, 0, 4, 0);
            this.LnUser.Name = "LnUser";
            this.LnUser.Size = new Size(68, 20);
            this.LnUser.TabIndex = 45;
            this.LnUser.Text = "作者(&W):";
            this.btn_Close.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.btn_Close.Location = new Point(513, 343);
            this.btn_Close.Margin = new Padding(4);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new Size(89, 32);
            this.btn_Close.TabIndex = 49;
            this.btn_Close.Text = "关闭(&C)";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.btnShowAll.Font = new Font("Microsoft YaHei", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.btnShowAll.Location = new Point(288, 343);
            this.btnShowAll.Margin = new Padding(4);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new Size(89, 32);
            this.btnShowAll.TabIndex = 50;
            this.btnShowAll.Text = "展示所有";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new EventHandler(this.btnShowAll_Click);
            this.CombSelectOCRLan.Location = new Point(616, 281);
            this.CombSelectOCRLan.Name = "CombSelectOCRLan";
            this.CombSelectOCRLan.Size = new Size(59, 23);
            this.CombSelectOCRLan.TabIndex = 51;
            this.CombSelectOCRLan.Text = "文字";
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.CombSelectOCRLan);
            base.Controls.Add(this.btnShowAll);
            base.Controls.Add(this.btn_Close);
            base.Controls.Add(this.btnOCR);
            base.Controls.Add(this.btnShow);
            base.Controls.Add(this.BtYes);
            base.Controls.Add(this.LbMsg);
            base.Controls.Add(this.txtMsg);
            base.Controls.Add(this.txtUser);
            base.Controls.Add(this.LnUser);
            base.Controls.Add(this._listnote);
            base.Margin = new Padding(4);
            base.Name = "UCNote";
            base.Size = new Size(684, 424);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
