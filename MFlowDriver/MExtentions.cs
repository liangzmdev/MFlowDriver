using System;
using System.Linq.Expressions;
using System.Windows;

namespace MFlowDriver
{
    public static class MFlowExtentions
    {
        /// <summary>
        /// 获取顶级元素
        /// </summary>
        /// <param name="obj">开始查找元素对象</param>
        /// <returns></returns>
        public static DependencyObject GetTopElement(this DependencyObject obj)
        {
            var p = LogicalTreeHelper.GetParent(obj);
            return p == null ? obj : GetTopElement(p);
        }

        public static string GetPropertyName<TV, TP>(this Expression<Func<TV, TP>> expression)
        {
            var propertyName = string.Empty;
            if (expression.Body is MemberExpression member)
            {
                propertyName = member.Member.Name;
            }
            return propertyName;
        }
    }
}
