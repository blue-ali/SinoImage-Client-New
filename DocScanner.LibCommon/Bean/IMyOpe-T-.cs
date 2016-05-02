using System;

namespace DocScanner.Bean
{
	public interface IMyOpe<T> where T : class
	{
		event EventHandler DataChanged;

		T OrigData
		{
			get;
			set;
		}

		T MyClone();

		bool MyEqual(T right);

		T FromPBMsg(object obj);
	}
}
