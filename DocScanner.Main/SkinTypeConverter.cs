using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;
using System.Collections.Generic;
using System.IO;

namespace DocScanner.Main
{
    public class SkinTypeConverter : StringTypeConverter
    {
        public const string None = "None";

        public override void InitItems()
        {
            this._items = new List<string>();
            this._items.Add("None");
            string path = SystemHelper.ResourceDir + "skins\\";
            string[] files = Directory.GetFiles(path);
            string[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                string fname = array[i];
                this._items.Add(FileHelper.GetFileNameNoExt(fname));
            }
        }
    }
}
