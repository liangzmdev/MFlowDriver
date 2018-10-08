using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MFlowDriver
{
    /// <summary>
    /// MContainer
    /// </summary>
    public class MContainer
    {
        private Panel pagePanel;
        private Panel topPanel;
        private Panel bottomPanel;

        /// <summary>
        /// MContainer
        /// </summary>
        /// <param name="window">窗口</param>
        public MContainer(Window window)
        {
            var container = new Grid();
            pagePanel = new Grid();
            topPanel = new Grid();
            bottomPanel = new Grid();
            container.Children.Add(bottomPanel);
            container.Children.Add(pagePanel);
            container.Children.Add(topPanel);

            window.Content = container;
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="mPage">页面</param>
        internal void AddPage(MPage mPage)
        {
            pagePanel.Children.Clear();
            pagePanel.Children.Add(mPage);
        }

        /// <summary>
        /// 添加上层页面
        /// </summary>
        /// <param name="widget">widget</param>
        public void AddBeforeContainer(MWidget widget)
        {
            topPanel.Children.Add(widget);
        }

        /// <summary>
        /// 添加底层页面
        /// </summary>
        /// <param name="widget">widget</param>
        public void AddAfterContainer(MWidget widget)
        {
            bottomPanel.Children.Add(widget);
        }

        /// <summary>
        /// 清除上层页面
        /// </summary>
        /// <param name="widget">指定Widget</param>
        public void RemoveTopPanel(MWidget widget = null)
        {
            if (widget != null)
            {
                var exist = topPanel.Children.Contains(widget);
                if (exist)
                {
                    topPanel.Children.Remove(widget);
                }
            }
            else
            {
                topPanel.Children.Clear();
            }
        }

        /// <summary>
        /// 清除底层页面
        /// </summary>
        /// <param name="widget">指定Widget</param>
        public void RemoveBottomPanel(MWidget widget)
        {
            if (widget != null)
            {
                var exist = bottomPanel.Children.Contains(widget);
                if (exist)
                {
                    bottomPanel.Children.Remove(widget);
                }
            }
            else
            {
                bottomPanel.Children.Clear();
            }
        }

        /// <summary>
        /// 清除所有Widget
        /// </summary>
        public void RemoveAllWidget()
        {
            var allWidgets = new List<MWidget>();
            foreach (MWidget widget in bottomPanel.Children)
            {
                allWidgets.Add(widget);
            }
            foreach (MWidget widget in topPanel.Children)
            {
                allWidgets.Add(widget);
            }
            allWidgets.ForEach(w => w.CloseWidget());
        }
    }
}