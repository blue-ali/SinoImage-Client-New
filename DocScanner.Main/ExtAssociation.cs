using DocScanner.Bean;
using DocScanner.Main;
using DocScanner.Main.UC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class ExtAssociation
    {

        [ThreadStatic]
        private static Dictionary<string, IUCView> _ext2UC = new Dictionary<string, IUCView>();


        static ExtAssociation()
        {
            //ExtAssociation._ext2UC = new Dictionary<string, IUCView>();
            //TODO 改目录，最后都输出到一起
            string[] files = Directory.GetFiles(".", "DocScanner.View.dll");
            if (files.Length == 0)
            {
                //TODO
            }
            else
            {
                string assemblyFile = files[0];
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    Type[] exportedTypes = assembly.GetExportedTypes();
                    //TODO 名字定义好
                    List<Type> types = exportedTypes.Where<Type>(x => x.Name.EndsWith("View")).ToList();
                    //string fullName = exportedTypes.First(new Func<Type, bool>(o => o.Name.EndsWith("View"))).FullName;
                    foreach(Type type in types)
                    {
                        IUCView iUCView = assembly.CreateInstance(type.FullName) as IUCView;
                        bool flag = iUCView != null;
                        if (flag)
                        {
                            string[] supportTypeExt = iUCView.GetSupportTypeExt();
                            string[] array2 = supportTypeExt;
                            for (int j = 0; j < array2.Length; j++)
                            {
                                string key = array2[j];
                                //ExtAssociation._ext2UC[key] = iUCView;
                                _ext2UC.Add(key, iUCView);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LibCommon.AppContext.Cur.MS.LogError(ex.ToString());
                }
            }
            UCPictureView uCPictureView = new UCPictureView();
            string[] supportTypeExt2 = uCPictureView.GetSupportTypeExt();
            for (int k = 0; k < supportTypeExt2.Length; k++)
            {
                string key2 = supportTypeExt2[k];
                //ExtAssociation._ext2UC[key2] = uCPictureView;
                _ext2UC.Add(key2, uCPictureView);
            }

        }

        public static IUCView GetCustomView(string ext)
        {
            bool flag = string.IsNullOrEmpty(ext);
            IUCView result;
            if (flag)
            {
                result = null;
            }
            else
            {
                ext = ext.ToLower();
                foreach (KeyValuePair<string, IUCView> current in ExtAssociation._ext2UC)
                {
                    bool flag2 = ext.Contains(current.Key);
                    if (flag2)
                    {
                        result = current.Value;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }
    }
}
