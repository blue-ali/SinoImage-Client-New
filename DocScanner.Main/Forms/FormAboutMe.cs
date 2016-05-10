using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.UICommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DocScanner.Main
{
    public class FormAboutMe : FormBase, IHasIPropertiesSetting
    {
        public class NestSetting : IPropertiesSetting
        {
            private FormAboutMe _parent;

            [Browsable(false)]
            public string Name
            {
                get
                {
                    return "界面设置-About对话框";
                }
            }

            [Category("用户UI设置"), DisplayName("LableLinkURL")]
            public string LableLinkURL
            {
                get
                {
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AboutDialog", "LableLinkURL");
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AboutDialog", "LableLinkURL", value.ToString());
                }
            }

            [Category("用户UI设置"), DisplayName("CompanyName")]
            public string CompanyName
            {
                get
                {
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AboutDialog", "CompanyName");
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AboutDialog", "CompanyName", value.ToString());
                }
            }

            [Category("用户UI设置"), DisplayName("公司Log"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
            public string CompanyLogImg
            {
                get
                {
                    string text = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AboutDialog", "CompanyLogImg");
                    bool flag = string.IsNullOrEmpty(text);
                    if (flag)
                    {
                        text = SystemHelper.ResourceDir + "companylog.jpg";
                    }
                    return text;
                }
                set
                {
                    string assemblesDirectory = SystemHelper.GetAssemblesDirectory();
                    bool flag = value.StartsWith(assemblesDirectory);
                    if (flag)
                    {
                        value = value.Substring(assemblesDirectory.Length);
                    }
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AboutDialog", "CompanyLogImg", value.ToString());
                }
            }

            [Category("用户UI设置"), DisplayName("公司宣传口号")]
            public string CompanyWords
            {
                get
                {
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AboutDialog", "CompanyWords");
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AboutDialog", "CompanyWords", value.ToString());
                }
            }

            [Category("用户UI设置"), DisplayName("公司Address")]
            public string CompanyAddress
            {
                get
                {
                    return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("AboutDialog", "CompanyAddress");
                }
                set
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("AboutDialog", "CompanyAddress", value.ToString());
                }
            }

            public NestSetting(FormAboutMe bar)
            {
                this._parent = bar;
            }
        }

        public const string CopyRight = "版权所有  ";

        private FormAboutMe.NestSetting _setting;

        private int i = 0;

        private IContainer components = null;

        private LinkLabel LableCompanyLink;

        private Label LabelAddress;

        private Label LabelExt;

        private WaterWavePictureBox pictureBox1;

        public static IPropertiesSetting Setting
        {
            get
            {
                return new FormAboutMe.NestSetting(null);
            }
        }

        public FormAboutMe()
        {
            this.InitializeComponent();
            this.Text = "";
            this.SetKeyEscCloseForm(true);
            this.pictureBox1.Image = Image.FromFile(this.GetSetting().CompanyLogImg);
            this.LableCompanyLink.Text = "版权所有  " + this.GetSetting().CompanyName;
            this.LableCompanyLink.Links.Add("版权所有  ".Length, this.LableCompanyLink.Text.Length - "版权所有  ".Length, this.GetSetting().LableLinkURL);
            this.LableCompanyLink.Click += delegate (object sender, EventArgs arg)
            {
                bool flag = string.IsNullOrEmpty(this.GetSetting().LableLinkURL);
                if (flag)
                {
                    try
                    {
                        Process.Start(this.GetSetting().LableLinkURL);
                    }
                    catch
                    {
                    }
                }
            };
            this.LabelAddress.Text = this.GetSetting().CompanyAddress;
            this.SetKeyEscCloseForm(true);
            this.LabelExt.Text = this.GetSetting().CompanyWords;
            this.LeftMouseMoveForm();
            this.pictureBox1.StartRipple();
            this.pictureBox1.Click += new EventHandler(this.PictureBox1_Click);
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public FormAboutMe.NestSetting GetSetting()
        {
            bool flag = this._setting == null;
            if (flag)
            {
                this._setting = new FormAboutMe.NestSetting(this);
            }
            return this._setting;
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return this.GetSetting();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.i++;
            bool flag = this.i % 3 == 0;
            if (flag)
            {
                AbstractSetting<AppSetting>.CurSetting.ShowAdvanceSetting = true;
            }
            else
            {
                AbstractSetting<AppSetting>.CurSetting.ShowAdvanceSetting = false;
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
            this.LableCompanyLink = new LinkLabel();
            this.LabelAddress = new Label();
            this.LabelExt = new Label();
            this.pictureBox1 = new WaterWavePictureBox();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.LableCompanyLink.AutoSize = true;
            this.LableCompanyLink.Location = new Point(154, 302);
            this.LableCompanyLink.Margin = new Padding(4, 0, 4, 0);
            this.LableCompanyLink.Name = "LableCompanyLink";
            this.LableCompanyLink.Size = new Size(55, 15);
            this.LableCompanyLink.TabIndex = 1;
            this.LableCompanyLink.TabStop = true;
            this.LableCompanyLink.Text = "ComURL";
            this.LabelAddress.AutoSize = true;
            this.LabelAddress.Location = new Point(137, 348);
            this.LabelAddress.Margin = new Padding(4, 0, 4, 0);
            this.LabelAddress.Name = "LabelAddress";
            this.LabelAddress.Size = new Size(63, 15);
            this.LabelAddress.TabIndex = 3;
            this.LabelAddress.Text = "Address";
            this.LabelExt.AutoSize = true;
            this.LabelExt.Font = new Font("SimSun", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.LabelExt.Location = new Point(205, 261);
            this.LabelExt.Margin = new Padding(4, 0, 4, 0);
            this.LabelExt.Name = "LabelExt";
            this.LabelExt.Size = new Size(167, 15);
            this.LabelExt.TabIndex = 4;
            this.LabelExt.Text = "智慧服务，相伴将来！";
            this.pictureBox1.Dock = DockStyle.Top;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Margin = new Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(601, 245);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(601, 422);
            base.Controls.Add(this.LabelExt);
            base.Controls.Add(this.LabelAddress);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.LableCompanyLink);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Margin = new Padding(3);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormAboutMe";
            this.Text = "FormAboutMe";
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
