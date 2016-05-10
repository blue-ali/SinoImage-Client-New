using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public static class ExceptionHelper
    {
        // Methods
        static ExceptionHelper()
        {
            Directory.CreateDirectory(GetExpdir());
        }

        public static void DumpException(Exception e)
        {
            FileStream stream = new FileStream(GetExpdir() + GetExceptionFileName(), FileMode.Append);
            StreamWriter writer = new StreamWriter(stream, Encoding.Default);
            writer.Write(e.ToString());
            writer.Close();
            stream.Close();
        }

        public static string GetExceptionFileName()
        {
            return (DateTime.Now.ToString("yyyyMMddHHmmss") + ".exception");
        }

        public static string GetExpdir()
        {
            return (SystemHelper.GetAssemblesDirectory() + @"exception\");
        }

        public static Exception GetFirstException(Exception e)
        {
            if(e.InnerException != null)
            {
                return GetFirstException(e.InnerException);
            }
            else
            {
                return e;
            }
        }
    }


}
