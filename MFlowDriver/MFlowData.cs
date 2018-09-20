using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFlowDriver
{
    /// <summary>
    /// MFlowData
    /// </summary>
    public class MFlowData : Dictionary<object, object>
    {
        /// <summary>
        /// 获取指定泛型类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetVaue<T>(string key)
        {
            return ContainsKey(key) ? (T)this[key] : default(T);
        }
    }
}
