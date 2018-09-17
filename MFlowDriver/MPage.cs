using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace MFlowDriver
{
    public class MPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MPageElement CurrentPage { get; set; }

        public MFlowData PartFlowData { get; set; }
        public MFlowData GlobalFlowData { get; set; }

        public Action<string> GotoNextPage { get; set; }
        public Action GotoSuccessPage { get; set; }
        public Action GotoFailurePage { get; set; }
        public Action GotoExceptionPage { get; set; }
        public Action GotoMainPage { get; set; }

        public object Messager { get; set; }

        public virtual void MInit() { }
        public virtual void MDispose() { }
        public virtual void TimeChange(int timeCount) { }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}