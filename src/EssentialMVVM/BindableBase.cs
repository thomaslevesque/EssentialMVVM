using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected SetPropertyResult Set<T>(
            ref T field,
            T newValue,
            IEqualityComparer<T>? comparer = null,
            [CallerMemberName] string propertyName = "")
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
