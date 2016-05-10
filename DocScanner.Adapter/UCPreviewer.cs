using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace DocScanner.Adapter.SharpWebcam
{
    public class UCPreviewer : UserControl
	{
		public class CapsConverter : StringConverter
		{
			private static List<string> _caps = new List<string>();

			public static List<string> Caps
			{
				get
				{
					return UCPreviewer.CapsConverter._caps;
				}
				set
				{
					UCPreviewer.CapsConverter._caps = value;
				}
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new TypeConverter.StandardValuesCollection(UCPreviewer.CapsConverter._caps.ToArray());
			}
		}

		private SharpWebcamAcquirer _parent;

		private FilterInfoCollection _videoDevices;

		private VideoCaptureDevice _videoSource1;

		private ImageList _iconlist = new ImageList();

		private List<string> _images = new List<string>();

		private IContainer components = null;

		private VideoSourcePlayer videoSourcePlayer1;

		private Label label1;

		private ListView listView1;

		private ComboBox comboBox_Cams;

		private Button btn_Setting;

		private Button startButton;

		private Button btn_Capture;

		private Button btn_Save;

		private Button btn_Cancel;

		public string Title
		{
			get
			{
				return "摄像头采集器";
			}
		}

		public UCPreviewer(SharpWebcamAcquirer parent)
		{
			this.InitializeComponent();
			this.startButton.Focus();
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.listView1.LargeImageList = this._iconlist;
			this.listView1.MultiSelect = false;
			this.listView1.DoubleClick += new EventHandler(this.ListView1_DoubleClick);
			base.HandleDestroyed += new EventHandler(this.UCPreviewer_HandleDestroyed);
			this._parent = parent;
			this._iconlist.ImageSize = new Size(this._parent.GetSetting().ThumbWidth, this._parent.GetSetting().ThumbHeight);
			this._iconlist.ColorDepth = ColorDepth.Depth24Bit;
			this.InitWebcams();
		}

		private void ListView1_DoubleClick(object sender, EventArgs e)
		{
			bool flag = this.listView1.SelectedItems.Count > 0;
			if (flag)
			{
				this.menuexitem_Click(this, e);
			}
		}

		private void UCPreviewer_HandleDestroyed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		private void InitWebcams()
		{
			try
			{
				this.comboBox_Cams.Enabled = true;
				this.btn_Setting.Enabled = true;
				this._videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
				bool flag = this._videoDevices.Count == 0;
				if (flag)
				{
					this.startButton.Enabled = false;
					this.comboBox_Cams.Items.Add("没有摄像头发现");
					this.comboBox_Cams.SelectedIndex = 0;
					this.comboBox_Cams.Enabled = false;
					this.btn_Setting.Enabled = false;
				}
				string a = "";
				try
				{
					a = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "default");
				}
				catch
				{
				}
				int num = 1;
				int i = 1;
				int count = this._videoDevices.Count;
				while (i <= count)
				{
					string text = i + " : " + this._videoDevices[i - 1].Name;
					bool flag2 = a == text;
					if (flag2)
					{
						num = i;
					}
					this.comboBox_Cams.Items.Add(text);
					i++;
				}
				this.comboBox_Cams.SelectedIndex = num - 1;
				this.comboBox_Cams.SelectedIndexChanged += delegate(object sender, EventArgs e)
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "default", this.comboBox_Cams.Text);
					this.Start();
				};
			}
			catch
			{
			}
		}

		private List<string> makeCaps(VideoCapabilities[] caps)
		{
			bool flag = caps == null || caps.Length == 0;
			List<string> result;
			if (flag)
			{
				result = new List<string>();
			}
			else
			{
				UCPreviewer.CapsConverter.Caps.Clear();
				for (int i = 0; i < caps.Length; i++)
				{
					VideoCapabilities videoCapabilities = caps[i];
					string item = videoCapabilities.FrameSize.Width.ToString() + "x" + videoCapabilities.FrameSize.Height.ToString();
					UCPreviewer.CapsConverter.Caps.Add(item);
				}
				result = UCPreviewer.CapsConverter.Caps;
			}
			return result;
		}

		private void btn_Start_Click(object sender, EventArgs e)
		{
		}

		public void Start()
		{
			this.videoSourcePlayer1.SignalToStop();
			this.videoSourcePlayer1.WaitForStop();
			bool flag = this._videoDevices.Count == 0;
			if (!flag)
			{
				this._videoSource1 = new VideoCaptureDevice(this._videoDevices[this.comboBox_Cams.SelectedIndex].MonikerString);
				VideoCapabilities[] snapshotCapabilities = this._videoSource1.SnapshotCapabilities;
				bool flag2 = snapshotCapabilities.Length != 0;
				if (flag2)
				{
					int num = this.makeCaps(snapshotCapabilities).IndexOf(this._parent.GetSetting().Resolutions);
					bool flag3 = num != -1;
					if (flag3)
					{
						this._videoSource1.SnapshotResolution = snapshotCapabilities[num];
						this._videoSource1.VideoResolution = snapshotCapabilities[num];
					}
					else
					{
						this._videoSource1.SnapshotResolution = snapshotCapabilities[snapshotCapabilities.Length - 1];
						this._videoSource1.VideoResolution = snapshotCapabilities[snapshotCapabilities.Length - 1];
					}
				}
				this.videoSourcePlayer1.VideoSource = this._videoSource1;
				this.videoSourcePlayer1.Start();
			}
		}

		private void btn_Capture_Click(object sender, EventArgs e)
		{
			bool flag = this._videoSource1 != null;
			if (flag)
			{
				this._videoSource1.NewFrame += new NewFrameEventHandler(this.videoSource1_NewFrame);
			}
		}

		public static Image LoadSizedImage(string strSourceFileName, int intWidth, int intHeight)
		{
			Image result;
			using (Stream stream = new FileStream(strSourceFileName, FileMode.Open, FileAccess.Read))
			{
				Image img = Image.FromStream(stream);
				Image image = UCPreviewer.Change2Size(img, intWidth, intHeight);
				result = image;
			}
			return result;
		}

		public static Image Change2Size(Image img, int Width, int Height)
		{
			return img.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
		}

		private void AddtoListivew(NewFrameEventArgs eventArgs)
		{
			string text = string.Concat(new object[]
			{
				this._parent.GetSetting().ImageDir,
				DateTime.Now.ToString("yyyyMMddHHmmssfff"),
				".",
				this._parent.GetSetting().FType
			});
			EncoderParameters encoderParameters = new EncoderParameters(1);
			EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, this._parent.GetSetting().ImgRatio);
			encoderParameters.Param[0] = encoderParameter;
			eventArgs.Frame.Save(text, this._parent.GetSetting().FType.ToImageCodecInfo(), encoderParameters);
			TmpFileMgr.AddTmpFile(text);
			bool flag = File.Exists(text);
			if (flag)
			{
				ListViewItem listViewItem = new ListViewItem();
				Image value = UCPreviewer.LoadSizedImage(text, this._parent.GetSetting().ThumbWidth, this._parent.GetSetting().ThumbHeight);
				this._iconlist.Images.Add(value);
				listViewItem.ImageIndex = this._iconlist.Images.Count - 1;
				listViewItem.Text = text;
				this.listView1.Items.Add(listViewItem);
			}
		}

		private void videoSource1_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			this._videoSource1.NewFrame -= new NewFrameEventHandler(this.videoSource1_NewFrame);
			bool invokeRequired = base.InvokeRequired;
			if (invokeRequired)
			{
				base.Invoke(new Action<NewFrameEventArgs>(this.AddtoListivew), new object[]
				{
					eventArgs
				});
			}
			else
			{
				this.AddtoListivew(eventArgs);
			}
		}

		private void btn_Save_Click(object sender, EventArgs e)
		{
			this._images.Clear();
			foreach (ListViewItem listViewItem in this.listView1.Items)
			{
				this._images.Add(listViewItem.Text);
			}
			bool flag = base.Parent is Form;
			if (flag)
			{
				(base.Parent as Form).DialogResult = DialogResult.OK;
				(base.Parent as Form).Close();
			}
		}

		public List<string> GetImages()
		{
			return this._images;
		}

		private void btn_Setting_Click(object sender, EventArgs e)
		{
			bool flag = this._videoSource1 == null;
			if (flag)
			{
				this._videoSource1 = new VideoCaptureDevice(this._videoDevices[this.comboBox_Cams.SelectedIndex].MonikerString);
			}
			bool flag2 = this._videoSource1 != null && this._videoSource1 != null;
			if (flag2)
			{
				try
				{
					this._videoSource1.DisplayPropertyPage(base.Handle);
				}
				catch (NotSupportedException ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		public new void Dispose()
		{
			try
			{
				bool flag = this._videoSource1 != null;
				if (flag)
				{
					this._videoSource1.Stop();
					this._videoSource1 = null;
				}
			}
			catch
			{
			}
			base.Dispose();
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem listViewItem in this.listView1.Items)
			{
				File.Delete(listViewItem.Text);
			}
			bool flag = base.Parent is Form;
			if (flag)
			{
				(base.Parent as Form).DialogResult = DialogResult.Cancel;
				(base.Parent as Form).Close();
			}
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Right;
			if (flag)
			{
				bool flag2 = this.listView1.SelectedItems.Count > 0;
				if (flag2)
				{
					ContextMenu contextMenu = new ContextMenu();
					MenuItem menuItem = new MenuItem();
					menuItem.Text = "删除";
					menuItem.Click += new EventHandler(this.menudelitem_Click);
					contextMenu.MenuItems.Add(menuItem);
					MenuItem menuItem2 = new MenuItem();
					menuItem2.Text = "使用本地程序浏览";
					menuItem2.Click += new EventHandler(this.menuexitem_Click);
					contextMenu.MenuItems.Add(menuItem2);
					contextMenu.Show(this.listView1, e.Location);
				}
			}
		}

		private void menuexitem_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", "\"" + this.listView1.SelectedItems[0].Text + "\"");
		}

		private void menudelitem_Click(object sender, EventArgs e)
		{
			File.Delete(this.listView1.SelectedItems[0].Text);
			this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
		}

		private void btn_Record_Click(object sender, EventArgs e)
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
			this.label1 = new Label();
			this.listView1 = new ListView();
			this.comboBox_Cams = new ComboBox();
			this.videoSourcePlayer1 = new VideoSourcePlayer();
			this.btn_Setting = new Button();
			this.startButton = new Button();
			this.btn_Capture = new Button();
			this.btn_Save = new Button();
			this.btn_Cancel = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(20, 12);
			this.label1.Name = "label1";
			this.label1.Size = new Size(52, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "摄像头";
			this.listView1.Location = new Point(13, 439);
			this.listView1.Margin = new Padding(3, 2, 3, 2);
			this.listView1.Name = "listView1";
			this.listView1.Size = new Size(672, 133);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.MouseClick += new MouseEventHandler(this.listView1_MouseClick);
			this.comboBox_Cams.FormattingEnabled = true;
			this.comboBox_Cams.Location = new Point(101, 8);
			this.comboBox_Cams.Margin = new Padding(3, 2, 3, 2);
			this.comboBox_Cams.Name = "comboBox_Cams";
			this.comboBox_Cams.Size = new Size(285, 23);
			this.comboBox_Cams.TabIndex = 2;
			this.videoSourcePlayer1.BackColor = SystemColors.ControlDark;
			this.videoSourcePlayer1.ForeColor = Color.White;
			this.videoSourcePlayer1.Location = new Point(13, 41);
			this.videoSourcePlayer1.Margin = new Padding(4, 4, 4, 4);
			this.videoSourcePlayer1.Name = "videoSourcePlayer1";
			this.videoSourcePlayer1.Size = new Size(673, 399);
			this.videoSourcePlayer1.TabIndex = 0;
			this.videoSourcePlayer1.VideoSource = null;
			this.btn_Setting.Location = new Point(525, 4);
			this.btn_Setting.Margin = new Padding(3, 2, 3, 2);
			this.btn_Setting.Name = "btn_Setting";
			this.btn_Setting.Size = new Size(112, 32);
			this.btn_Setting.TabIndex = 4;
			this.btn_Setting.Text = "设置";
			this.btn_Setting.UseVisualStyleBackColor = true;
			this.btn_Setting.Click += new EventHandler(this.btn_Setting_Click);
			this.startButton.Location = new Point(393, 4);
			this.startButton.Margin = new Padding(3, 2, 3, 2);
			this.startButton.Name = "startButton";
			this.startButton.Size = new Size(112, 32);
			this.startButton.TabIndex = 5;
			this.startButton.Text = "启动";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Visible = false;
			this.startButton.Click += new EventHandler(this.btn_Start_Click);
			this.btn_Capture.Location = new Point(20, 578);
			this.btn_Capture.Margin = new Padding(3, 2, 3, 2);
			this.btn_Capture.Name = "btn_Capture";
			this.btn_Capture.Size = new Size(96, 35);
			this.btn_Capture.TabIndex = 6;
			this.btn_Capture.Text = "拍照";
			this.btn_Capture.UseVisualStyleBackColor = true;
			this.btn_Capture.Click += new EventHandler(this.btn_Capture_Click);
			this.btn_Save.Location = new Point(396, 578);
			this.btn_Save.Margin = new Padding(3, 2, 3, 2);
			this.btn_Save.Name = "btn_Save";
			this.btn_Save.Size = new Size(96, 35);
			this.btn_Save.TabIndex = 7;
			this.btn_Save.Text = "确定";
			this.btn_Save.UseVisualStyleBackColor = true;
			this.btn_Save.Click += new EventHandler(this.btn_Save_Click);
			this.btn_Cancel.Location = new Point(577, 578);
			this.btn_Cancel.Margin = new Padding(3, 2, 3, 2);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new Size(96, 35);
			this.btn_Cancel.TabIndex = 8;
			this.btn_Cancel.Text = "放弃";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.btn_Save);
			base.Controls.Add(this.btn_Capture);
			base.Controls.Add(this.startButton);
			base.Controls.Add(this.btn_Setting);
			base.Controls.Add(this.videoSourcePlayer1);
			base.Controls.Add(this.comboBox_Cams);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.label1);
			base.Margin = new Padding(3, 2, 3, 2);
			base.Name = "UCPreviewer";
			base.Size = new Size(709, 616);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
