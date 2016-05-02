using System;

namespace DocScanner.Bean
{
	public class TEventArg<T> : EventArgs
	{
		private T _arg;

		public T Arg
		{
			get
			{
				return this._arg;
			}
			set
			{
				this._arg = value;
			}
		}

		public TEventArg(T input)
		{
			this._arg = input;
		}
	}
}
