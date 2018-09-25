using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace MFlowDriver
{
    /// <summary>
    /// MWidget
    /// </summary>
    public class MWidget : UserControl, INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> fieldsDict = new Dictionary<string, object>();

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