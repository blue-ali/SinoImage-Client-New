using System;

namespace DocScanner.Bean
{
	public interface IUCView
	{
		NFileInfo CurFileInfo
		{
			get;
			set;
		}

		bool ProcessCMD(string cmd);

		string[] GetSupportTypeExt();
	}
}
