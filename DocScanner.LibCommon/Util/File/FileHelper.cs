using DocScanner.LibCommon;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DocScanner.LibCommon.Util
{
    public static class FileHelper
    {
        // Fields
        private const int FO_COPY = 2;
        private const int FO_DELETE = 3;
        private const int FO_MOVE = 1;
        private const ushort FOF_ALLOWUNDO = 0x40;
        private const ushort FOF_NOCONFIRMATION = 0x10;

        public static string[] imgExts = new string[] { "JPG", "GIF", "BMP", "PNG" };

        // Methods
        public static bool CopyFile(string srcpath, string destpath)
        {
            SHFILEOPSTRUCT str = new SHFILEOPSTRUCT
            {
                wFunc = 2,
                pFrom = srcpath,
                pTo = destpath,
                fFlags = 0x40,
                lpszProgressTitle = "另存到"
            };
            return !SHFileOperation(str);
        }

        public static void CreateBack(string fname)
        {
        }

        public static void DoSpecialJob()
        {
            string assemblesDirectory = SystemHelper.GetAssemblesDirectory();
        }

        public static string GetAssociatedExe(string fname)
        {
            return "";
        }

        public static string GetFileDir(string fname)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return "";
            }
            fname = fname.Replace('/', '\\');
            string str = fname.Substring(0, fname.LastIndexOf('\\'));
            if (!str.EndsWith(@"\"))
            {
                str = str + @"\";
            }
            return str;
        }

        public static string GetFileExtNoIncDot(string fname)
        {
            return fname.Substring(fname.LastIndexOf('.') + 1);
        }

        public static string GetFileName(string fname)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return "";
            }
            fname = fname.Replace('/', '\\');
            return fname.Substring(fname.LastIndexOf('\\') + 1);
        }

        public static string GetFileNameNoExt(string fname)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return "";
            }
            fname = fname.Replace('/', '\\');
            string str = fname.Substring(fname.LastIndexOf('\\') + 1);
            if (str.IndexOf('.') != -1)
            {
                str = str.Substring(0, str.IndexOf('.'));
            }
            return str;
        }

        public static Image GetFilesIcon(string fname)
        {
            Icon icon = null;
            if (File.Exists(fname))
            {
                icon = Icon.ExtractAssociatedIcon(fname);
            }
            if (icon != null)
            {
                Image image = Image.FromHbitmap(icon.ToBitmap().GetHbitmap());
                icon.Dispose();
                return image;
            }
            return null;
        }

        public static int GetFileVersion(string fname)
        {
            int num = fname.LastIndexOf("[");
            int num2 = fname.LastIndexOf("]");
            if (((num != -1) && (num2 != -1)) && (num < num2))
            {
                return int.Parse(fname.Substring(num + 1, (num2 - num) - 1));
            }
            return 0;
        }

        //public static string[] GetImageExts()
        //{
            //return new string[] {
            //".ani", ".anm", ".bmp", ".dib", ".rle", ".cdr", ".cur", ".dcm", ".emf", ".gif", ".hdr", ".ico", ".ics", ".jpg", ".jpeg", ".pcd",
            //".png", ".tif", ".tiff"

        // };
        //}

        public static bool HasBacked(string fname)
        {
            return false;
        }

        public static bool IsHttpURL(this string fname)
        {
            if (string.IsNullOrEmpty(fname))
            {
                return false;
            }
            fname = fname.ToLower();
            return (fname.StartsWith("http:") || fname.StartsWith("https:"));
        }

        /*
            public static bool IsImageExt(string fname)
            {
                fname = fname.ToLower();
                foreach (string str in GetImageExts())
                {
                    if (fname.EndsWith(str))
                    {
                        return true;
                    }
                }
                return false;
            }*/
        public static bool IsImageExt(String filePah)
        {
            string ext = GetFileExt(filePah);
            if (imgExts.Contains(ext))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string LocalFile2URL(string fpath)
        {
            fpath = fpath.ToLower().Trim();
            if (fpath.StartsWith("file://"))
            {
                return fpath;
            }
            return ("file://" + fpath.Replace(@"\", "/"));
        }

        public static string SetFileVersion(string fname, int ver)
        {
            int num = fname.LastIndexOf("[");
            int startIndex = fname.LastIndexOf("]");
            if (((num != -1) && (startIndex != -1)) && (num < startIndex))
            {
                return (fname.Substring(0, num + 1) + ver.ToString() + fname.Substring(startIndex));
            }
            if (fname.IndexOf(".") != -1)
            {
                object[] objArray1 = new object[] { GetFileNameNoExt(fname), "[", ver, "].", GetFileExtNoIncDot(fname) };
                return string.Concat(objArray1);
            }
            object[] objArray2 = new object[] { fname, "[", ver, "]" };
            return string.Concat(objArray2);
        }

        /// <summary>
        /// 获取文件真实格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExt(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            string ext = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                ext = buffer.ToString();
                buffer = r.ReadByte();
                ext += buffer.ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                r.Close();
                fs.Close();
            }
            return FileExtUtil.GetExtName(ext.ToInt());
        
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SHFileOperation([In, Out] SHFILEOPSTRUCT str);

        // Properties
        public static string TempPath
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public uint wFunc;
            public string pFrom;
            public string pTo;
            public ushort fFlags;
            public int fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
    }

}