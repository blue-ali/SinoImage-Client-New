using System;
using System.ComponentModel;

namespace DocScanner.Bean
{
	public interface IPropertiesSetting
	{
		[Browsable(false)]
		string Name
		{
			get;
		}
	}
}
