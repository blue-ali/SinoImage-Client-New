using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public abstract class StringTypeConverter : TypeConverter
    {
        // Fields
        protected List<string> _items = new List<string>();

        // Methods
        public StringTypeConverter()
        {
            this.InitItems();
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(this._items);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public abstract void InitItems();
    }

}
