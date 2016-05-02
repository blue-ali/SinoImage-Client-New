using System;
using System.Threading;
using System.Windows.Forms;

namespace DocScaner.Network
{
	public class SpeedReporter : IDisposable
	{
		private bool _firstInvoked;

		private System.Threading.Timer _timer;

		private int SumVal;

		private int LastSecondSumVal = 0;

		public Control Host
		{
			get;
			set;
		}

		public void OnSpeed(int value)
		{
			bool flag = !this._firstInvoked;
			if (flag)
			{
				this._timer = new System.Threading.Timer(new TimerCallback(this.OnTimerCallBack), null, 1000, 1000);
			}
			this._firstInvoked = true;
			this.SumVal += value;
		}

		private void OnTimerCallBack(object state)
		{
			int num = this.SumVal - this.LastSecondSumVal;
			bool flag = this.Host != null;
			if (flag)
			{
			}
			this.LastSecondSumVal = this.SumVal;
		}

		public void Dispose()
		{
			bool flag = this._timer != null;
			if (flag)
			{
				this._timer.Dispose();
				this._timer = null;
			}
		}
	}
}
