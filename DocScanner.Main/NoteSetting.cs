using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class NoteSetting : AbstractSetting<NoteSetting>, IPropertiesSetting
    {
        private Font _notefont;

        [Browsable(false)]
        public override string Name
        {
            get
            {
                return "批注设置";
            }
        }

        [Category("作图窗口设置"), DisplayName("批注颜色")]
        public Color NoteColor
        {
            get
            {
                Color color = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "UINoteColor").ToColor();
                bool flag = color == Color.FromArgb(0, 0, 0, 0);
                if (flag)
                {
                    color = Color.Green;
                }
                return color;
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("UISetting", "UINoteColor", value.ToArgb().ToString());
            }
        }

        [Category("作图窗口设置"), DisplayName("当前选择批注颜色")]
        public Color HightNoteColor
        {
            get
            {
                Color color = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("ImagePane", "SelectPenColor").ToColor();
                bool flag = color == Color.FromArgb(0, 0, 0, 0);
                if (flag)
                {
                    color = Color.Yellow;
                }
                return color;
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("ImagePane", "SelectPenColor", value.ToArgb().ToString());
            }
        }

        [Category("作图窗口设置"), DisplayName("批注字体大小")]
        public float NoteFontSize
        {
            get
            {
                float num = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("ImagePane", "NoteFontSize").ToFloat();
                bool flag = num == 0f;
                if (flag)
                {
                    num = 12f;
                }
                return num;
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("ImagePane", "NoteFontSize", value.ToString());
                this._notefont = new Font("", value);
            }
        }

        [Browsable(false)]
        public Font NoteFont
        {
            get
            {
                bool flag = this._notefont == null;
                if (flag)
                {
                    this._notefont = new Font("", this.NoteFontSize);
                }
                return this._notefont;
            }
        }

        public override bool Equals(NoteSetting other)
        {
            throw new NotImplementedException();
        }
    }
}
