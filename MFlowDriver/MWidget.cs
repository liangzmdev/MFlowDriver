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
        /// <param name="isBottom">是否位于页面底层</param>
        public void ShowWidget(bool isBottom = false)
        {
            OnShowWidget();
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
        public virtual void OnShowWidget() { }

        /// <summary>
        /// 关闭Widget
        /// </summary>
        public void CloseWidget()
        {
            Container.RemoveTopPanel(this);
            Container.RemoveBottomPanel(this);
            OnCloseWidget();
        }

        /// <summary>
        /// 关闭Widget时调用
        /// </summary>
        public virtual void OnCloseWidget() { }
    }
}