namespace MFlowDriver
{
    /// <summary>
    /// MWidget
    /// </summary>
    public class MWidget : MNotifyUserControl
    {
        /// <summary>
        /// 容器
        /// </summary>
        public MContainer Container { get; set; }

        /// <summary>
        /// 显示Widget
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isBottom">是否位于页面底层</param>
        public void ShowWidget(object data = null, bool isBottom = false)
        {
            if (this.Parent != null)
            {
                return;
            }
            OnShowWidget(data);
            if (!isBottom)
            {
                Container.AddBeforeContainer(this);
            }
            else
            {
                Container.AddAfterContainer(this);
            }
        }

        /// <summary>
        /// 显示Widget时调用
        /// </summary>
        protected virtual void OnShowWidget(object data) { }

        /// <summary>
        /// 关闭Widget
        /// </summary>
        /// <param name="data">数据</param>
        public void CloseWidget(object data = null)
        {
            Container.RemoveTopPanel(this);
            Container.RemoveBottomPanel(this);
            OnCloseWidget(data);
        }

        /// <summary>
        /// 关闭Widget时调用
        /// </summary>
        protected virtual void OnCloseWidget(object data) { }
    }
}