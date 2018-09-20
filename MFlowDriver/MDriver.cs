using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MFlowDriver
{
    /// <summary>
    /// MDriver
    /// </summary>
    public class MDriver
    {
        private static MFlow flow = null;
        private static MPageElement currentPageEle;
        private static object messager = null;
        private static Panel container = null;
        private static string mainPageName = string.Empty;
        private static LinkedList<string> pageHistories = new LinkedList<string>();

        private static int timeCount;
        private static int TimeCount
        {
            get => timeCount;
            set
            {
                timeCount = value;
                currentPageEle?.Instance?.TimeChange(value < 0 ? 0 : value);
            }
        }

        /// <summary>
        /// 流程驱动初始化
        /// </summary>
        /// <param name="container">流程容器</param>
        /// <param name="flow">页面流程</param>
        /// <param name="mainPageName">流程主页面</param>
        public static void Init(Panel container, MFlow flow, string mainPageName)
        {
            MDriver.container = container;
            MDriver.flow = flow;
            MDriver.mainPageName = mainPageName;
            GotoPageByPageName(mainPageName);
            StartTimer();
            RegistClickEvent();
        }

        private static void RegistClickEvent()
        {
            var window = container.GetTopElement();
            if (window is UIElement element)
            {
                element.PreviewMouseDown += (sender, e) =>
                {
                    TimeCount = currentPageEle.Timeout;
                };
            }
        }

        private static void StartTimer()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (TimeCount == 0)
                    {
                        GotoPageByPageName(mainPageName);
                    }
                    else if (TimeCount > 0)
                    {
                        TimeCount -= 1;
                    }
                }
            });
        }

        private static void GotoPageByPageName(string pageName)
        {
            container.Dispatcher.Invoke(() =>
            {
                if (!flow.Has(pageName))
                {
                    throw MFlowException.Of($"未找到此页面:{pageName}");
                }
                var pageEle = flow[pageName];

                if (pageEle.Instance == null)
                {
                    var page = Activator.CreateInstance(pageEle.PageType) as MPage;
                    pageEle.Instance = page;
                }
                
                dynamic nextPage = pageEle.Instance;
                dynamic currentPage = currentPageEle?.Instance;

                nextPage.CurrentPage = pageEle;
                nextPage.Messager = MDriver.messager;

                nextPage.PartFlowData = currentPage?.PartFlowData ?? new MFlowData();
                nextPage.GlobalFlowData = currentPage?.GlobalFlowData ?? new MFlowData();

                if (pageName == mainPageName)
                {
                    nextPage.PartFlowData.Clear();
                    pageHistories.Clear();
                }

                pageHistories.AddLast(pageName);

                nextPage.GotoPreviousPage = (Action)(() =>
                {
                    if (pageHistories.Count > 1)
                    {
                        GotoPageByPageName(pageHistories.Last.Previous.Value);
                    }
                });
                nextPage.GotoNextPage = (Action<string>)(identityName => GotoPageByIdentityName(identityName, pageEle));
                nextPage.GotoSuccessPage = (Action)(() => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_SUCCESS, pageEle));
                nextPage.GotoFailurePage = (Action)(() => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_FAILURE, pageEle));
                nextPage.GotoExceptionPage = (Action)(() => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_EXCEPTION, pageEle));
                nextPage.GotoMainPage = (Action)(() => GotoPageByPageName(MDriver.mainPageName));

                currentPage?.MDispose();
                nextPage?.MInit();

                currentPageEle = pageEle;
                TimeCount = pageEle.Timeout;
                container.Children.Clear();
                container.Children.Add(pageEle.Instance);
            });
        }

        private static void GotoPageByIdentityName(string identityName, MPageElement pageEle)
        {
            if (pageEle.NextPages == null || !pageEle.NextPages.HasIdentityName(identityName))
            {
                throw MFlowException.Of($"不包含此页面标识名称:{identityName}");
            }
            var pageName = pageEle.NextPages[identityName];
            GotoPageByPageName(pageName);
        }

        /// <summary>
        /// 注册消息提供者
        /// </summary>
        /// <param name="messager">消息提供者</param>
        public static void RegistMessager(object messager) => MDriver.messager = messager;
    }
}