using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MFlowDriver.Mvvm
{
    /// <summary>
    /// CommandHelper
    /// </summary>
    public class CommandHelper
    {
        private static readonly RoutedEventHandler routedEventHandler = new RoutedEventHandler(OnEventRaised);

        private static void OnEventRaised(object sender, RoutedEventArgs arg)
        {
            if (sender is DependencyObject dependencyObject)
            {
                ICommand command = GetBindingCommand(dependencyObject);
                if (command != null && command.CanExecute(arg))
                {
                    command?.Execute(arg);
                }
            }
        }

        /// <summary>
        /// GetEventName
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetEventName(DependencyObject obj)
        {
            return (string)obj.GetValue(EventNameProperty);
        }

        /// <summary>
        /// SetEventName
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetEventName(DependencyObject obj, string value)
        {
            obj.SetValue(EventNameProperty, value);
        }

        /// <summary>
        /// EventNameProperty
        /// </summary>
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.RegisterAttached("EventName", typeof(string), typeof(CommandHelper), new PropertyMetadata(default(string), OnEventNamePropertyChanged));

        /// <summary>
        /// GetBindingCommand
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICommand GetBindingCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(BindingCommandProperty);
        }

        /// <summary>
        /// SetBindingCommand
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetBindingCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(BindingCommandProperty, value);
        }

        /// <summary>
        /// SetBindingCommand
        /// </summary>
        public static readonly DependencyProperty BindingCommandProperty =
            DependencyProperty.RegisterAttached("BindingCommand", typeof(ICommand), typeof(CommandHelper), new PropertyMetadata(default(ICommand)));

        private static void OnEventNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement)
            {
                var eventName = e.NewValue.ToString();
                if (string.IsNullOrWhiteSpace(eventName))
                {
                    return;
                }
                var eventNameArr = eventName.ToString().Split('.');
                var events = EventManager.GetRoutedEvents();
                RoutedEvent routedEvent = null;
                if (eventNameArr.Length > 1)
                {
                    routedEvent = events.FirstOrDefault(ev => ev.Name == eventNameArr[1]);
                    if (routedEvent == null)
                    {
                        var evName = eventNameArr[0];
                        if (evName.Contains(":"))
                        {
                            evName = evName.Split(':')[1];
                        }
                        var type = evName.GetClassType();
                        if (type != null)
                        {
                            Activator.CreateInstance(type);
                            routedEvent = EventManager.GetRoutedEvents().FirstOrDefault(ev => ev.Name == eventNameArr[1]);
                        }
                    }
                }
                else
                {
                    routedEvent = events.FirstOrDefault(ev => ev.Name == eventNameArr[0]);
                }
                if (routedEvent != null)
                {
                    uiElement.AddHandler(routedEvent, routedEventHandler);
                }
            }
        }
    }
}