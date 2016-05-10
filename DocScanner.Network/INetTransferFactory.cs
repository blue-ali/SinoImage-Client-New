using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocScanner.Network
{
    public static class INetTransferFactory
	{
		private static Dictionary<string, Type> _supporttransfers;

		public static INetTransfer GetNetTransfer()
		{
			string configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "NetTransferType");
			bool flag = string.IsNullOrEmpty(configParamValue);
			INetTransfer result;
			if (flag)
			{
				result = new HttpNetTransfer();
			}
			else
			{
				result = (Activator.CreateInstance(INetTransferFactory._supporttransfers[configParamValue]) as INetTransfer);
			}
			return result;
		}

		public static List<string> GetSupportTransferType()
		{
			return INetTransferFactory._supporttransfers.Keys.ToList<string>();
		}

		static INetTransferFactory()
		{
			INetTransferFactory._supporttransfers = new Dictionary<string, Type>();
			INetTransferFactory._supporttransfers["LocalFileMode"] = typeof(LocalFileTransfer);
			INetTransferFactory._supporttransfers["HttpMode"] = typeof(HttpNetTransfer);
			INetTransferFactory._supporttransfers["ThreadHttpMode"] = typeof(ThreadNetTransfer);
			INetTransferFactory._supporttransfers["ThreadLocalFileMode"] = typeof(ThreadLocalFileTransfer);
		}
	}
}
