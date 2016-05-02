using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class AppContextMgr
    {
        // Fields
        private static Dictionary<object, AppContext> _map = new Dictionary<object, AppContext>();

        // Methods
        public static void RemoveContext(object key)
        {
            Dictionary<object, AppContext> dictionary = _map;
            lock (dictionary)
            {
                if (!_map.ContainsKey(key))
                {
                    _map.Remove(key);
                }
            }
        }
    }

}
