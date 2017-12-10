using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EssentialMVVM
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        private static readonly SetPropertyResult _notChangedResult = new NotChangedSetPropertyResult();
        private readonly SetPropertyResult _changedResult;

        public BindableBase()
        {
            _changedResult = new ChangedSetPropertyResult(OnPropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected SetPropertyResult Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (AreEqual(field, newValue, propertyName))
            {
                return _notChangedResult;
            }

            field = newValue;
            OnPropertyChanged(propertyName);
            return _changedResult;
        }

        protected virtual bool AreEqual<T>(T currentValue, T newValue, string propertyName)
        {
            return EqualityComparer<T>.Default.Equals(currentValue, newValue);
        }
    }
}
