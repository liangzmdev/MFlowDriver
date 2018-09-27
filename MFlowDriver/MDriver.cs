using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Panel container = null;
        private static string mainPageName = string.Empty;
        private static LinkedList<string> pageHistories = new LinkedList<string>();
        private static List<Tuple<string, Type, object>> components = new List<Tuple<string, Type, object>>();

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
            InitAllPage();
            GotoPageByPageName(mainPageName);
            StartTimer();
            RegistClickEvent();
        }

        private static void InitAllPage()
        {
            var partFlowData = new MFlowData();
            var globalFlowData = new MFlowData();
            flow.AllPages.ForEach(e =>
            {
                var page = Activator.CreateInstance(e.PageType) as MPage;
                page.CurrentPage = e;
                page.PartFlowData = partFlowData;
                page.GlobalFlowData = globalFlowData;

                page.GotoPreviousPage = () =>
                {
                    if (pageHistories.Count > 1)
                    {
                        GotoPageByPageName(pageHistories.Last.Previous.Value);
                    }
                };

                page.GotoNextPage = identityName => GotoPageByIdentityName(identityName, e);
                page.GotoSuccessPage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_SUCCESS, e);
                page.GotoFailurePage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_FAILURE, e);
                page.GotoExceptionPage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_EXCEPTION, e);
                page.GotoMainPage = () => GotoPageByPageName(MDriver.mainPageName);

                e.Instance = page;
            });
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

                var nextPage = pageEle.Instance;
                var currentPage = currentPageEle?.Instance;

                if (pageName == mainPageName)
                {
                    nextPage.PartFlowData.Clear();
                    pageHistories.Clear();
                }

                pageHistories.AddLast(pageName);

                currentPage?.MDispose();
                nextPage.MInit();

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
        /// 注册组件
        /// </summary>
        /// <param name="component">组件实例</param>
        /// <param name="name">组件名称</param>
        public static void RegistComponent(object component, string name = null)
        {
            if (components.Any(e => e.Item1 == name && name != null))
            {
                throw MFlowException.Of("存在同名组件");
            }

            if (name == null)
            {
                var cpn = components.Where(e => e.Item2 == component.GetType());
                if (cpn.Any())
                {
                    components.Remove(cpn.First());
                }
                components.Add(new Tuple<string, Type, object>(null, component.GetType(), component));
            }
            else
            {
                components.Add(new Tuple<string, Type, object>(name, component.GetType(), component));
            }
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="name">组件名称</param>
        /// <returns>组件实例</returns>
        public static T GetComponent<T>(string name = null)
        {
            var cpn = components.Where(e => e.Item1 == name && e.Item2 == typeof(T)).LastOrDefault();
            return cpn != null ? (T)cpn.Item3 : default(T);
        }
    }
}