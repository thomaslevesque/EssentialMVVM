using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EssentialMVVM
{
    public class AsyncDelegateCommand : AsyncDelegateCommandBase
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool>? _canExecute;
        private readonly Action<Exception>? _onError;

        public AsyncDelegateCommand(Func<Task> executeAsync, Func<bool>? canExecute = null, Action<Exception>? onError = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
            _onError = onError;
        }

        protected override void OnError(Exception exception) => _onError?.Invoke(exception);

        protected override bool CanExecuteCore(object? parameter) => _canExecute?.Invoke() ?? true;

        protected override Task ExecuteAsyncCore(object? parameter) => _executeAsync();
    }

    public class AsyncDelegateCommand<T> : AsyncDelegateCommandBase
    {
        private static readonly bool IsParameterValueType = typeof(T).GetTypeInfo().IsValueType;

        private readonly Func<T, bool>? _canExecute;
        private readonly Action<Exception>? _onError;
        private readonly Func<T, Task> _executeAsync;

        public AsyncDelegateCommand(Func<T, Task> executeAsync, Func<T, bool>? canExecute = null, Action<Exception>? onError = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
            _onError = onError;
        }

        protected override void OnError(Exception exception) => _onError?.Invoke(exception);

        protected override bool CanExecuteCore(object? parameter)
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

        protected override Task ExecuteAsyncCore(object? parameter) => _executeAsync((T) parameter!);
    }
}
