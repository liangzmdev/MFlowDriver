using System;

namespace MFlowDriver.Component
{
    internal class ComponentMetadata
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Instance { get; set; }
        public LifeTime LifeTime { get; set; }
    }

    /// <summary>
    /// 组件生命周期
    /// </summary>
    public enum LifeTime
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        Sington,

        /// <summary>
        /// 多例模式
        /// </summary>
        Prototype
    }
}