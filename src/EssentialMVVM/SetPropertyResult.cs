using System;

namespace EssentialMVVM
{
    public abstract class SetPropertyResult
    {
        public abstract bool HasChanged { get; }
        public abstract SetPropertyResult AndNotifyPropertyChanged(string propertyName);
        public abstract SetPropertyResult AndExecute(Action action);

        public static implicit operator bool(SetPropertyResult result) => result.HasChanged;
        public static bool operator true(SetPropertyResult result) => result.HasChanged;
        public static bool operator false(SetPropertyResult result) => !result.HasChanged;
    }

    internal class ChangedSetPropertyResult : SetPropertyResult
    {
        private readonly Action<string> _notifyAction;

        public ChangedSetPropertyResult(Action<string> notifyAction)
        {
            _notifyAction = notifyAction;
        }

        public override bool HasChanged => true;

        public override SetPropertyResult AndNotifyPropertyChanged(string propertyName)
        {
            _notifyAction(propertyName);
            return this;
        }

        public override SetPropertyResult AndExecute(Action action)
        {
            action();
            return this;
        }
    }

    internal class NotChangedSetPropertyResult : SetPropertyResult
    {
        public override bool HasChanged => false;
        public override SetPropertyResult AndNotifyPropertyChanged(string propertyName) => this;
        public override SetPropertyResult AndExecute(Action action) => this;
    }
}