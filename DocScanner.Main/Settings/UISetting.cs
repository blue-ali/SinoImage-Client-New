using DocScanner.Bean;
using DocScanner.LibCommon;
using Sunisoft.IrisSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace DocScanner.Main
{
    public class UISetting : IPropertiesSetting
    {
        private static readonly UISetting instance = new UISetting();

        private UISetting() { }

        public static UISetting GetInstance()
        {
            return instance;
        }

        private SkinEngine _skin;

        [Category("界面设置"), Description("CurTheme"), TypeConverter(typeof(SkinTypeConverter))]
        public string Theme
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "CurTheme");
            }
            set
            {
                if (value != this.Theme)
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("UISetting", "CurTheme", value);
                    this.UpdateSkin();
                }
            }
        }

        public string Name
        {
            get
            {
                return "界面设置";
            }
        }

        //[Browsable(false)]
        //public string Name
        //{
        //    get
        //    {
        //        return "界面设置-主界面";
        //    }
        //}

        [Browsable(false), Category("主窗口设置"), DisplayName("右边工具栏宽度")]
        public float RightPaneWidth
        {
            get
            {
                int num = AppContext.GetInstance().Config.GetConfigParamValue("UCBench", "RightPaneWidth").ToInt();
                if (num == 0)
                {
                    num = 0x110;
                }
                return (float)num;
            }
            set
            {
                AppContext.GetInstance().Config.SetConfigParamValue("UCBench", "RightPaneWidth", value.ToString());
            }
        }

        [Category("主窗口设置"), DisplayName("顶部工具栏高度")]
        public float TopPaneHeight
        {
            get
            {
                return (float)AppContext.GetInstance().Config.GetConfigParamValue("UCBench", "TopPaneHeight").ToInt();
            }
            set
            {
                AppContext.GetInstance().Config.SetConfigParamValue("UCBench", "TopPaneHeight", value.ToString());
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

        [Category("用户UI设置"), DisplayName("是否显示按钮文字")]
        public bool ShowButtonText
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("topPane", "ShowButtonText").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("topPane", "ShowButtonText", value.ToString());
            }
        }

        [Category("用户UI设置"), DisplayName("背景色,如不存在背景图片，则此效果出现")]
        public Color BarBackColor
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("topPane", "BarBackColor").ToColor();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("topPane", "BarBackColor", value.ToArgb().ToString());
            }
        }

        [Category("用户UI设置"), DisplayName("背景图片"), Editor(typeof(FileNameEditor), typeof(UITypeEditor)), EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
        public string BarBackImage
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("topPane", "BarBackImage");
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("topPane", "BarBackImage", value.ToString());
            }
        }

        [Category("用户UI设置"), DisplayName("按钮大小")]
        public int ButtonSize
        {
            get
            {
                int num = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("topPane", "ButtonSize").ToInt();
                return (num == 0) ? 96 : num;
            }
            set
            {
                bool flag = value != this.ButtonSize;
                if (flag)
                {
                    LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("topPane", "ButtonSize", value.ToString());
                }
            }
        }

        [Category("用户界面定义"), Description("允许自定义层次子节点")]
        public bool AllowCreateCategory
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "AllowCreateCategory").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "AllowCreateCategory", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("允许移除菜单")]
        public bool AllowDelMenu
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "AllowDelMenu").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "AllowDelMenu", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("批次号节点高度")]
        public int BatchNodeHeight
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "BatchNodeHeight");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 0x20;
                }
                return int.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "BatchNodeHeight", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("文件节点标题类型")]
        public ENFileNodeTitleType FileNodeTitleType
        {
            get
            {
                return NodeTitleTypeSetting.FileNodeTitleType;
            }
            set
            {
                NodeTitleTypeSetting.FileNodeTitleType = value;
            }
        }

        /*
        [Category("用户界面定义"), Description("节点字体大小")]
        public float Lev1NodeFontSize
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "Lev1NodeFontSize");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 14f;
                }
                return float.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "Lev1NodeFontSize", value.ToString());
            }
        }*/


        [Category("用户界面定义"), Description("缩略图尺寸")]
        public int ThumbImgSize
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "ThumbImgSize");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 0x60;
                }
                return int.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "ThumbImgSize", value.ToString());
            }
        }
    }
}
