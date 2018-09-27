using System;

namespace MFlowDriver
{
    /// <summary>
    /// MPageElement
    /// </summary>
    public class MPageElement
    {
        /// <summary>
        /// 流程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 之后流程页面对应关系
        /// </summary>
        public MFlowDict NextPages { get; set; }

        /// <summary>
        /// 流程页面实例
        /// </summary>
        public MPage Instance { get; set; }

        private Type pageType;

        /// <summary>
        /// 页面类型
        /// </summary>
        public Type PageType
        {
            get => pageType;
            set
            {
                if (!typeof(MPage).IsAssignableFrom(value))
                {
                    throw MFlowException.Of("注册页面的类型必须从MPage继承");
                }
                pageType = value;
            }
        }

        /// <summary>
        /// Of
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="nextPages">子页面集合</param>
        /// <returns>MPageElement</returns>
        public static MPageElement Of(string name, Type pageType, int timeout, MFlowDict nextPages)
        {
            return new MPageElement() { Name = name, PageType = pageType, Timeout = timeout, NextPages = nextPages };
        }

        /// <summary>
        /// Of
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <returns>MPageElement</returns>
        public static MPageElement Of(string name, Type pageType)
        {
            return Of(name, pageType, int.MinValue, null);
        }

        /// <summary>
        /// Of
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <param name="pageType">页面类型</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>MPageElement</returns>
        public static MPageElement Of(string name, Type pageType, int timeout)
        {
            return Of(name, pageType, timeout, null);
        }
    }

    /// <summary>
    /// MTimeout
    /// </summary>
    public class MTimeout
    {
        /// <summary>
        /// 永久
        /// </summary>
        public static int Forever { get; } = int.MinValue;

        /// <summary>
        /// 默认 60s
        /// </summary>
        public static int Default { get; } = 60;
    }
}