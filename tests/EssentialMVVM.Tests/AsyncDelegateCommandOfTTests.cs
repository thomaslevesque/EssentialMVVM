using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace EssentialMVVM.Tests
{
    public class AsyncDelegateCommandOfTTests
    {
        [Fact]
        public void CanExecute_returns_true_if_not_specified()
        {
            var command = new AsyncDelegateCommand<int>(x => Task.CompletedTask);
            command.CanExecute(42).Should().BeTrue();
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(11, false)]
        [InlineData(42, true)]
        public void CanExecute_returns_value_from_specified_delegate(int parameter, bool canExecute)
        {
            var command = new AsyncDelegateCommand<int>(x => Task.CompletedTask, x => x % 2 == 0);
            command.CanExecute(parameter).Should().Be(canExecute);
        }

        [Fact]
        public void Execute_calls_specified_delegate_and_doesnt_wait_for_completion()
        {
            bool wasCalled = false;
            bool completed = false;
            var tcs = new TaskCompletionSource<bool>();
            var command = new AsyncDelegateCommand<int>(async x =>
            {
                wasCalled = true;
                await tcs.Task;
                completed = true;
            });

            command.Execute(42);
            wasCalled.Should().BeTrue();
            completed.Should().BeFalse();

            // To prevent the test from hanging
            tcs.SetResult(true);
        }

        [Fact]
        public async Task CanExecute_returns_false_during_execution_and_true_after_completion()
        {
            var tcs = new TaskCompletionSource<bool>();
            var command = new AsyncDelegateCommand<int>(x => tcs.Task);
            command.Execute(42);
            command.CanExecute(42).Should().BeFalse();
            tcs.SetResult(true);
            await command.WaitForCompletionAsync();
            command.CanExecute(42).Should().BeTrue();
        }
    }
}
