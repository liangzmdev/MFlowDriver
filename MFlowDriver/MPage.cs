using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls;

namespace MFlowDriver
{
    /// <summary>
    /// MPage
    /// </summary>
    public class MPage : UserControl, INotifyPropertyChanged
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
        /// 消息发送器
        /// </summary>
        public object Messager { get; set; }

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

        readonly Dictionary<string, object> fieldsDict = new Dictionary<string, object>();

        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">propertyName</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        protected T GetProperty<T>(string propertyName)
        {
            T value = default(T);
            if (propertyName != null && fieldsDict.ContainsKey(propertyName))
            {
                value = (T)fieldsDict[propertyName];
            }
            return value;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        protected void SetProperty<T>(string propertyName, T value)
        {
            fieldsDict[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
    }
}