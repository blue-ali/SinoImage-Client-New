using DocScanner.Bean;
using DocScanner.LibCommon;
using Sunisoft.IrisSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class UISetting : AbstractSetting<UISetting>, IPropertiesSetting
    {
        private SkinEngine _skin;

        [Category("界面设置"), Description("CurTheme"), TypeConverter(typeof(SkinTypeConverter))]
        public string Theme
        {
            get
            {
                return LibCommon.AppContext.Cur.Cfg.GetConfigParamValue("UISetting", "CurTheme");
            }
            set
            {
                bool flag = value != this.Theme;
                if (flag)
                {
                    LibCommon.AppContext.Cur.Cfg.SetConfigParamValue("UISetting", "CurTheme", value);
                    this.UpdateSkin();
                }
            }
        }

        public override string Name
        {
            get
            {
                return "界面设置";
            }
        }

        public void UpdateSkin()
        {
            bool flag = string.IsNullOrEmpty(this.Theme) || this.Theme == "None";
            if (flag)
            {
                bool flag2 = this._skin != null;
                if (flag2)
                {
                    this._skin.SkinAllForm = false;
                    this._skin.SkinFormOnly = true;
                    this._skin.SkinFile = "";
                }
            }
            else
            {
                string path = Path.Combine(SystemHelper.GetAssemblesDirectory(), "Resources", "skins");
                bool flag3 = this._skin == null;
                if (flag3)
                {
                    this._skin = new SkinEngine();
                }
                this._skin.SkinAllForm = false;
                this._skin.SkinFormOnly = true;
                string skinFile = Path.Combine(path, this.Theme + ".ssk");
                this._skin.SkinFile = skinFile;
            }
        }

        public override bool Equals(UISetting other)
        {
            return true;
        }
    }
}
