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

        private static List<BatchTemplateDef> _tempates;

        static BatchTemplateMgr()
        {
            string text = SystemHelper.GetAssemblesDirectory() + "_batchtemplate.dat";
            try
            {
                BatchTemplateMgr._tempates = SerializeHelper.DeSerializeFromXML<List<BatchTemplateDef>>(text);
            }
            catch
            {
                File.Delete(text);
            }
            if (BatchTemplateMgr._tempates == null)
            {
                BatchTemplateMgr._tempates = new List<BatchTemplateDef>();
            }
        }

        public static void RemoveTemplate(string itemname)
        {
            if (BatchTemplateMgr._tempates.RemoveAll((BatchTemplateDef o) => o.Name == itemname) > 0)
            {
                BatchTemplateMgr.SaveTemplates();
            }
        }

        public static void AddUpdateTemplate(BatchTemplateDef template)
        {
            if (template != null)
            {
                BatchTemplateMgr._tempates.RemoveAll((BatchTemplateDef o) => o.Name == template.Name);
                BatchTemplateMgr._tempates.Add(template);
                //BatchTemplateMgr.SaveTemplates();
            }
        }

        public static BatchTemplateDef GetTempalte(string name)
        {
            return (from o in BatchTemplateMgr._tempates
                    where o.Name == name
                    select o).First<BatchTemplateDef>();
        }

        public static List<BatchTemplateDef> GetTemplates()
        {
            return BatchTemplateMgr._tempates;
        }

        public static bool ContainTemplate(string Name)
        {
            bool result;
            if (BatchTemplateMgr._tempates == null || BatchTemplateMgr._tempates.Count == 0)
            {
                result = false;
            }
            else
            {
                result = BatchTemplateMgr._tempates.Find((BatchTemplateDef o) => o.Name == Name) != null;
            }
            return result;
        }

        public static void SaveTemplates()
        {
            string fname = SystemHelper.GetAssemblesDirectory() + "_batchtemplate.dat";
            SerializeHelper.SerializeToXML<List<BatchTemplateDef>>(BatchTemplateMgr._tempates, fname);
        }
    }
}
