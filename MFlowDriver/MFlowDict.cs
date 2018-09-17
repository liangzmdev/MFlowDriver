using System.Collections.Generic;

namespace MFlowDriver
{
    public class MFlowDict
    {
        private Dictionary<string, string> flowElmentDict = new Dictionary<string, string>();

        /// <summary>
        /// 添加跳转页面对应关系
        /// </summary>
        /// <param name="identityName">标识名称</param>
        /// <param name="pageElementName">页面元素名称</param>
        public void Add(string identityName, string pageElementName)
        {
            flowElmentDict.Add(identityName, pageElementName);
        }

        /// <summary>
        /// 添加跳转页面对应关系
        /// </summary>
        /// <param name="identityName1">标识名称1</param>
        /// <param name="pageElementName1">页面元素名称1</param>
        /// <param name="identityName2">标识名称2</param>
        /// <param name="pageElementName2">页面元素名称2</param>
        public void Add(string identityName1, string pageElementName1, string identityName2, string pageElementName2)
        {
            flowElmentDict.Add(identityName1, pageElementName1);
            flowElmentDict.Add(identityName2, pageElementName2);
        }

        /// <summary>
        /// 添加跳转页面对应关系
        /// </summary>
        /// <param name="identityName1">标识名称1</param>
        /// <param name="pageElementName1">页面元素名称1</param>
        /// <param name="identityName2">标识名称2</param>
        /// <param name="pageElementName2">页面元素名称2</param>
        /// <param name="identityName3">标识名称3</param>
        /// <param name="pageElementName3">页面元素名称3</param>
        public void Add(string identityName1, string pageElementName1, string identityName2, string pageElementName2, string identityName3, string pageElementName3)
        {
            flowElmentDict.Add(identityName1, pageElementName1);
            flowElmentDict.Add(identityName2, pageElementName2);
            flowElmentDict.Add(identityName3, pageElementName3);
        }

        public string this[string identityName] { get => flowElmentDict[identityName]; set => flowElmentDict[identityName] = value; }

        /// <summary>
        /// 是否包含指定的页面标识名称
        /// </summary>
        /// <param name="identityName">页面标识名称</param>
        /// <returns></returns>
        public bool HasIdentityName(string identityName)
        {
            return flowElmentDict.ContainsKey(identityName);
        }
    }
}