using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Common
{
    public static class EnumManager<TEnum>
    {
        // Methods
        public static DataTable GetDataTable()
        {
            FieldInfo[] fields = typeof(TEnum).GetFields();
            DataTable table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Value", Type.GetType("System.Int32"));
            foreach (FieldInfo info in fields)
            {
                if (!info.IsSpecialName)
                {
                    DataRow row = table.NewRow();
                    row[0] = info.Name;
                    row[1] = Convert.ToInt32(info.GetRawConstantValue());
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static void SetComboxControl(ComboBox ctrl)
        {
            ctrl.DataSource = EnumManager<TEnum>.GetDataTable();
            ctrl.DisplayMember = "Name";
            ctrl.ValueMember = "Value";
        }

        public static void SetListControl(ListControl list)
        {
            list.DataSource = EnumManager<TEnum>.GetDataTable();
            list.DisplayMember = "Name";
            list.ValueMember = "Value";
        }
    }

}
