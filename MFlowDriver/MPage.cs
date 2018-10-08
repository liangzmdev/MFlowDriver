using System;

namespace MFlowDriver
{
    /// <summary>
    /// MPage
    /// </summary>
    public class MPage : MNotifyUserControl
    {
        /// <summary>
        /// 容器
        /// </summary>
        public MContainer Container { get; set; }

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
        public T GetComponent<T>(string name = null) where T : class
        {
            return MDriver.GetComponent<T>(name);
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void PauseTimer()
        {
            MDriver.TimerEnabled = false;
        }

        /// <summary>
        /// 恢复计时器
        /// </summary>
        public void RegainTimer()
        {
            MDriver.TimerEnabled = true;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void ResetTimer()
        {
            MDriver.ResetTimer();
        }

        /// <summary>
        /// 显示指定Widget到上层
        /// </summary>
        /// <param name="isBottom">是否显示到底层</param>
        /// <returns>MWidget</returns>
        public T ShowWidget<T>(bool isBottom = false) where T : MWidget
        {
            var widget = GetComponent<T>();
            widget.ShowWidget(isBottom);
            return widget;
        }

        /// <summary>
        /// 获取Widget
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>组件实例</returns>
        public T GetWidget<T>() where T : MWidget
        {
            return GetComponent<T>();
        }

        /// <summary>
        /// 关闭Widget
        /// </summary>
        public void CloseWidget<T>() where T : MWidget
        {
            var widget = GetComponent<T>();
            widget.CloseWidget();
        }

        /// <summary>
        /// 移除所有Widget
        /// </summary>
        public void RemoveAllWidgets()
        {
            Container.RemoveAllWidgets();
        }
    }
}