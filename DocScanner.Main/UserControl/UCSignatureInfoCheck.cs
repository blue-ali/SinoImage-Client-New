using DocScanner.ImgUtils;
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
    public class UCSignatureInfoCheck : UserControl
    {
        private List<InsureUserInfo> infos;

        private ImageList _iconlist = new ImageList();

        private IContainer components = null;

        private PictureBox pictureBox1;

        private PictureBox pictureBox2;

        private Button btn_Query;

        private Label label1;

        private TextBox textBox1;

        private PropertyGrid propertyGrid1;

        private Label label2;

        private Button btn_CheckPass;

        private Button btn_CheckFailed;

        private Button btn_Close;

        private ListView listBox_Infos;

        public string Title
        {
            get
            {
                return "签名信息审核";
            }
        }

        public Image ToCheckBitmap
        {
            set
            {
                this.pictureBox1.Image = value.Change2Size(this.pictureBox1.Width, this.pictureBox1.Height);
            }
        }

        public UCSignatureInfoCheck()
        {
            this.InitializeComponent();
            this.listBox_Infos.LargeImageList = this._iconlist;
            this._iconlist.ImageSize = new Size(96, 96);
            this._iconlist.ColorDepth = ColorDepth.Depth24Bit;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Form form = base.Parent as Form;
            bool flag = form != null;
            if (flag)
            {
                form.Close();
            }
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            this.listBox_Infos.Items.Clear();
            this._iconlist.Images.Clear();
            this.propertyGrid1.SelectedObject = null;
            this.pictureBox2.Image = null;
            this.infos = InsureUserInfoMgr.Instance.FindUserByName(this.textBox1.Text);
            bool flag = this.infos.Count == 0;
            if (flag)
            {
                MessageBox.Show("没有找到相应账户信息");
            }
            else
            {
                foreach (InsureUserInfo current in this.infos)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    using (current.SignatureImg = new Bitmap(ImageHelper.LoadLocalImage(LibCommon.AppContext.GetInstance().GetVal<AppSetting>(typeof(AppSetting)).TmpFileDir + current.SignatureImgName, true)))
                    {
                        Image value = current.SignatureImg.Change2Size(96, 96);
                        this._iconlist.Images.Add(value);
                        listViewItem.ImageIndex = this._iconlist.Images.Count - 1;
                        this.listBox_Infos.Items.Add(listViewItem);
                    }
                }
            }
        }

        private void listBox_Infos_MouseClick(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                bool flag2 = this.listBox_Infos.SelectedItems.Count > 0;
                if (flag2)
                {
                    this.propertyGrid1.SelectedObject = this.infos[this.listBox_Infos.SelectedIndices[0]];
                    this.pictureBox2.Image = this.infos[this.listBox_Infos.SelectedIndices[0]].SignatureImg.Change2Size(this.pictureBox2.Width, this.pictureBox2.Height);
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
            this.pictureBox1 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.btn_Query = new Button();
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.propertyGrid1 = new PropertyGrid();
            this.label2 = new Label();
            this.btn_CheckPass = new Button();
            this.btn_CheckFailed = new Button();
            this.btn_Close = new Button();
            this.listBox_Infos = new ListView();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            ((ISupportInitialize)this.pictureBox2).BeginInit();
            base.SuspendLayout();
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(436, 7);
            this.pictureBox1.Margin = new Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(318, 256);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox2.Location = new Point(436, 280);
            this.pictureBox2.Margin = new Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(318, 258);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.btn_Query.Location = new Point(203, 39);
            this.btn_Query.Margin = new Padding(2);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new Size(58, 23);
            this.btn_Query.TabIndex = 4;
            this.btn_Query.Text = "调阅";
            this.btn_Query.UseVisualStyleBackColor = true;
            this.btn_Query.Click += new EventHandler(this.btn_Query_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(30, 44);
            this.label1.Margin = new Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(31, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "姓名";
            this.textBox1.Location = new Point(86, 40);
            this.textBox1.Margin = new Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(92, 20);
            this.textBox1.TabIndex = 6;
            this.propertyGrid1.CommandsVisibleIfAvailable = false;
            this.propertyGrid1.Location = new Point(30, 297);
            this.propertyGrid1.Margin = new Padding(2);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = PropertySort.NoSort;
            this.propertyGrid1.Size = new Size(356, 241);
            this.propertyGrid1.TabIndex = 7;
            this.propertyGrid1.ToolbarVisible = false;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(34, 280);
            this.label2.Margin = new Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "详细信息";
            this.btn_CheckPass.Location = new Point(330, 561);
            this.btn_CheckPass.Margin = new Padding(2);
            this.btn_CheckPass.Name = "btn_CheckPass";
            this.btn_CheckPass.Size = new Size(76, 30);
            this.btn_CheckPass.TabIndex = 9;
            this.btn_CheckPass.Text = "核准";
            this.btn_CheckPass.UseVisualStyleBackColor = true;
            this.btn_CheckFailed.Location = new Point(484, 561);
            this.btn_CheckFailed.Margin = new Padding(2);
            this.btn_CheckFailed.Name = "btn_CheckFailed";
            this.btn_CheckFailed.Size = new Size(76, 30);
            this.btn_CheckFailed.TabIndex = 10;
            this.btn_CheckFailed.Text = "审核不通过";
            this.btn_CheckFailed.UseVisualStyleBackColor = true;
            this.btn_Close.Location = new Point(655, 561);
            this.btn_Close.Margin = new Padding(2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new Size(76, 30);
            this.btn_Close.TabIndex = 11;
            this.btn_Close.Text = "关闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.listBox_Infos.Location = new Point(30, 160);
            this.listBox_Infos.Name = "listBox_Infos";
            this.listBox_Infos.Size = new Size(356, 103);
            this.listBox_Infos.TabIndex = 12;
            this.listBox_Infos.UseCompatibleStateImageBehavior = false;
            this.listBox_Infos.MouseClick += new MouseEventHandler(this.listBox_Infos_MouseClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.listBox_Infos);
            base.Controls.Add(this.btn_Close);
            base.Controls.Add(this.btn_CheckFailed);
            base.Controls.Add(this.btn_CheckPass);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.propertyGrid1);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btn_Query);
            base.Controls.Add(this.pictureBox2);
            base.Controls.Add(this.pictureBox1);
            base.Margin = new Padding(2);
            base.Name = "UCSignatureInfoCheck";
            base.Size = new Size(772, 600);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            ((ISupportInitialize)this.pictureBox2).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
