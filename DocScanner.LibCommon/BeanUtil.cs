using DocScanner.Bean;
using DocScanner.CodeUtils;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class BeanUtil
    {

        public static List<NFileInfo> FileDialog2FileInfo(OpenFileDialog fileDialog, string batchNo)
        {
            string[] fileNames = fileDialog.FileNames;

            return fileNames.Select(x => new NFileInfo { BatchNO = batchNo,
                                                         CreateTime = DateTime.Now.ToString(ConstString.DateFormat),
                                                         LocalPath = x,
                                                         FileMD5 = MD5Helper.GetFileMD5(x),
                                                         FileSize = (int)new FileInfo(x).Length,
                                                         FileName = FileHelper.GetFileName(x) }
                                    ).ToList();
        }

    }
}
