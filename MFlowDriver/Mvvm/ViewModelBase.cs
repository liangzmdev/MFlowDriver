using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MFlowDriver.Mvvm
{
    /// <summary>
    /// ViewModelBase
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
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
        public T GetProperty<T>(string propertyName)
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
        public void SetProperty<T>(string propertyName, T value)
        {
            fieldsDict[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
    }

    /// <summary>
    /// ViewModelBaseExtentions
    /// </summary>
    public static class ViewModelBaseExtentions
    {
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="TV">TV</typeparam>
        /// <typeparam name="TP">TP</typeparam>
        /// <param name="vm">ViewModelBase</param>
        /// <param name="expression">expression</param>
        /// <returns></returns>
        public static TP Get<TV, TP>(this TV vm, Expression<Func<TV, TP>> expression) where TV : ViewModelBase
        {
            var propertyName = expression.GetPropertyName();
            return vm.GetProperty<TP>(propertyName);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="TV">TV</typeparam>
        /// <typeparam name="TP">TP</typeparam>
        /// <param name="vm">vm</param>
        /// <param name="expression">expression</param>
        /// <param name="value">属性值</param>
        public static void Set<TV, TP>(this TV vm, Expression<Func<TV, TP>> expression, TP value) where TV : ViewModelBase
        {
            var propertyName = expression.GetPropertyName();
            vm.SetProperty(propertyName, value);
        }
    }
}
