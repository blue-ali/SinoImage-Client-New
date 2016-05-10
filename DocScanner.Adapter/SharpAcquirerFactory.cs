using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocScanner.Bean;
using DocScanner.LibCommon;

namespace DocScanner.AdapterFactory
{
    public class SharpAcquirerFactory : IDisposable
	{

		private Dictionary<string, IFileAcquirer> _acqs = new Dictionary<string, IFileAcquirer>();

		public Dictionary<string, IFileAcquirer> Acqs
		{
			get
			{
				return this._acqs;
			}
		}

		public SharpAcquirerFactory()
		{
			this.InitializeAdapters();
		}

		public IFileAcquirer GetAdapter(string name = "")
		{
			bool flag = this._acqs.Count == 0;
			if (flag)
			{
				throw new Exception("程序目录下没有任何采集适配器");
			}
			bool flag2 = string.IsNullOrEmpty(name);
			if (flag2)
			{
				name = AppContext.GetInstance().Config.GetConfigParamValue("AdapterSetting", "DefaultAdapter");
			}
			bool flag3 = string.IsNullOrEmpty(name);
			IFileAcquirer result;
			if (flag3)
			{
				result = this._acqs.First<KeyValuePair<string, IFileAcquirer>>().Value;
			}
			else
			{
				bool flag4 = this._acqs.ContainsKey(name);
				if (!flag4)
				{
					throw new Exception("没有对应" + name + "的采集适配器");
				}
				result = this._acqs[name];
			}
			return result;
		}

		public static void ShowSetting(SharpAcquirerFactory aptf)
		{
			FormContainer formContainer = new FormContainer();
			UCAdapterSetting control = new UCAdapterSetting(aptf);
			formContainer.SetControl(control);
			formContainer.TopLevel = true;
			formContainer.SetKeyEscCloseForm(true);
			formContainer.ShowDialog();
		}

		private bool InitializeAdapters()
		{
			string[] files = Directory.GetFiles(".", "DocScanner.Adapter.dll");
                bool result;
            if (files.Length == 0)
            {
                //TODO 
            }
            else
            {
                //string[] array = files;
                //for (int i = 0; i < array.Length; i++)
                //{
                string assemblyFile = files[0];
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    Type[] exportedTypes = assembly.GetExportedTypes();

                    List<Type> types = exportedTypes.Where<Type>(o => o.Name.EndsWith("Acquirer")).ToList();
                    foreach(Type type in types)
                    {
                        IFileAcquirer fileAcquirer = assembly.CreateInstance(type.FullName) as IFileAcquirer;
                        if (fileAcquirer != null)
                        {
                            this._acqs[fileAcquirer.Name] = fileAcquirer;
                        }
                    }

                    //               IEnumerable<Type> arg_53_0 = exportedTypes;
                    //Func<Type, bool> arg_53_1;
                    //if ((arg_53_1 = SharpAcquirerFactory.<>c.<>9__6_0) == null)
                    //{
                    //	arg_53_1 = (SharpAcquirerFactory.<>c.<>9__6_0 = new Func<Type, bool>(SharpAcquirerFactory.<>c.<>9.<InitializeAdapters>b__6_0));
                    //}
                    //string fullName = arg_53_0.First(arg_53_1).FullName;


                }
                catch (Exception)
                {
                    result = false;
                    return result;
                }
            }
			result = true;
			return result;
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, IFileAcquirer> current in this._acqs)
			{
				current.Value.Dispose();
			}
			this._acqs.Clear();
		}
	}
}
