using System;
using System.Windows.Input;

namespace MFlowDriver.Mvvm
{
    /// <summary>
    /// DelegateCommand
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        readonly Action<T> execute;
        readonly Predicate<T> canExecute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">execute</param>
        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">execute</param>
        /// <param name="canExecute">canExecute</param>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        /// <summary>
        /// CanExecute
        /// </summary>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute((T)parameter);
        }

        /// <summary>
        /// CanExecuteChanged
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// RaiseCanExecuteChanged
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }

    /// <summary>
    /// DelegateCommand
    /// </summary>
    public class DelegateCommand : ICommand
    {
        readonly Action execute;
        readonly Func<bool> canExecute;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="execute">execute</param>
        public DelegateCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="execute">execute</param>
        /// <param name="canExecute">canExecute</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        /// <summary>
        /// CanExecute
        /// </summary>
        /// <param name="parameter">parameter</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        /// <summary>
        /// CanExecuteChanged
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// RaiseCanExecuteChanged
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter">parameter</param>
        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}
