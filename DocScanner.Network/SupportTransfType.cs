using DocScanner.LibCommon;

namespace DocScanner.Network
{
    public class SupportTransfType : StringTypeConverter
	{
		public override void InitItems()
		{
			this._items.AddRange(INetTransferFactory.GetSupportTransferType());
		}
	}
}
