using DocScanner.Common;
using DocScanner.ImgUtils;
using DocScanner.LibCommon;
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
    public class UCFilterImg : UserControl
    {
        private IContainer components = null;

        private CheckBox checkBoxImgType;

        private ComboBox comboBox_ImgType;

        private CheckBox checkBoxSize;

        private CheckBox checkBoxFilterBlank;

        private CheckBox checkBoxAutoRectify;

        private CheckBox checkBoxRemoveBlackEdge;

        private CheckBox checkBoxSharp;

        private CheckBox checkBoxGray;

        private CheckBox checkBoxBlackWhite;

        private TextBox textBox_Size;

        private Label label1;

        public EMostImageType ImageType
        {
            get
            {
                return ImgTypeProcessor.DestType;
            }
            set
            {
                ImgTypeProcessor.DestType = value;
            }
        }

        public int ImageSize
        {
            get
            {
                return ImgSizeProcessor.MaxSize;
            }
            set
            {
                ImgSizeProcessor.MaxSize = value;
            }
        }

        public string Title
        {
            get
            {
                return "图像批量读入过滤转换";
            }
        }

        public UCFilterImg()
        {
            this.InitializeComponent();
            EnumManager<EMostImageType>.SetComboxControl(this.comboBox_ImgType);
            this.comboBox_ImgType.DataBindings.Add("SelectedValue", this, "ImageType").Parse += new ConvertEventHandler(this.UCFilterImg_Parse);
            this.textBox_Size.DataBindings.Add("Text", this, "ImageSize");
            this.LoadCheckBox();
            base.HandleDestroyed += new EventHandler(this.UCFilterImg_HandleDestroyed);
        }

        private void UCFilterImg_Parse(object sender, ConvertEventArgs e)
        {
            e.Value = (EMostImageType)e.Value;
        }

        private void UCFilterImg_HandleDestroyed(object sender, EventArgs e)
        {
            this.SaveCheckBox();
        }

        private void LoadCheckBox()
        {
            this.checkBoxImgType.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgTypeProcessor)).Enabled;
            this.checkBoxSize.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgSizeProcessor)).Enabled;
            this.checkBoxFilterBlank.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgBlankProcessor)).Enabled;
            this.checkBoxAutoRectify.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgRectifyProcessor)).Enabled;
            this.checkBoxRemoveBlackEdge.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgRmBlackEdgeProcessor)).Enabled;
            this.checkBoxSharp.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgSharpProcessor)).Enabled;
            this.checkBoxGray.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgGrayProcessor)).Enabled;
            this.checkBoxBlackWhite.Checked = ImageChainProcessor.Cur.GetProcessor(typeof(ImgBWProcessor)).Enabled;
        }

        private void SaveCheckBox()
        {
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgTypeProcessor)).Enabled = this.checkBoxImgType.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgBlankProcessor)).Enabled = this.checkBoxFilterBlank.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgRectifyProcessor)).Enabled = this.checkBoxAutoRectify.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgRmBlackEdgeProcessor)).Enabled = this.checkBoxRemoveBlackEdge.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgSharpProcessor)).Enabled = this.checkBoxSharp.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgSizeProcessor)).Enabled = this.checkBoxSize.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgGrayProcessor)).Enabled = this.checkBoxGray.Checked;
            ImageChainProcessor.Cur.GetProcessor(typeof(ImgBWProcessor)).Enabled = this.checkBoxBlackWhite.Checked;
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
            this.checkBoxImgType = new CheckBox();
            this.comboBox_ImgType = new ComboBox();
            this.checkBoxSize = new CheckBox();
            this.checkBoxFilterBlank = new CheckBox();
            this.checkBoxAutoRectify = new CheckBox();
            this.checkBoxRemoveBlackEdge = new CheckBox();
            this.checkBoxSharp = new CheckBox();
            this.checkBoxGray = new CheckBox();
            this.checkBoxBlackWhite = new CheckBox();
            this.textBox_Size = new TextBox();
            this.label1 = new Label();
            base.SuspendLayout();
            this.checkBoxImgType.AutoSize = true;
            this.checkBoxImgType.BackColor = SystemColors.ControlLightLight;
            this.checkBoxImgType.Location = new Point(44, 54);
            this.checkBoxImgType.Name = "checkBoxImgType";
            this.checkBoxImgType.Size = new Size(189, 19);
            this.checkBoxImgType.TabIndex = 0;
            this.checkBoxImgType.Text = "读入图片统一转换为格式";
            this.comboBox_ImgType.FormattingEnabled = true;
            this.comboBox_ImgType.Location = new Point(307, 50);
            this.comboBox_ImgType.Name = "comboBox_ImgType";
            this.comboBox_ImgType.Size = new Size(162, 23);
            this.comboBox_ImgType.TabIndex = 1;
            this.checkBoxSize.AutoSize = true;
            this.checkBoxSize.BackColor = SystemColors.ControlLightLight;
            this.checkBoxSize.Location = new Point(44, 103);
            this.checkBoxSize.Name = "checkBox2";
            this.checkBoxSize.Size = new Size(174, 19);
            this.checkBoxSize.TabIndex = 2;
            this.checkBoxSize.Text = "读入图片大小转换小于";
            this.checkBoxFilterBlank.AutoSize = true;
            this.checkBoxFilterBlank.BackColor = SystemColors.ControlLightLight;
            this.checkBoxFilterBlank.Location = new Point(44, 198);
            this.checkBoxFilterBlank.Name = "checkBox_FilterBlank";
            this.checkBoxFilterBlank.Size = new Size(128, 19);
            this.checkBoxFilterBlank.TabIndex = 3;
            this.checkBoxFilterBlank.Text = "自动过滤空白页";
            this.checkBoxAutoRectify.AutoSize = true;
            this.checkBoxAutoRectify.BackColor = SystemColors.ControlLightLight;
            this.checkBoxAutoRectify.Location = new Point(44, 243);
            this.checkBoxAutoRectify.Name = "checkBox4";
            this.checkBoxAutoRectify.Size = new Size(81, 19);
            this.checkBoxAutoRectify.TabIndex = 4;
            this.checkBoxAutoRectify.Text = "自动纠偏";
            this.checkBoxRemoveBlackEdge.AutoSize = true;
            this.checkBoxRemoveBlackEdge.BackColor = SystemColors.ControlLightLight;
            this.checkBoxRemoveBlackEdge.Location = new Point(44, 290);
            this.checkBoxRemoveBlackEdge.Name = "checkBox5";
            this.checkBoxRemoveBlackEdge.Size = new Size(97, 19);
            this.checkBoxRemoveBlackEdge.TabIndex = 5;
            this.checkBoxRemoveBlackEdge.Text = "自动去黑边";
            this.checkBoxSharp.AutoSize = true;
            this.checkBoxSharp.BackColor = SystemColors.ControlLightLight;
            this.checkBoxSharp.Location = new Point(44, 331);
            this.checkBoxSharp.Name = "checkBox6";
            this.checkBoxSharp.Size = new Size(81, 19);
            this.checkBoxSharp.TabIndex = 6;
            this.checkBoxSharp.Text = "自动锐化";
            this.checkBoxGray.AutoSize = true;
            this.checkBoxGray.BackColor = SystemColors.ControlLightLight;
            this.checkBoxGray.Location = new Point(44, 370);
            this.checkBoxGray.Name = "checkBox7";
            this.checkBoxGray.Size = new Size(81, 19);
            this.checkBoxGray.TabIndex = 7;
            this.checkBoxGray.Text = "自动灰度";
            this.checkBoxBlackWhite.AutoSize = true;
            this.checkBoxBlackWhite.BackColor = SystemColors.ControlLightLight;
            this.checkBoxBlackWhite.Location = new Point(44, 409);
            this.checkBoxBlackWhite.Name = "checkBoxBlackWhite";
            this.checkBoxBlackWhite.Size = new Size(81, 19);
            this.checkBoxBlackWhite.TabIndex = 8;
            this.checkBoxBlackWhite.Text = "自动黑白";
            this.textBox_Size.Location = new Point(308, 94);
            this.textBox_Size.Name = "textBox_Size";
            this.textBox_Size.Size = new Size(132, 25);
            this.textBox_Size.TabIndex = 9;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(462, 102);
            this.label1.Name = "label1";
            this.label1.Size = new Size(15, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "K";
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBox_Size);
            base.Controls.Add(this.checkBoxBlackWhite);
            base.Controls.Add(this.checkBoxGray);
            base.Controls.Add(this.checkBoxSharp);
            base.Controls.Add(this.checkBoxRemoveBlackEdge);
            base.Controls.Add(this.checkBoxAutoRectify);
            base.Controls.Add(this.checkBoxFilterBlank);
            base.Controls.Add(this.checkBoxSize);
            base.Controls.Add(this.comboBox_ImgType);
            base.Controls.Add(this.checkBoxImgType);
            base.Name = "UCFilterImg";
            base.Size = new Size(696, 464);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
