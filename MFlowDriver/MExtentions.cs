using System.Windows;

namespace MFlowDriver
{
    public static class MFlowExtentions
    {
        public static DependencyObject GetTopElement(this DependencyObject obj)
        {
            var p = LogicalTreeHelper.GetParent(obj);
            return p == null ? obj : GetTopElement(p);
        }
    }
}
