using System;
using System.Collections.Generic;
using System.Linq;

namespace MFlowDriver
{
    public class MFlow
    {
        public const string IDNENTITY_NAME_SUCCESS = "SUCCESS";
        public const string IDNENTITY_NAME_FAILURE = "FAILURE";
        public const string IDNENTITY_NAME_EXCEPTION = "EXCEPTION";

        private List<MPageElement> mPages = new List<MPageElement>();

        public MFlow()
        {
        }

        public static MFlow Of()
        {
            return new MFlow();
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="element">页面元素</param>
        public void Add(MPageElement element)
        {
            if (Has(element.Name))
            {
                throw MFlowException.Of("已存在此页面");
            }
            mPages.Add(element);
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="nextPages">后面页面对应关系</param>
        public void Add(string name, Type pageType, int timeout, MFlowDict nextPages)
        {
            Add(MPageElement.Of(name, pageType, timeout, nextPages)); 
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        public void Add(string name, Type pageType)
        {
            Add(MPageElement.Of(name, pageType, MTimeout.Default));
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="timeout">超时时间</param>
        public void Add(string name, Type pageType, int timeout)
        {
            Add(MPageElement.Of(name, pageType, timeout));
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="successPage">默认成功页面</param>
        public void Add(string name, Type pageType, int timeout, string successPage)
        {
            var nextPages = new MFlowDict();
            nextPages.Add(IDNENTITY_NAME_SUCCESS, successPage);
            Add(MPageElement.Of(name, pageType, timeout, nextPages));
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="successPage">默认成功页面</param>
        public void Add(string name, Type pageType, string successPage)
        {
            var nextPages = new MFlowDict();
            nextPages.Add(IDNENTITY_NAME_SUCCESS, successPage);
            Add(MPageElement.Of(name, pageType, MTimeout.Default, nextPages));
        }

        /// <summary>
        /// 获取指定页面元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MPageElement GetPage(string name)
        {
            return mPages.Where(page => page.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 清除所有流程页面
        /// </summary>
        public void Clear() => mPages.Clear();

        /// <summary>
        /// 是否包含该页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <returns></returns>
        public bool Has(string name)
        {
            return mPages.Any(page => page.Name == name);
        }

        public MPageElement this[string name] { get => GetPage(name); }
    }
}