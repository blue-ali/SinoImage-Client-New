using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCStatusBar : UserControl, IHasIPropertiesSetting
    {
        public class NestIPropertiesSetting : IPropertiesSetting
        {
            private UCStatusBar _bar;

            [Browsable(false)]
            public string Name
            {
                get
                {
                    return "界面设置-底部状态栏";
                }
            }

            public NestIPropertiesSetting(UCStatusBar bar)
            {
                this._bar = bar;
            }
        }

        [ThreadStatic]
        private static UCStatusBar _curinstance;

        private Timer _timer2;

        private UCStatusBar.NestIPropertiesSetting _setting;

        private IContainer components = null;

        private Timer timer1;

        private RadStatusStrip radStatusBar1;

        private RadTextBoxElement textBox1;

        private RadToolStripSeparatorItem radToolStripSeparatorItem1;

        private RadLabelElement radLabelElement0;

        private RadProgressBarElement radProgressBarElement1;

        private RadToolStripSeparatorItem radToolStripSeparatorItem2;

        private RadButtonElement radButtonElementL;

        private RadButtonElement radButtonElementR;

        private RadToolStripSeparatorItem radToolStripSeparatorItem3;

        private RadLabelElement radLabelElement1;

        private RadToolStripSeparatorItem radToolStripSeparatorItem4;

        private RadLabelElement radLabelElement2;

        private RadToolStripSeparatorItem radToolStripSeparatorItem5;

        private RadLabelElement radLabelElement3;

        private RadToolStripSeparatorItem radToolStripSeparatorItem6;

        private RadLabelElement radLabelElement4;

        public event EventHandler<TEventArg<string>> OnActionClick;

        public static UCStatusBar CurInstance
        {
            get
            {
                return UCStatusBar._curinstance;
            }
        }

        public UCStatusBar()
        {
            this.InitializeComponent();
            UCStatusBar._curinstance = this;
            this.radStatusBar1.Dock = DockStyle.Fill;
            this.radButtonElementL.Click += new EventHandler(this.radButtonElementL_Click);
            this.radButtonElementR.Click += new EventHandler(this.radButtonElementR_Click);
            this.OnActionClick += new EventHandler<TEventArg<string>>(this.UCStatusBar_OnActionClick);
            this.radLabelElement2.DataBindings.Add("Text", AbstractSetting<AccountSetting>.CurSetting, "AccountOrgID").ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            this.radLabelElement3.DataBindings.Add("Text", AbstractSetting<AccountSetting>.CurSetting, "AccountName").ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            this.radLabelElement4.DataBindings.Add("Text", AbstractSetting<UpdateSetting>.CurSetting, "AppVersion").ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            this.radLabelElement2.Click += new EventHandler(this.RadLabelElementUserInfo_Click);
            this.radLabelElement3.Click += new EventHandler(this.RadLabelElementUserInfo_Click);
        }

        private void RadLabelElementUserInfo_Click(object sender, EventArgs e)
        {
            UCAccountSetting control = new UCAccountSetting();
            control.ShowInContainer();
        }

        private void RadLabelElement2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UCStatusBar_OnActionClick(object sender, TEventArg<string> e)
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(e.Arg, null);
        }

        private void radButtonElementBottom_Click(object sender, EventArgs e)
        {
            bool flag = this.OnActionClick != null;
            if (flag)
            {
                this.OnActionClick(this, new TEventArg<string>("BottomPaneShift"));
            }
        }

        private void radButtonElementL_Click(object sender, EventArgs e)
        {
            bool flag = this.OnActionClick != null;
            if (flag)
            {
                this.OnActionClick(this, new TEventArg<string>("LeftPaneShift"));
            }
            bool flag2 = this.radButtonElementL.Text == "＞";
            if (flag2)
            {
                this.radButtonElementL.Text = "＜";
            }
            else
            {
                this.radButtonElementL.Text = "＞";
            }
        }

        private void radButtonElementR_Click(object sender, EventArgs e)
        {
            bool flag = this.OnActionClick != null;
            if (flag)
            {
                this.OnActionClick(this, new TEventArg<string>("RightPaneShift"));
            }
            bool flag2 = this.radButtonElementR.Text == "＞";
            if (flag2)
            {
                this.radButtonElementR.Text = "＜";
            }
            else
            {
                this.radButtonElementR.Text = "＞";
            }
        }

        public void OnMessage(object sender, MessageEventArgs arg)
        {
            bool invokeRequired = base.InvokeRequired;
            if (invokeRequired)
            {
                base.Invoke(new Action<object, MessageEventArgs>(this.OnMessage), new object[]
                {
                    sender,
                    arg
                });
            }
            else
            {
                switch (arg.MsgType)
                {
                    case EMessageType.None:
                        this.textBox1.Text = "";
                        break;
                    case EMessageType.Log:
                        this.textBox1.ForeColor = Color.Gray;
                        this.textBox1.Text = "Log: " + arg.Msg;
                        break;
                    case EMessageType.Info:
                        this.textBox1.ForeColor = Color.Black;
                        this.textBox1.Text = "Info: " + arg.Msg;
                        break;
                    case EMessageType.Warning:
                        this.textBox1.ForeColor = Color.YellowGreen;
                        this.textBox1.Text = "Warning: " + arg.Msg;
                        break;
                    case EMessageType.Success:
                        this.textBox1.ForeColor = Color.Green;
                        this.textBox1.Text = "Successed: " + arg.Msg;
                        break;
                    case EMessageType.Error:
                        this.textBox1.ForeColor = Color.Red;
                        this.textBox1.Text = "Error: " + arg.Msg;
                        break;
                }
                this.DelayClearMsg();
            }
        }

        public void DelayClearMsg()
        {
            bool flag = this._timer2 == null;
            if (flag)
            {
                this._timer2 = new Timer();
                this._timer2.Interval = 10000;
                this._timer2.Tick += delegate (object senderx, EventArgs argx)
                {
                    this.textBox1.Text = "";
                    this.textBox1.ForeColor = Color.Black;
                    this._timer2.Dispose();
                    this._timer2 = null;
                };
                this._timer2.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.radProgressBarElement1.PerformStepValue1();
            bool flag = this.radProgressBarElement1.Value1 >= this.radProgressBarElement1.Maximum;
            if (flag)
            {
                this.radProgressBarElement1.Value1 = this.radProgressBarElement1.Minimum;
            }
        }

        public void BeginJob(string jobname)
        {
            bool invokeRequired = base.InvokeRequired;
            if (invokeRequired)
            {
                base.Invoke(new Action<string>(this.BeginJob), new object[]
                {
                    jobname
                });
            }
            else
            {
                this.textBox1.ForeColor = Color.Gray;
                this.textBox1.Text = jobname;
                this.radLabelElement0.Text = jobname;
                this.Refresh();
                this.radProgressBarElement1.Value1 = this.radProgressBarElement1.Minimum;
                this.timer1.Enabled = true;
                this.timer1.Start();
            }
        }

        public void EndJob()
        {
            bool invokeRequired = base.InvokeRequired;
            if (invokeRequired)
            {
                base.Invoke(new Action(this.EndJob));
            }
            else
            {
                this.textBox1.Text = "";
                this.radLabelElement0.Text = "完成";
                this.radProgressBarElement1.Value1 = this.radProgressBarElement1.Minimum;
                this.Refresh();
                this.timer1.Stop();
            }
        }

        public UCStatusBar.NestIPropertiesSetting GetSetting()
        {
            bool flag = this._setting == null;
            if (flag)
            {
                this._setting = new UCStatusBar.NestIPropertiesSetting(this);
            }
            return this._setting;
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return this.GetSetting();
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
            this.components = new Container();
            this.radStatusBar1 = new RadStatusStrip();
            this.textBox1 = new RadTextBoxElement();
            this.radToolStripSeparatorItem1 = new RadToolStripSeparatorItem();
            this.radLabelElement0 = new RadLabelElement();
            this.radProgressBarElement1 = new RadProgressBarElement();
            this.radToolStripSeparatorItem2 = new RadToolStripSeparatorItem();
            this.radLabelElement1 = new RadLabelElement();
            this.radToolStripSeparatorItem3 = new RadToolStripSeparatorItem();
            this.radButtonElementL = new RadButtonElement();
            this.radButtonElementR = new RadButtonElement();
            this.radLabelElement2 = new RadLabelElement();
            this.radToolStripSeparatorItem5 = new RadToolStripSeparatorItem();
            this.radLabelElement3 = new RadLabelElement();
            this.radToolStripSeparatorItem6 = new RadToolStripSeparatorItem();
            this.radLabelElement4 = new RadLabelElement();
            this.radToolStripSeparatorItem4 = new RadToolStripSeparatorItem();
            this.timer1 = new Timer(this.components);
            ((ISupportInitialize)this.radStatusBar1).BeginInit();
            base.SuspendLayout();
            this.radStatusBar1.AutoSize = true;
            this.radStatusBar1.BackColor = SystemColors.ControlLightLight;
            this.radStatusBar1.Items.AddRange(new RadItem[]
            {
                this.textBox1,
                this.radToolStripSeparatorItem1,
                this.radLabelElement0,
                this.radProgressBarElement1,
                this.radToolStripSeparatorItem2,
                this.radLabelElement1,
                this.radToolStripSeparatorItem3,
                this.radButtonElementL,
                this.radButtonElementR,
                this.radToolStripSeparatorItem3,
                this.radLabelElement2,
                this.radToolStripSeparatorItem5,
                this.radLabelElement3,
                this.radToolStripSeparatorItem6,
                this.radLabelElement4
            });
            this.radStatusBar1.LayoutStyle = RadStatusBarLayoutStyle.Stack;
            this.radStatusBar1.Location = new Point(0, 1);
            this.radStatusBar1.Margin = new Padding(5, 4, 5, 4);
            this.radStatusBar1.Name = "radStatusBar1";
            this.radStatusBar1.RootElement.ControlBounds = new Rectangle(0, 1, 300, 24);
            this.radStatusBar1.RootElement.StretchVertically = true;
            this.radStatusBar1.Size = new Size(1155, 23);
            this.radStatusBar1.SizingGrip = true;
            this.radStatusBar1.TabIndex = 0;
            this.radStatusBar1.Text = "radStatusBar1";
            this.textBox1.CanFocus = true;
            this.textBox1.Margin = new Padding(1);
            this.textBox1.Name = "radButtonElement1";
            this.radStatusBar1.SetSpring(this.textBox1, true);
            this.textBox1.StretchVertically = false;
            this.textBox1.Text = "";
            this.radToolStripSeparatorItem1.Margin = new Padding(1);
            this.radToolStripSeparatorItem1.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem1.Name = "radToolStripSeparatorItem1";
            this.radStatusBar1.SetSpring(this.radToolStripSeparatorItem1, false);
            this.radToolStripSeparatorItem1.Text = "radToolStripSeparatorItem1";
            this.radLabelElement0.Margin = new Padding(1);
            this.radLabelElement0.MinSize = new Size(2, 17);
            this.radLabelElement0.Name = "radLabelElement0";
            this.radStatusBar1.SetSpring(this.radLabelElement0, false);
            this.radLabelElement0.Text = "任务进度";
            this.radLabelElement0.TextWrap = true;
            this.radProgressBarElement1.AutoSize = false;
            this.radProgressBarElement1.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            this.radProgressBarElement1.Bounds = new Rectangle(0, 0, 133, 16);
            this.radProgressBarElement1.ClipDrawing = true;
            this.radProgressBarElement1.DefaultSize = new Size(130, 16);
            this.radProgressBarElement1.Margin = new Padding(1);
            this.radProgressBarElement1.Name = "radProgressBarElement1";
            this.radStatusBar1.SetSpring(this.radProgressBarElement1, false);
            this.radProgressBarElement1.Text = "";
            this.radProgressBarElement1.TextAlignment = ContentAlignment.MiddleCenter;
            this.radToolStripSeparatorItem2.Margin = new Padding(1);
            this.radToolStripSeparatorItem2.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem2.Name = "radToolStripSeparatorItem2";
            this.radStatusBar1.SetSpring(this.radToolStripSeparatorItem2, false);
            this.radToolStripSeparatorItem2.Text = "radToolStripSeparatorItem2";
            this.radLabelElement1.Margin = new Padding(1);
            this.radLabelElement1.MinSize = new Size(2, 17);
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusBar1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "";
            this.radLabelElement1.TextWrap = true;
            this.radToolStripSeparatorItem3.Margin = new Padding(1);
            this.radToolStripSeparatorItem3.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem3.Name = "radToolStripSeparatorItem3";
            this.radStatusBar1.SetSpring(this.radToolStripSeparatorItem3, false);
            this.radToolStripSeparatorItem3.Text = "radToolStripSeparatorItem3";
            this.radButtonElementL.Margin = new Padding(1);
            this.radButtonElementL.MinSize = new Size(2, 17);
            this.radButtonElementL.Name = "radLabelElementL";
            this.radStatusBar1.SetSpring(this.radButtonElementL, false);
            this.radButtonElementL.Text = "＜";
            this.radButtonElementR.Margin = new Padding(1);
            this.radButtonElementR.MinSize = new Size(2, 17);
            this.radButtonElementR.Name = "radLabelElementR";
            this.radStatusBar1.SetSpring(this.radButtonElementR, false);
            this.radButtonElementR.Text = "＞";
            this.radLabelElement2.Margin = new Padding(1);
            this.radLabelElement2.MinSize = new Size(2, 17);
            this.radLabelElement2.Name = "radLabelElement1";
            this.radStatusBar1.SetSpring(this.radLabelElement2, false);
            this.radLabelElement2.Text = "";
            this.radLabelElement2.TextWrap = true;
            this.radToolStripSeparatorItem5.Margin = new Padding(1);
            this.radToolStripSeparatorItem5.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem5.Name = "radToolStripSeparatorItem5";
            this.radStatusBar1.SetSpring(this.radToolStripSeparatorItem5, false);
            this.radToolStripSeparatorItem5.Text = "radToolStripSeparatorItem5";
            this.radLabelElement3.Margin = new Padding(1);
            this.radLabelElement3.Name = "radLabelElement2";
            this.radStatusBar1.SetSpring(this.radLabelElement3, false);
            this.radLabelElement3.Text = "";
            this.radLabelElement3.TextWrap = true;
            this.radToolStripSeparatorItem6.Margin = new Padding(1);
            this.radToolStripSeparatorItem6.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem6.Name = "radToolStripSeparatorItem4";
            this.radStatusBar1.SetSpring(this.radToolStripSeparatorItem6, false);
            this.radToolStripSeparatorItem6.Text = "radToolStripSeparatorItem4";
            this.radLabelElement4.Margin = new Padding(1);
            this.radLabelElement4.Name = "radLabelElement3";
            this.radStatusBar1.SetSpring(this.radLabelElement4, false);
            this.radLabelElement4.Text = "";
            this.radLabelElement4.TextWrap = true;
            this.radToolStripSeparatorItem4.Margin = new Padding(1);
            this.radToolStripSeparatorItem4.MinSize = new Size(2, 17);
            this.radToolStripSeparatorItem4.Name = "radToolStripSeparatorItem4";
            this.radToolStripSeparatorItem4.Text = "radToolStripSeparatorItem4";
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.radStatusBar1);
            base.Name = "UCStatusBar";
            base.Size = new Size(1155, 24);
            ((ISupportInitialize)this.radStatusBar1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
