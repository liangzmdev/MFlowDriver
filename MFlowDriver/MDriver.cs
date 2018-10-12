using MFlowDriver.Component;
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
        private static Panel rootPanel = null;
        private static MContainer container = null;
        private static string mainPageName = string.Empty;
        private static LinkedList<string> pageHistories = new LinkedList<string>();
        private static List<ComponentMetadata> components = new List<ComponentMetadata>();

        /// <summary>
        /// 倒计时事件
        /// </summary>
        public static event Action<int> TimeCountDownHandler;

        private static int timeCount;

        private static int TimeCount
        {
            get => timeCount;
            set
            {
                timeCount = value;
                currentPageEle?.Instance?.TimeChange(value < 0 ? 0 : value);
                TimeCountDownHandler?.Invoke(value);
            }
        }

        /// <summary>
        /// 计时器开关
        /// </summary>
        public static bool TimerEnabled { get; set; } = true;

        /// <summary>
        /// 流程驱动初始化
        /// </summary>
        /// <param name="rootPanel">容器</param>
        /// <param name="flow">页面流程</param>
        /// <param name="mainPageName">流程主页面</param>
        public static void Init(Panel rootPanel, MFlow flow, string mainPageName)
        {
            MDriver.rootPanel = rootPanel;
            MDriver.container = new MContainer(rootPanel);
            MDriver.flow = flow;
            MDriver.mainPageName = mainPageName;
            InitAllPage();
            GotoPageByPageName(mainPageName, false);
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
                page.Container = container;
                page.CurrentPage = e;
                page.PartFlowData = partFlowData;
                page.GlobalFlowData = globalFlowData;

                page.GotoPreviousPage = () =>
                {
                    if (pageHistories.Count > 1)
                    {
                        GotoPageByPageName(pageHistories.Last.Previous.Value, true);
                    }
                };

                page.GotoNextPage = identityName => GotoPageByIdentityName(identityName, e);
                page.GotoSuccessPage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_SUCCESS, e);
                page.GotoFailurePage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_FAILURE, e);
                page.GotoExceptionPage = () => GotoPageByIdentityName(MFlow.IDNENTITY_NAME_EXCEPTION, e);
                page.GotoMainPage = GotoMainPage;

                e.Instance = page;
            });
        }

        private static void RegistClickEvent()
        {
            var window = rootPanel.GetTopElement();
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
                    if (TimerEnabled)
                    {
                        Thread.Sleep(1000);
                        if (TimeCount == 0)
                        {
                            GotoPageByPageName(mainPageName, false);
                        }
                        else if (TimeCount > 0)
                        {
                            TimeCount -= 1;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public static void ResetTimer()
        {
            TimeCount = currentPageEle.Timeout;
        }

        private static void GotoPageByPageName(string pageName, bool isPreviousPage)
        {
            rootPanel.Dispatcher.Invoke(() =>
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
                    pageHistories.AddLast(pageName);
                }
                else
                {
                    if (!isPreviousPage)
                    {
                        pageHistories.AddLast(pageName);
                    }
                    else
                    {
                        pageHistories.RemoveLast();
                    }
                }

                currentPage?.MDispose();
                nextPage.MInit();

                currentPageEle = pageEle;
                TimeCount = pageEle.Timeout;

                container.AddPage(pageEle.Instance);
            });
        }

        private static void GotoPageByIdentityName(string identityName, MPageElement pageEle)
        {
            if (pageEle.NextPages == null || !pageEle.NextPages.HasIdentityName(identityName))
            {
                throw MFlowException.Of($"不包含此页面标识名称:{identityName}");
            }
            var pageName = pageEle.NextPages[identityName];
            GotoPageByPageName(pageName, false);
        }

        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="pageName">页面名称</param>
        public static void GotoPageByPageName(string pageName)
        {
            GotoPageByPageName(pageName, false);
        }

        /// <summary>
        /// 跳转到主页面
        /// </summary>
        public static void GotoMainPage()
        {
            GotoPageByPageName(mainPageName);
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="component">组件实例</param>
        /// <param name="name">组件名称</param>
        public static void RegistComponent<T>(T component, string name = null) where T : class
        {
            if (components.Any(e => e.Name == name && name != null))
            {
                throw MFlowException.Of("存在同名组件");
            }

            if (name == null)
            {
                var cpn = components.Where(e => e.Type == component.GetType());
                if (cpn.Any())
                {
                    components.Remove(cpn.First());
                }
                components.Add(new ComponentMetadata() { Instance = component, Type = component.GetType(), LifeTime = LifeTime.Sington });
            }
            else
            {
                components.Add(new ComponentMetadata() { Name = name, Instance = component, Type = component.GetType(), LifeTime = LifeTime.Sington });
            }
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="lifeTime">生命周期</param>
        public static void RegistComponent<T>(LifeTime lifeTime = LifeTime.Sington) where T : class
        {
            var componentType = typeof(T);
            var cpn = components.Where(e => e.Type == componentType);
            if (cpn.Any())
            {
                components.Remove(cpn.First());
            }
            components.Add(new ComponentMetadata() { Type = componentType, LifeTime = lifeTime, Instance = Activator.CreateInstance(componentType) });
        }

        /// <summary>
        /// 注册Widget
        /// </summary>
        /// <typeparam name="T">Widget类型</typeparam>
        /// <param name="lifeTime">生命周期</param>
        public static void RegistWidget<T>(LifeTime lifeTime = LifeTime.Sington) where T : MWidget
        {
            RegistComponent<T>(lifeTime);
        }

        /// <summary>
        /// 获取Widget
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetWidget<T>() where T : class
        {
            return GetComponent<T>();
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="name">组件名称</param>
        /// <returns>组件实例</returns>
        public static T GetComponent<T>(string name = null) where T : class
        {
            var cpn = components.Where(e => e.Name == name && e.Type == typeof(T)).LastOrDefault();
            if (cpn != null)
            {
                T instance = null;
                if (cpn.LifeTime == LifeTime.Sington)
                {
                    instance = cpn.Instance == null ? (T)Activator.CreateInstance(cpn.Type) : (T)cpn.Instance;
                    cpn.Instance = instance;
                }
                else if (cpn.LifeTime == LifeTime.Prototype)
                {
                    instance = (T)Activator.CreateInstance(cpn.Type);
                }

                if (typeof(MWidget).IsAssignableFrom(cpn.Type))
                {
                    var widget = instance as MWidget;
                    widget.Container = container;
                }

                return instance;
            }
            else
            {
                return default(T);
            }
        }
    }
}