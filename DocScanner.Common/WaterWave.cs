using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.UICommon
{
    public sealed class WaterWave : IDisposable
    {
        private int[,] _buf1;

        private int[,] _buf2;

        private Bitmap _newImage = null;

        private BitmapData _orgData = null;

        private Bitmap _orgImage = null;

        private unsafe byte* _pOrgBase;

        private Random _radom;

        private int _rippleCount = 90;

        private int _width;

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public unsafe WaterWave(Bitmap bitmap)
        {
            this._orgImage = (Bitmap)bitmap.Clone();
            this._newImage = (Bitmap)bitmap.Clone();
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this._orgData = this._orgImage.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            this._pOrgBase = (byte*)this._orgData.Scan0.ToPointer();
            this._width = this.Width * 3;
            bool flag = this._width % 4 != 0;
            if (flag)
            {
                this._width = 4 * (this._width / 4 + 1);
            }
            this._buf1 = new int[this.Width, this.Height];
            this._buf2 = new int[this.Width, this.Height];
        }

        public void Dispose()
        {
            bool flag = this._orgData != null;
            if (flag)
            {
                this._orgImage.UnlockBits(this._orgData);
            }
            this._orgData = null;
        }

        public void DropStone(Point point)
        {
            try
            {
                this._buf1[point.X, point.Y] -= 100;
                this._rippleCount = 90;
            }
            catch
            {
            }
        }

        public ThisTuple<bool, Bitmap> GetFrame()
        {
            bool flag = this._rippleCount == 0;
            ThisTuple<bool, Bitmap> result;
            if (flag)
            {
                result = new ThisTuple<bool, Bitmap>(false, this._newImage);
            }
            else
            {
                this.RippleSpread();
                this.RenderRipple();
                this._rippleCount--;
                result = new ThisTuple<bool, Bitmap>(true, this._newImage);
            }
            return result;
        }

        public Point GetNextRadomPos()
        {
            bool flag = this._radom == null;
            if (flag)
            {
                this._radom = new Random();
            }
            int x = (int)(this._radom.NextDouble() * (double)this.Width);
            int y = (int)(this._radom.NextDouble() * (double)this.Height);
            Point result = new Point(x, y);
            return result;
        }

        private unsafe void RenderRipple()
        {
            this._newImage = new Bitmap(this.Width, this.Height);
            BitmapData bitmapData = this._newImage.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
            for (int i = 1; i < this.Width - 1; i++)
            {
                for (int j = 1; j < this.Height - 1; j++)
                {
                    int num = this._buf1[i, j - 1] - this._buf1[i, j + 1];
                    int num2 = this._buf1[i - 1, j] - this._buf1[i + 1, j];
                    bool flag = i + num < 0 || i + num >= this.Width || j + num2 < 0 || j + num2 >= this.Height;
                    if (!flag)
                    {
                        (ptr + j * this._width)[i * 3] = (this._pOrgBase + (j + num2) * this._width)[(i + num) * 3];
                        (ptr + j * this._width + i * 3)[1] = (this._pOrgBase + (j + num2) * this._width + (i + num) * 3)[1];
                        (ptr + j * this._width + i * 3)[2] = (this._pOrgBase + (j + num2) * this._width + (i + num) * 3)[2];
                    }
                }
            }
            this._newImage.UnlockBits(bitmapData);
        }

        private void RippleSpread()
        {
            for (int i = 1; i < this.Width - 1; i++)
            {
                for (int j = 1; j < this.Height - 1; j++)
                {
                    this._buf2[i, j] = (this._buf1[i - 1, j] + this._buf1[i + 1, j] + this._buf1[i, j - 1] + this._buf1[i, j + 1] >> 1) - this._buf2[i, j];
                    this._buf2[i, j] -= this._buf2[i, j] >> 5;
                }
            }
            int[,] buf = this._buf1;
            this._buf1 = this._buf2;
            this._buf2 = buf;
        }
    }
}
