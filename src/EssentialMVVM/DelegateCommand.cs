using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EssentialMVVM
{
    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public DelegateCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public override void Execute(object? parameter) => _execute();
    }

    public class DelegateCommand<T> : DelegateCommandBase
    {
        private static readonly bool IsParameterValueType = typeof(T).GetTypeInfo().IsValueType;

        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is T value)
                return _canExecute?.Invoke(value) ?? true;

            if (parameter is null)
            {
                if (IsParameterValueType)
                    return false;
                return _canExecute?.Invoke(default!) ?? true;
            }

            return false;
        }

        public override void Execute(object? parameter) => _execute((T) parameter!);
    }
}
