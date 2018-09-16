using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFlowDriver
{
    public class MFlowData : Dictionary<object, object>
    {
        public T GetVaue<T>(string key)
        {
            return ContainsKey(key) ? (T)this[key] : default(T);
        }
    }
}
