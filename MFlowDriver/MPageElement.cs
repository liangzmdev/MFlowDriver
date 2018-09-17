using System;

namespace MFlowDriver
{
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

        public static MPageElement Of(string name, Type pageType, int timeout, MFlowDict nextPages)
        {
            return new MPageElement() { Name = name, PageType = pageType, Timeout = timeout, NextPages = nextPages};
        }

        public static MPageElement Of(string name, Type pageType)
        {
            return Of(name, pageType, int.MinValue, null);
        }

        public static MPageElement Of(string name, Type pageType, int timeout)
        {
            return Of(name, pageType, timeout, null);
        }
    }

    public class MTimeout
    {
        public static int Forever { get; } = int.MinValue;
        public static int Default { get; } = 60;
    }
}