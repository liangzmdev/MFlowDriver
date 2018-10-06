using System;

namespace MFlowDriver
{
    /// <summary>
    /// MPage
    /// </summary>
    public class MPage : MWidget
    {
        /// <summary>
        /// 当前页面元素
        /// </summary>
        public MPageElement CurrentPage { get; set; }

        /// <summary>
        /// 局部流程数据
        /// </summary>
        public MFlowData PartFlowData { get; set; }

        /// <summary>
        /// 全局流程数据
        /// </summary>
        public MFlowData GlobalFlowData { get; set; }

        /// <summary>
        /// 定位上一个页面
        /// </summary>
        public Action GotoPreviousPage { get; set; }

        /// <summary>
        /// 定位下一个页面
        /// </summary>
        public Action<string> GotoNextPage { get; set; }

        /// <summary>
        /// 地位SUCCESS默认页面
        /// </summary>
        public Action GotoSuccessPage { get; set; }

        /// <summary>
        /// 定位FAILURE默认页面
        /// </summary>
        public Action GotoFailurePage { get; set; }

        /// <summary>
        /// 定位EXCEPTION默认页面
        /// </summary>
        public Action GotoExceptionPage { get; set; }

        /// <summary>
        /// 定位主页面
        /// </summary>
        public Action GotoMainPage { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void MInit() { }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void MDispose() { }

        /// <summary>
        /// 超时时间变化调用
        /// </summary>
        /// <param name="timeCount"></param>
        public virtual void TimeChange(int timeCount) { }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="name">组件名称</param>
        /// <returns>组件实例</returns>
        public T GetComponent<T>(string name = null)
        {
            return MDriver.GetComponent<T>(name);
        }
    }
}