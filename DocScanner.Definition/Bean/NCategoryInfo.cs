using System;
using System.ComponentModel;

namespace DocScanner.Bean
{
	public class NCategoryInfo
	{
		private string _cat;

		[Category("分类信息"), DisplayName("名称")]
		public string CategoryName
		{
			get
			{
				return this._cat;
			}
			set
			{
				this._cat = value;
			}
		}

		public NCategoryInfo(string cat)
		{
			this._cat = cat;
		}
	}
}
