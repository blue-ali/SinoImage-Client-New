using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon.Util
{
    public class FileExtUtil
    {
        public static string[] imgExts = new string[] {"JPG","GIF","BMP","PNG"};

        public static string GetExtName(int extId)
        {
            return Enum.GetName(typeof(EnumFileExt), extId);
        }

        public static bool IsImageExt(String ext)
        {
            if (imgExts.Contains(ext))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
