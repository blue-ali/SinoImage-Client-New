using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using DocScanner.Main.UC;
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
    public class FormImgFullScreen : FormBase
    {
        private UCPictureView _parent;

        private IContainer components = null;

        public PictureBox ImageScreen;

        public FormImgFullScreen(UCPictureView parent)
        {
            this.InitializeComponent();
            this._parent = parent;
            this.ShowParentPic();
            this.ImageScreen.MouseDoubleClick += new MouseEventHandler(this.ImageScreen_MouseDoubleClick);
        }

        private void ImageScreen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.Close();
        }

        private void ShowParentPic()
        {
            bool flag = this._parent.CurFileInfo != null && !string.IsNullOrEmpty(this._parent.CurFileInfo.LocalPath) && ImageHelper.IsImgExt(this._parent.CurFileInfo.LocalPath);
            if (flag)
            {
                this.ImageScreen.Image = ImageHelper.LoadLocalImage(this._parent.CurFileInfo.LocalPath, true);
            }
        }

        private void FImgFullScreen_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Escape;
            if (flag)
            {
                base.Dispose();
                base.Close();
            }
            else
            {
                bool flag2 = e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Space || e.KeyCode == Keys.Next;
                if (flag2)
                {
                    LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("NavigateNextItem", null);
                }
                else
                {
                    bool flag3 = e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Prior;
                    if (flag3)
                    {
                        LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("NavigatePrevItem", null);
                    }
                    else
                    {
                        bool flag4 = e.KeyCode == Keys.Home;
                        if (flag4)
                        {
                            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("NavigateFirstItem", null);
                        }
                        else
                        {
                            bool flag5 = e.KeyCode == Keys.End;
                            if (flag5)
                            {
                                LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("NavigateLastItem", null);
                            }
                        }
                    }
                }
                this.ShowParentPic();
                this.ImageScreen.Refresh();
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
            this.ImageScreen = new PictureBox();
            ((ISupportInitialize)this.ImageScreen).BeginInit();
            base.SuspendLayout();
            this.ImageScreen.Dock = DockStyle.Fill;
            this.ImageScreen.Location = new Point(0, 0);
            this.ImageScreen.Name = "ImageScreen";
            this.ImageScreen.Size = new Size(1137, 564);
            this.ImageScreen.SizeMode = PictureBoxSizeMode.Zoom;
            this.ImageScreen.TabIndex = 0;
            this.ImageScreen.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1137, 564);
            base.Controls.Add(this.ImageScreen);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FImgFullScreen";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "按ESC退出全屏";
            base.WindowState = FormWindowState.Maximized;
            base.KeyDown += new KeyEventHandler(this.FImgFullScreen_KeyDown);
            ((ISupportInitialize)this.ImageScreen).EndInit();
            base.ResumeLayout(false);
        }
    }
}
