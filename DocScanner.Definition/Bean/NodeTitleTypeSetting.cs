using DocScanner.LibCommon;
using System;

namespace DocScanner.Bean
{
    public class NodeTitleTypeSetting
	{
		public static ENFileNodeTitleType FileNodeTitleType
		{
			get
			{
				string configParamValue = AppContext.Cur.Cfg.GetConfigParamValue("LeftPaneSetting", "FileNodeTitleType");
				bool flag = string.IsNullOrEmpty(configParamValue);
				ENFileNodeTitleType result;
				if (flag)
				{
					result = ENFileNodeTitleType.FileName;
				}
				else
				{
					object obj = Enum.Parse(typeof(ENFileNodeTitleType), configParamValue);
					result = (ENFileNodeTitleType)obj;
				}
				return result;
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("LeftPaneSetting", "FileNodeTitleType", value.ToString());
			}
		}
	}
}
