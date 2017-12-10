using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EssentialMVVM
{
    public class AsyncDelegateCommand : AsyncDelegateCommandBase
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;
        private readonly Action<Exception> _onError;

        public AsyncDelegateCommand([NotNull] Func<Task> executeAsync, Func<bool> canExecute = null, Action<Exception> onError = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
            _onError = onError;
        }

        protected override void OnError(object parameter, Exception exception) => _onError(exception);

        protected override bool CanExecuteCore(object parameter) => _canExecute?.Invoke() ?? true;

        protected override Task ExecuteAsyncCore(object parameter) => _executeAsync();
    }

    public class AsyncDelegateCommand<T> : AsyncDelegateCommandBase
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T, Exception> _onError;
        private readonly Func<T, Task> _executeAsync;

        public AsyncDelegateCommand([NotNull] Func<T, Task> executeAsync, Func<T, bool> canExecute = null, Action<T, Exception> onError = null)
        {
            _canExecute = canExecute;
            _onError = onError;
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        }

        protected override void OnError(object parameter, Exception exception) => _onError((T) parameter, exception);

        protected override bool CanExecuteCore(object parameter) => _canExecute?.Invoke((T) parameter) ?? true;

        protected override Task ExecuteAsyncCore(object parameter) => _executeAsync((T) parameter);
    }
}
