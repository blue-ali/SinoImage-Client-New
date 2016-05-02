using System;
using System.Drawing;
using System.Text;

namespace DocScanner.LibCommon
{
    public static class ConvertHelper
    {
        // Methods
        public static string FixPaddingString(string str, int len, char fillchar = '0')
        {
            if (str == null)
            {
                str = "";
            }
            if (str.Length < len)
            {
                str = new string(fillchar, len - str.Length) + str;
                return str;
            }
            if (str.Length > len)
            {
                str = str.Substring(0, len);
            }
            return str;
        }

        public static string FormatCurrToStr(string str)
        {
            double result = 0.0;
            if (double.TryParse(str, out result))
            {
                return result.ToString();
            }
            return str;
        }

        public static string FormatStrToCurr(string str)
        {
            if (!str.Equals(""))
            {
                return FormatStrToCurr(str, true);
            }
            return "0";
        }

        public static string FormatStrToCurr(string str, bool needSplit)
        {
            double result = 0.0;
            if (double.TryParse(str, out result))
            {
                result /= 100.0;
                if (needSplit)
                {
                    return result.ToString("N2");
                }
                return result.ToString("#0.00");
            }
            return str;
        }

        public static string RectToStr(this Rectangle rect)
        {
            object[] objArray1 = new object[] { rect.Left, ",", rect.Top, ",", rect.Width, ",", rect.Height };
            return string.Concat(objArray1);
        }

        public static Color RGBStringToColor(string strRGB)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = strRGB.Split(separator);
            int red = (strArray.Length != 0) ? Convert.ToInt32(strArray[0]) : 0xeb;
            int green = (strArray.Length > 1) ? Convert.ToInt32(strArray[1]) : 0xeb;
            int blue = (strArray.Length > 2) ? Convert.ToInt32(strArray[2]) : 0xeb;
            return Color.FromArgb(red, green, blue);
        }

        public static Color StrToColor(string color)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = color.Split(separator);
            int red = (strArray.Length != 0) ? strArray[0].Trim().StrToIntDef(0) : 0;
            int green = (strArray.Length > 1) ? strArray[1].Trim().StrToIntDef(0) : 0;
            int blue = (strArray.Length > 2) ? strArray[2].Trim().StrToIntDef(0) : 0;
            return Color.FromArgb(red, green, blue);
        }

        public static Color StrToColor(string color, Color defaultColor)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = color.Split(separator);
            int red = (strArray.Length != 0) ? strArray[0].Trim().StrToIntDef(defaultColor.R) : defaultColor.R;
            int green = (strArray.Length > 1) ? strArray[1].Trim().StrToIntDef(defaultColor.G) : defaultColor.G;
            int blue = (strArray.Length > 2) ? strArray[2].Trim().StrToIntDef(defaultColor.B) : defaultColor.B;
            return Color.FromArgb(red, green, blue);
        }

        public static int StrToIntDef(this string Value, int defaultvalue)
        {
            int num = defaultvalue;
            try
            {
                num = int.Parse(Value);
            }
            catch
            {
            }
            return num;
        }

        public static Rectangle StrToRect(this string rect)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = rect.Split(separator);
            int x = (strArray.Length != 0) ? strArray[0].Trim().StrToIntDef(0) : 0;
            int y = (strArray.Length > 1) ? strArray[1].Trim().StrToIntDef(0) : 0;
            int width = (strArray.Length > 2) ? strArray[2].Trim().StrToIntDef(0) : 0;
            return new Rectangle(x, y, width, (strArray.Length > 3) ? strArray[3].Trim().StrToIntDef(0) : 0);
        }

        public static bool ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return (value.ToLower() == "true");
        }

        public static Color ToColor(this string color)
        {
            int num;
            if (string.IsNullOrEmpty(color))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(color);
            }
            return Color.FromArgb(num);
        }

        public static string ToCommentString(this int x)
        {
            if (x == 0)
            {
                return "";
            }
            return ("[" + x + "]");
        }

        public static float ToFloat(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0f;
            }
            try
            {
                return float.Parse(value);
            }
            catch
            {
                return 0f;
            }
        }

        public static int ToInt(this string Value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return 0;
            }
            int num = 0;
            try
            {
                num = int.Parse(Value);
            }
            catch
            {
            }
            return num;
        }
    }


}