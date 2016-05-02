using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    internal class Enum2DataSource
    {
        // Fields
        private static Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        // Methods
        public object DataSouce<T>()
        {
            if (_cache.ContainsKey(typeof(T)))
            {
                return _cache[typeof(T)];
            }
            else
            {
                var list = Enum.GetValues(typeof(T)).Cast<T>().Select(p => new { Key = (int)Enum.Parse(typeof(T), p.ToString()), Value = p.ToString() }).ToList();
                _cache[typeof(T)] = list;
                return list;
            }
        }
    }

}
