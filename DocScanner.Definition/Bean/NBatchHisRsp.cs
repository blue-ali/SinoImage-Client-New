using DocScanner.Bean.pb;
using System.Collections.Generic;

namespace DocScanner.Bean
{
    public class NBatchHisRsp
	{
		private List<NBatchInfo> _batchs = new List<NBatchInfo>();

		public List<NBatchInfo> Batchs
		{
			get
			{
				return this._batchs;
			}
		}

		public NResultInfo Result
		{
			get;
			set;
		}

		public static NBatchHisRsp FromPBMsg(MsgBatchHisRsp input)
		{
			NBatchHisRsp nBatchHisRsp = new NBatchHisRsp();
			bool flag = input != null && input.Batchs1List != null;
			if (flag)
			{
				foreach (MsgBatchInfo current in input.Batchs1List)
				{
					nBatchHisRsp._batchs.Add(NBatchInfo.FromPBMsg(current));
				}
			}
			return nBatchHisRsp;
		}
	}
}
