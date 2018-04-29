using System;
using System.Threading.Tasks;

namespace EssentialMVVM
{
    public abstract class AsyncDelegateCommandBase : DelegateCommandBase
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        public bool IsExecuting => _executionTask != null;

        private Task _executionTask;
        private Task ExecutionTask
        {
            get => _executionTask;
            set
            {
                _executionTask = value;
                RaiseCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && CanExecuteCore(parameter);
        }

        public override async void Execute(object parameter)
        {
            try
            {
                var task = ExecutionTask = ExecuteAsyncCore(parameter);
                await task;
            }
            catch (Exception ex)
            {
                OnError(parameter, ex);
            }
            finally
            {
                ExecutionTask = null;
            }
        }

        public Task WaitForCompletionAsync() => ExecutionTask ?? CompletedTask;

        protected abstract void OnError(object parameter, Exception exception);
        protected abstract bool CanExecuteCore(object parameter);
        protected abstract Task ExecuteAsyncCore(object parameter);
    }
}