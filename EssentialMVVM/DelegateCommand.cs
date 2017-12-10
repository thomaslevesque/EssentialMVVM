using System;
using JetBrains.Annotations;

namespace EssentialMVVM
{
    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand([NotNull] Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public override void Execute(object parameter) => _execute();
    }

    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public DelegateCommand([NotNull] Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke((T) parameter) ?? true;

        public override void Execute(object parameter) => _execute((T) parameter);
    }
}
