using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.UICommon
{
    public class WaterWavePictureBox : PictureBox
    {
        private Timer _timer;

        private WaterWave _waterwave;

        public void StartRipple()
        {
            Control parent = base.Parent;
            while (!(parent is Form))
            {
                parent = parent.Parent;
            }
            this._waterwave = new WaterWave(base.Image as Bitmap);
            this._timer = new Timer();
            this._timer.Interval = 100;
            this._timer.Tick += new EventHandler(this.timer_Tick);
            this._timer.Start();
            this._waterwave.DropStone(this._waterwave.GetNextRadomPos());
            base.MouseClick += new MouseEventHandler(this.WavePictureBox_MouseClick);
        }

        public void StopRipple()
        {
            bool flag = this._timer != null;
            if (flag)
            {
                this._timer.Dispose();
                this._timer = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = this._timer != null;
            if (flag)
            {
                this._timer.Dispose();
                this._timer = null;
            }
            if (disposing)
            {
                bool flag2 = this._waterwave != null;
                if (flag2)
                {
                    this._waterwave.Dispose();
                    this._waterwave = null;
                }
            }
            base.Dispose(disposing);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ThisTuple<bool, Bitmap> frame = this._waterwave.GetFrame();
            bool flag = !frame.Item1;
            if (flag)
            {
                this._waterwave.DropStone(this._waterwave.GetNextRadomPos());
            }
            base.Image = frame.Item2;
        }

        private void WavePictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = base.PointToClient(Control.MousePosition);
            double num = (double)point.X;
            num = num * (double)this._waterwave.Width / (double)base.Width;
            double num2 = (double)point.Y;
            num2 = num2 * (double)this._waterwave.Height / (double)base.Height;
            this._waterwave.DropStone(new Point((int)num, (int)num2));
        }
    }
}
