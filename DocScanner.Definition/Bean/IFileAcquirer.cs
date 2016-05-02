using System;

namespace DocScanner.Bean
{
	public interface IFileAcquirer : IHasIPropertiesSetting, IDisposable
	{
		event EventHandler<TEventArg<string>> OnAcquired;

		event EventHandler<TEventArg<string>> OnError;

		bool Initialized
		{
			get;
		}

		string Name
		{
			get;
		}

		string CnName
		{
			get;
		}

		bool Initialize(IAcquirerParam initparam = null);

		bool Acquire();

		void UnInitialize();
	}
}
