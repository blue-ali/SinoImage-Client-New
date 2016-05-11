using DocScanner.Bean;
using DocScanner.LibCommon;
using System.ComponentModel;

namespace DocScanner.Main.Setting
{
    public class SummaryPropertiesSetting : IPropertiesSetting
    {
        private static readonly SummaryPropertiesSetting instance = new SummaryPropertiesSetting();

        private SummaryPropertiesSetting() { }

        public static SummaryPropertiesSetting GetInstance()
        {
            return instance;
        }

        [Browsable(false)]
        public string Name
        {
            get
            {
                return "界面设置-详细信息框";
            }
        }

        public bool ProperGridVisialbe
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("RightPropertyPane", "ProperGridVisialbe").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("RightPropertyPane", "ProperGridVisialbe", value.ToString());
            }
        }

    }
}
