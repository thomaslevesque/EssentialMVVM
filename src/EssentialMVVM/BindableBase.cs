using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EssentialMVVM
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        private static readonly SetPropertyResult NotChangedResult = new NotChangedSetPropertyResult();
        private readonly SetPropertyResult _changedResult;

        protected BindableBase()
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
        protected SetPropertyResult Set<T>(
            ref T field,
            T newValue,
            IEqualityComparer<T> comparer = null,
            [CallerMemberName] string propertyName = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            if (comparer.Equals(field, newValue))
            {
                return NotChangedResult;
            }

            field = newValue;
            OnPropertyChanged(propertyName);
            return _changedResult;
        }
    }
}
