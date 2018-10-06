using System;
using System.Linq.Expressions;
using System.Windows;

namespace MFlowDriver
{
    /// <summary>
    /// MFlowExtentions
    /// </summary>
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

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="TV">TV</typeparam>
        /// <typeparam name="TP">TP</typeparam>
        /// <param name="expression">expression</param>
        /// <returns></returns>
        public static string GetPropertyName<TV, TP>(this Expression<Func<TV, TP>> expression)
        {
            var propertyName = string.Empty;
            if (expression.Body is MemberExpression member)
            {
                propertyName = member.Member.Name;
            }
            return propertyName;
        }

        /// <summary>
        /// 返回当前程序域中指定名称的找到的第一个类型
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Type GetClassType(this string className)
        {
            var asmArr = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asmArr)
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.Name == className)
                    {
                        return type;
                    }
                }
            }
            return null;
        }
    }
}