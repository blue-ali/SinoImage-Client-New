using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class BatchTemplateMgr
    {
        public const string TemplateFileNmae = "_batchtemplate.dat";

        private static List<BatchTemplatedef> _tempates;

        static BatchTemplateMgr()
        {
            string text = SystemHelper.GetAssemblesDirectory() + "_batchtemplate.dat";
            try
            {
                BatchTemplateMgr._tempates = SerializeHelper.DeSerializeFromXML<List<BatchTemplatedef>>(text);
            }
            catch
            {
                File.Delete(text);
            }
            bool flag = BatchTemplateMgr._tempates == null;
            if (flag)
            {
                BatchTemplateMgr._tempates = new List<BatchTemplatedef>();
            }
        }

        public static void RemoveTemplate(string itemname)
        {
            bool flag = BatchTemplateMgr._tempates.RemoveAll((BatchTemplatedef o) => o.Name == itemname) > 0;
            if (flag)
            {
                BatchTemplateMgr.SaveTemplates();
            }
        }

        public static void AddUpdateTemplate(BatchTemplatedef template)
        {
            bool flag = template != null;
            if (flag)
            {
                BatchTemplateMgr._tempates.RemoveAll((BatchTemplatedef o) => o.Name == template.Name);
                BatchTemplateMgr._tempates.Add(template);
                BatchTemplateMgr.SaveTemplates();
            }
        }

        public static BatchTemplatedef GetTempalte(string name)
        {
            return (from o in BatchTemplateMgr._tempates
                    where o.Name == name
                    select o).First<BatchTemplatedef>();
        }

        public static List<BatchTemplatedef> GetTemplates()
        {
            return BatchTemplateMgr._tempates;
        }

        public static bool ContainTemplate(string Name)
        {
            bool flag = BatchTemplateMgr._tempates == null || BatchTemplateMgr._tempates.Count == 0;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = BatchTemplateMgr._tempates.Find((BatchTemplatedef o) => o.Name == Name) != null;
                result = flag2;
            }
            return result;
        }

        public static void SaveTemplates()
        {
            string fname = SystemHelper.GetAssemblesDirectory() + "_batchtemplate.dat";
            SerializeHelper.SerializeToXML<List<BatchTemplatedef>>(BatchTemplateMgr._tempates, fname);
        }
    }
}
