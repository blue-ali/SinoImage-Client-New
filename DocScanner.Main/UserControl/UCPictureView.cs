using DocScaner.Common;
using DocScanner.Bean;
using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocScanner.Main.UC
{
    public partial class UCPictureView : System.Windows.Forms.UserControl, IHasIPropertiesSetting, IUCView
    {
        private enum ZoomOpe
        {
            None,
            FitW,
            FitH,
            CustSize
        }

        public class NestSetting : IPropertiesSetting
        {
            private UCPictureView _parent;

            [Browsable(false)]
            public string Name
            {
                get
                {
                    return "界面设置-中心作图窗口参数";
                }
            }

            [Browsable(false), Category("作图窗口设置"), DisplayName("是否显示滚动条")]
            public bool Scrollable
            {
                get
                {
                    return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("Centerpane", "Scrollable") == "True";
                }
                set
                {
                    bool flag = value != this.Scrollable;
                    if (flag)
                    {
                        string text = value.ToString();
                        LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("Centerpane", "Scrollable", value.ToString());
                    }
                    bool flag2 = value;
                    if (flag2)
                    {
                        this._parent.AutoScroll = true;
                        this._parent.pictureBox1.Dock = DockStyle.None;
                        this._parent.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                        this._parent.SizeChanged += new EventHandler(this._parent.UCCenterImg_SizeChanged);
                    }
                    else
                    {
                        this._parent.AutoScroll = false;
                        this._parent.pictureBox1.Dock = DockStyle.Fill;
                        this._parent.SizeChanged -= new EventHandler(this._parent.UCCenterImg_SizeChanged);
                    }
                }
            }

            [Category("作图窗口设置"), DisplayName("选择框颜色")]
            public Color SelectPenColor
            {
                get
                {
                    Color color = LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("ImagePane", "SelectPenColor").ToColor();
                    bool flag = color == Color.FromArgb(0, 0, 0, 0);
                    if (flag)
                    {
                        color = Color.Red;
                    }
                    return color;
                }
                set
                {
                    LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("ImagePane", "SelectPenColor", value.ToArgb().ToString());
                }
            }

            public NestSetting(UCPictureView obj)
            {
                this._parent = obj;
            }
        }

        private NFileInfo _curFileInfo;

        private bool _selected = false;

        private bool _inZoomMode;

        private bool _selecting;

        private Rectangle _selection;

        private NNoteInfo _curHilightNote;

        private bool _isClipCutSelected = false;

        private bool _enbleDoImageUndo = true;

        private Image _zoombefore;

        private UCPictureView.NestSetting _setting;

        private Dictionary<string, Action> _stringCMDs = new Dictionary<string, Action>();

        private string[] _supportexts;

        

        public Image _orignPicture;

        private float Curratio
        {
            get;
            set;
        }

        private UCPictureView.ZoomOpe Lastsizeope
        {
            get
            {
                return (UCPictureView.ZoomOpe)IniConfigSetting.Cur.GetConfigParamValue("CentImgSetting", "LastSizeOpe").ToInt();
            }
            set
            {
                IniConfigSetting arg_19_0 = IniConfigSetting.Cur;
                string arg_19_1 = "CentImgSetting";
                string arg_19_2 = "LastSizeOpe";
                int num = (int)value;
                arg_19_0.SetConfigParamValue(arg_19_1, arg_19_2, num.ToString());
            }
        }

        public NFileInfo CurFileInfo
        {
            get
            {
                return this._curFileInfo;
            }
            set
            {
                this._curFileInfo = value;
                this._curHilightNote = null;
                Image image = null;
                bool flag = this._curFileInfo != null;
                if (flag)
                {
                    bool flag2 = ImageHelper.IsImgExt(this._curFileInfo.LocalPath);
                    if (flag2)
                    {
                        Image image2 = ImageHelper.LoadLocalImage(this._curFileInfo.LocalPath, true);
                        this._zoombefore = image2;
                        this._orignPicture = (Image)image2.Clone();
                        bool flag3 = this.Lastsizeope == UCPictureView.ZoomOpe.FitW;
                        if (flag3)
                        {
                            this.Curratio = (float)this.pictureBox1.Width / (float)this._orignPicture.Width;
                        }
                        else
                        {
                            bool flag4 = this.Lastsizeope == UCPictureView.ZoomOpe.FitH;
                            if (flag4)
                            {
                                this.Curratio = (float)this.pictureBox1.Height / (float)this._orignPicture.Height;
                            }
                            else
                            {
                                bool flag5 = this.Lastsizeope == UCPictureView.ZoomOpe.None;
                                if (flag5)
                                {
                                    this.Curratio = 1f;
                                }
                            }
                        }
                        image = this.pictureBox1.Image;
                        this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                    }
                }
                else
                {
                    image = this.pictureBox1.Image;
                    this.pictureBox1.Image = null;
                }
                bool flag6 = image != null;
                if (flag6)
                {
                    image.Dispose();
                }
            }
        }

        private bool NoPicture
        {
            get
            {
                bool flag = this.pictureBox1.Image != null;
                return !flag;
            }
        }

        public bool ShowedNote
        {
            get;
            set;
        }

        public UCPictureView()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.GetSetting().Scrollable = true;
            this.pictureBox1.MouseDoubleClick += new MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseWheel += new MouseEventHandler(this.pictureBox1_MouseWheel);
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.Paint += new PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new MouseEventHandler(this.pictureBox1_MouseClick);
            base.KeyDown += new KeyEventHandler(this.UCCenterImg_KeyDown);
        }

        public void UCCenterImg_CompressSaveFile()
        {
            EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30);
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = encoderParameter;
            ImageCodecInfo encoder = this.CurFileInfo.GetFileExt().ToImageCodecInfo();
            try
            {
                this._zoombefore.Save(this.CurFileInfo.LocalPath, encoder, encoderParameters);
            }
            catch
            {
            }
            this.UpdateThumbNail();
        }

        public void DoImageUndo()
        {
            bool flag = this.pictureBox1.Image != null && this._enbleDoImageUndo;
            if (flag)
            {
                this.pictureBox1.Image = this._orignPicture;
                this._zoombefore = this._orignPicture;
                this._orignPicture.Save(this.CurFileInfo.LocalPath);
                this.UpdateThumbNail();
            }
        }

        private void UCCenterImg_SizeChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Location = base.ClientRectangle.Location;
            this.pictureBox1.Size = base.ClientRectangle.Size;
        }

        private void UCCenterImg_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Add;
            if (flag)
            {
                this.DoZoomOut();
            }
            else
            {
                bool flag2 = e.KeyCode == Keys.OemMinus;
                if (flag2)
                {
                    this.DoZoomIn();
                }
            }
        }

        public void DoFullScreen()
        {
            bool flag = this.CurFileInfo != null;
            if (flag)
            {
                FormImgFullScreen formImgFullScreen = new FormImgFullScreen(this);
                formImgFullScreen.ShowDialog();
            }
        }

        public void DoPrint()
        {
            bool flag = this._curFileInfo != null && this.pictureBox1.Image != null;
            if (flag)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += delegate (object sender, PrintPageEventArgs e)
                {
                    using (Bitmap bitmap = new Bitmap(this.pictureBox1.Image))
                    {
                        double num = (double)((float)bitmap.Height * 0.1f / ((float)bitmap.Width * 0.1f));
                        int num2 = e.PageBounds.Width - (e.PageSettings.Margins.Left + e.PageSettings.Margins.Right);
                        int height = (int)((double)num2 * num);
                        int x = (e.PageBounds.Width - num2) / 2;
                        e.Graphics.DrawImage(bitmap, x, e.PageBounds.Top + 20, num2, height);
                    }
                };
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;
                bool flag2 = printDialog.ShowDialog() == DialogResult.OK;
                if (flag2)
                {
                    new PrintPreviewDialog
                    {
                        Document = printDialog.Document
                    }.Show();
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool flag = this._selected && e.Button == MouseButtons.Left;
                if (flag)
                {
                }
            }
        }

        public Image getImage()
        {
            return this.pictureBox1.Image;
        }

        public Rectangle GetSelectedRectangle()
        {
            //this.Curratio = (float)this.pictureBox1.Width / (float)this._orignPicture.Width;
            //Rectangle rect = new Rectangle((int)(_selection.X * Curratio), (int)(_selection.Y * Curratio), (int)(_selection.Width * Curratio), (int)(_selection.Height * Curratio));
            //return rect;
            return this._selection;
        }

        public void DoFitViewW()
        {
            this.GetSetting().Scrollable = false;
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.Curratio = (float)this.pictureBox1.Width / (float)this._orignPicture.Width;
                this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                this.Lastsizeope = UCPictureView.ZoomOpe.FitW;
            }
            this.GetSetting().Scrollable = true;
        }

        

        public void DoFitViewH()
        {
            this.GetSetting().Scrollable = false;
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.Curratio = (float)this.pictureBox1.Height / (float)this._zoombefore.Height;
                this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                this.Lastsizeope = UCPictureView.ZoomOpe.FitH;
            }
            this.GetSetting().Scrollable = true;
        }

        public void DoImageFlipHorizontal()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                using (Bitmap bitmap = new Bitmap(this.pictureBox1.Image))
                {
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    this.pictureBox1.Image = bitmap;
                    this._zoombefore.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageFlipVertical()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                using (Bitmap bitmap = new Bitmap(this.pictureBox1.Image))
                {
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                    this.pictureBox1.Image = bitmap;
                    this._zoombefore.RotateFlip(RotateFlipType.Rotate180FlipX);
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoChangeLightness()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                FormValue formValue = new FormValue();
                formValue.Text = "亮度";
                bool flag2 = formValue.ShowDialog() == DialogResult.OK;
                if (flag2)
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.Lightness(formValue.ResultValue);
                    this._zoombefore = this._zoombefore.Lightness(formValue.ResultValue);
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageToComparison()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                FormValue formValue = new FormValue();
                formValue.Text = "对比度";
                bool flag2 = formValue.ShowDialog() == DialogResult.OK;
                if (flag2)
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.KiContrast(formValue.ResultValue);
                    this._zoombefore = this._zoombefore.KiContrast(formValue.ResultValue);
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageToGray()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.pictureBox1.Image = this.pictureBox1.Image.ToGrayBitmap();
                this._zoombefore = this._zoombefore.ToGrayBitmap();
                this.UCCenterImg_CompressSaveFile();
            }
        }

        public void DoRemoveBlackEdge()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.pictureBox1.Image = this.pictureBox1.Image.RemoveBlackEdge();
                this._zoombefore = this._zoombefore.RemoveBlackEdge();
                this.UCCenterImg_CompressSaveFile();
            }
        }

        public void DoImageClipCut()
        {
            this._isClipCutSelected = true;
        }

        public void DoRemoveBlock()
        {
            int nBlockSize = 2;
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                using (new DurTimeJob("降噪中.."))
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.RemoveBlock(nBlockSize);
                    this._zoombefore = this._zoombefore.RemoveBlock(nBlockSize);
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageSoft()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                using (new DurTimeJob("柔化中.."))
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.SoftImage();
                    this._zoombefore = this._zoombefore.SoftImage();
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageSharp()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                using (new DurTimeJob("锐化中..."))
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.SharpImage();
                    this._zoombefore = this._zoombefore.SharpImage();
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoAutoAmendBevel()
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                using (new DurTimeJob("纠偏中..."))
                {
                    this.pictureBox1.Image = this.pictureBox1.Image.AutoDeskew();
                    this._zoombefore = this._zoombefore.AutoDeskew();
                    this.UCCenterImg_CompressSaveFile();
                }
            }
        }

        public void DoImageToBlack()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.pictureBox1.Image = this.pictureBox1.Image.OtsuThreshold();
                this._zoombefore = this._zoombefore.OtsuThreshold();
                this.UCCenterImg_CompressSaveFile();
            }
        }

        public void DoZoomIn()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                bool flag2 = this.Curratio > 5f;
                if (!flag2)
                {
                    this.Curratio *= 1.1f;
                    this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                    LibCommon.AppContext.Cur.MS.LogDebug("DoZoomIn");
                }
            }
        }

        public void DoZoomOut()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                bool flag2 = (double)this.Curratio < 0.1;
                if (!flag2)
                {
                    this.Curratio = 0.909f * this.Curratio;
                    this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                    LibCommon.AppContext.Cur.MS.LogDebug("DoZoomOut");
                }
            }
        }

        public void DoRotateImageL()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.pictureBox1.Image = this.pictureBox1.Image.Rotate(90);
                this._zoombefore = this._zoombefore.Rotate(90);
                this.UCCenterImg_CompressSaveFile();
            }
        }

        public void DoRotateImageR()
        {
            bool flag = this.pictureBox1.Image != null;
            if (flag)
            {
                this.pictureBox1.Image = this.pictureBox1.Image.Rotate(-90);
                this._zoombefore = this._zoombefore.Rotate(-90);
                this.UCCenterImg_CompressSaveFile();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool isClipCutSelected = this._isClipCutSelected;
                if (isClipCutSelected)
                {
                    bool flag = this._selection.Size.Height <= 5 || this._selection.Size.Width <= 5;
                    if (flag)
                    {
                        MessageBox.Show("请重新框选区域。");
                        return;
                    }
                    FormClipCut formClipCut = new FormClipCut();
                    bool flag2 = formClipCut.ShowDialog() == DialogResult.OK;
                    if (flag2)
                    {
                        bool flag3 = this._isClipCutSelected && this.pictureBox1.Image != null;
                        if (flag3)
                        {
                            this.pictureBox1.Image = this.pictureBox1.Image.ImgCut(this._selection);
                            this._zoombefore = this.pictureBox1.Image;
                            this.UCCenterImg_CompressSaveFile();
                            this._isClipCutSelected = false;
                        }
                    }
                    else
                    {
                        this._isClipCutSelected = false;
                    }
                    this._enbleDoImageUndo = true;
                }
                bool flag4 = e.Button == MouseButtons.Right && this.pictureBox1.Image != null;
                if (flag4)
                {
                    ContextMenu contextMenu = new ContextMenu();
                    bool flag5 = this._selected && this._selection.ToNormalRectangle().Contains(e.Location);
                    if (flag5)
                    {
                        MenuItem menuItem = new MenuItem("批注");
                        menuItem.Click += new EventHandler(this.menuNoteitem_Click);
                        contextMenu.MenuItems.Add(menuItem);
                        MenuItem menuItem2 = new MenuItem("框选查看");
                        menuItem2.Click += new EventHandler(this.menuClipView_Click);
                        contextMenu.MenuItems.Add(menuItem2);
                        bool allowMasaic = AbstractSetting<FunctionSetting>.CurSetting.AllowMasaic;
                        if (allowMasaic)
                        {
                            MenuItem menuItem3 = new MenuItem("马赛克");
                            menuItem3.Click += new EventHandler(this.MenuMasic_Click);
                            contextMenu.MenuItems.Add(menuItem3);
                        }
                    }
                    contextMenu.Show(this.pictureBox1, e.Location);
                }
            }
        }

        private void MenuMasic_Click(object sender, EventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                Rectangle region = this._selection.ToNormalRectangle();
                region.X = (int)((float)this._selection.X / this.Curratio);
                region.Y = (int)((float)this._selection.Y / this.Curratio);
                region.Width = (int)((float)this._selection.Width / this.Curratio);
                region.Height = (int)((float)this._selection.Height / this.Curratio);
                int effectWidth = IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "ImgPrcMosaicBlockSize", "10").ToInt();
                Bitmap image = ImageHelper.AddMosaic(this._orignPicture.ToBitmap(), effectWidth, region);
                this.pictureBox1.Image = image;
                this.pictureBox1.Refresh();
            }
        }

        private void menusignaturecheck_Click(object sender, EventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                Image toCheckBitmap = this.pictureBox1.Image.ImgCut(this._selection);
                UCSignatureInfoCheck uCSignatureInfoCheck = new UCSignatureInfoCheck();
                FormContainer formContainer = new FormContainer();
                uCSignatureInfoCheck.ToCheckBitmap = toCheckBitmap;
                formContainer.SetControl(uCSignatureInfoCheck);
                formContainer.ShowDialog();
                this._selected = false;
                this.pictureBox1.Refresh();
            }
        }

        private void menusignature_Click(object sender, EventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                Image clipBitmap = this.pictureBox1.Image.ImgCut(this._selection);
                UCSignatureInfoRecord uCSignatureInfoRecord = new UCSignatureInfoRecord();
                FormContainer formContainer = new FormContainer();
                uCSignatureInfoRecord.ClipBitmap = clipBitmap;
                formContainer.SetControl(uCSignatureInfoRecord);
                formContainer.ShowDialog();
                this._selected = false;
                this.pictureBox1.Refresh();
            }
        }

        private void menuClipView_Click(object sender, EventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                Image cutBit = this.pictureBox1.Image.ImgCut(this._selection);
                new FormClipViewer
                {
                    CutBit = cutBit
                }.ShowDialog();
                this._selected = false;
                this.pictureBox1.Refresh();
            }
        }

        private void menuClipCut_Click(object sender, EventArgs e)
        {
            bool flag = this._isClipCutSelected && this.pictureBox1.Image != null;
            if (flag)
            {
                FormClipCut formClipCut = new FormClipCut();
                this.pictureBox1.Image = this.pictureBox1.Image.ImgCut(this._selection);
                this._zoombefore = this.pictureBox1.Image;
                this.UCCenterImg_CompressSaveFile();
                this._isClipCutSelected = false;
            }
        }

        private void menuNoteitem_Click(object sender, EventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                UCNote uCNote = new UCNote();
                Rectangle selectRegion = this._selection.ToNormalRectangle();
                selectRegion.X = (int)((float)this._selection.X / this.Curratio);
                selectRegion.Y = (int)((float)this._selection.Y / this.Curratio);
                selectRegion.Width = (int)((float)this._selection.Width / this.Curratio);
                selectRegion.Height = (int)((float)this._selection.Height / this.Curratio);
                uCNote.SelectRegion = selectRegion;
                uCNote.ImgInfo = this._curFileInfo;
                uCNote.CutImg = this.pictureBox1.Image.ImgCut(this._selection.ToNormalRectangle());
                FormContainer formContainer = new FormContainer();
                formContainer.SetControl(uCNote);
                bool flag = formContainer.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                }
                this._selected = false;
                this.pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bool selecting = this._selecting;
            if (selecting)
            {
                Pen pen = new Pen(this.GetSetting().SelectPenColor);
                Rectangle rect = this._selection.ToNormalRectangle();
                e.Graphics.DrawRectangle(pen, rect);
            }
            this.ShowNotes(e.Graphics);
        }

        public void ShowNoteRegion(NNoteInfo note)
        {
            this._curHilightNote = note;
            this.pictureBox1.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool flag = e.Button == MouseButtons.Left && this._selecting && this._selection.Size != default(Size) && this.pictureBox1.Image != null;
                if (flag)
                {
                    this._selecting = false;
                    this._selected = true;
                }
                else
                {
                    this._selecting = false;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool selecting = this._selecting;
                if (selecting)
                {
                    this._selection.Width = e.X - this._selection.X;
                    this._selection.Height = e.Y - this._selection.Y;
                    this.pictureBox1.Refresh();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool flag = e.Button == MouseButtons.Left;
                if (flag)
                {
                    bool flag2 = e.X >= 0 && e.Y >= 0 && (float)e.X <= (float)this.pictureBox1.Image.Width * this.Curratio && (float)e.Y <= (float)this.pictureBox1.Height * this.Curratio;
                    if (flag2)
                    {
                        this._selecting = true;
                        this._selection = new Rectangle(new Point(e.X, e.Y), default(Size));
                    }
                }
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            bool noPicture = this.NoPicture;
            if (!noPicture)
            {
                bool flag = !this._inZoomMode;
                if (flag)
                {
                    this._zoombefore = this.pictureBox1.Image;
                    this._inZoomMode = true;
                }
                this._inZoomMode = true;
                bool flag2 = this.pictureBox1.Image != null;
                if (flag2)
                {
                    bool flag3 = e.Delta < 0;
                    if (flag3)
                    {
                        bool flag4 = (double)this.Curratio < 0.1;
                        if (flag4)
                        {
                            return;
                        }
                        this.Curratio = 0.909f * this.Curratio;
                    }
                    else
                    {
                        bool flag5 = this.Curratio > 5f;
                        if (flag5)
                        {
                            return;
                        }
                        this.Curratio *= 1.1f;
                    }
                    this.pictureBox1.Image = this._zoombefore.ScaleByPercent(this.Curratio);
                }
            }
        }

        public void ReloadFile()
        {
            bool flag = this.CurFileInfo != null;
            if (flag)
            {
                bool flag2 = File.Exists(this.CurFileInfo.LocalPath);
                if (flag2)
                {
                    this.pictureBox1.Image = ImageHelper.LoadLocalImage(this.CurFileInfo.LocalPath, true);
                }
                else
                {
                    LibCommon.AppContext.Cur.MS.LogError("文件不存在" + this.CurFileInfo.LocalPath);
                }
            }
        }

        private void ShowNotes(Graphics g)
        {
            bool showedNote = this.ShowedNote;
            if (showedNote)
            {
                bool flag = this._curFileInfo != null && this._curFileInfo.NotesList != null && this._curFileInfo.NotesList.Count > 0;
                if (flag)
                {
                    foreach (NNoteInfo current in this._curFileInfo.NotesList)
                    {
                        Pen pen = new Pen(AbstractSetting<NoteSetting>.CurSetting.NoteColor);
                        Rectangle rect = default(Rectangle);
                        rect.X = (int)(this.Curratio * (float)current.RegionX);
                        rect.Y = (int)(this.Curratio * (float)current.RegionY);
                        rect.Width = (int)(this.Curratio * (float)current.RegionWidth);
                        rect.Height = (int)(this.Curratio * (float)current.RegionHeight);
                        g.DrawRectangle(pen, rect);
                        g.DrawString(string.Concat(new string[]
                        {
                            current.NoteMsg,
                            "(",
                            current.NoteName,
                            " ",
                            current.GetCreateTime().ToViewTime(),
                            ")"
                        }), AbstractSetting<NoteSetting>.CurSetting.NoteFont, pen.Brush, new Point(rect.Left, rect.Bottom));
                    }
                }
            }
            bool flag2 = this._curHilightNote != null;
            if (flag2)
            {
                Pen pen2 = new Pen(AbstractSetting<NoteSetting>.CurSetting.HightNoteColor);
                Rectangle rect2 = default(Rectangle);
                rect2.X = (int)(this.Curratio * (float)this._curHilightNote.RegionX);
                rect2.Y = (int)(this.Curratio * (float)this._curHilightNote.RegionY);
                rect2.Width = (int)(this.Curratio * (float)this._curHilightNote.RegionWidth);
                rect2.Height = (int)(this.Curratio * (float)this._curHilightNote.RegionHeight);
                g.DrawRectangle(pen2, rect2);
                g.DrawString(this._curHilightNote.NoteMsg, SystemFonts.DefaultFont, pen2.Brush, new Point(rect2.Left, rect2.Bottom));
            }
        }

        public void DoAllShowNote()
        {
            this.ShowedNote = !this.ShowedNote;
            this.pictureBox1.Refresh();
        }

        public void UpdateThumbNail()
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("UpdateThumbNail", null);
        }

        public UCPictureView.NestSetting GetSetting()
        {
            bool flag = this._setting == null;
            if (flag)
            {
                this._setting = new UCPictureView.NestSetting(this);
            }
            return this._setting;
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return this.GetSetting();
        }

        public static long FileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        

        public bool ProcessCMD(string cmd)
        {
            bool flag = this._stringCMDs.Count == 0;
            if (flag)
            {
                this._stringCMDs["FullScreen"] = new Action(this.DoFullScreen);
                this._stringCMDs["DoAllShowNote"] = new Action(this.DoAllShowNote);
                this._stringCMDs["DoZoomOut"] = new Action(this.DoZoomOut);
                this._stringCMDs["DoZoomIn"] = new Action(this.DoZoomIn);
                this._stringCMDs["ReLoadImage"] = new Action(this.ReloadFile);
                this._stringCMDs["DoRotateImageL"] = new Action(this.DoRotateImageL);
                this._stringCMDs["DoRotateImageR"] = new Action(this.DoRotateImageR);
                this._stringCMDs["DoImageFlipHorizontal"] = new Action(this.DoImageFlipHorizontal);
                this._stringCMDs["DoImageFlipVertical"] = new Action(this.DoImageFlipVertical);
                this._stringCMDs["DoFitViewHeight"] = new Action(this.DoFitViewH);
                this._stringCMDs["DoFitViewWidth"] = new Action(this.DoFitViewW);
                this._stringCMDs["DoChangeLightness"] = new Action(this.DoChangeLightness);
                this._stringCMDs["DoImageToComparison"] = new Action(this.DoImageToComparison);
                this._stringCMDs["DoImageToGray"] = new Action(this.DoImageToGray);
                this._stringCMDs["DoImageToBlack"] = new Action(this.DoImageToBlack);
                this._stringCMDs["DoImageSharp"] = new Action(this.DoImageSharp);
                this._stringCMDs["DoImageSoft"] = new Action(this.DoImageSoft);
                this._stringCMDs["AutoAmendBevel"] = new Action(this.DoAutoAmendBevel);
                this._stringCMDs["ImageClipCut"] = new Action(this.DoImageClipCut);
                this._stringCMDs["ImageUndo"] = new Action(this.DoImageUndo);
                this._stringCMDs["RemoveBlackEdge"] = new Action(this.DoRemoveBlackEdge);
                this._stringCMDs["RemoveBlock"] = new Action(this.DoRemoveBlock);
                this._stringCMDs["ShowAllNotes"] = new Action(this.DoAllShowNote);
                this._stringCMDs["DoPrint"] = new Action(this.DoPrint);
            }
            bool flag2 = this._stringCMDs.ContainsKey(cmd);
            bool result;
            if (flag2)
            {
                this._stringCMDs[cmd]();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public string[] GetSupportTypeExt()
        {
            bool flag = this._supportexts == null;
            if (flag)
            {
                this._supportexts = FileHelper.GetImageExts();
            }
            return this._supportexts;
        }
    }
}
