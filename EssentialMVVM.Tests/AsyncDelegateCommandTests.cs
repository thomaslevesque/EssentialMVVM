using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace EssentialMVVM.Tests
{
    public class AsyncDelegateCommandTests
    {
        [Fact]
        public void CanExecute_returns_true_if_not_specified()
        {
            var command = new AsyncDelegateCommand(() => Task.CompletedTask);
            command.CanExecute(null).Should().BeTrue();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CanExecute_returns_value_from_specified_delegate(bool canExecute)
        {
            var command = new AsyncDelegateCommand(() => Task.CompletedTask, () => canExecute);
            command.CanExecute(null).Should().Be(canExecute);
        }

        [Fact]
        public void Execute_calls_specified_delegate_and_doesnt_wait_for_completion()
        {
            bool wasCalled = false;
            bool completed = false;
            var tcs = new TaskCompletionSource<bool>();
            var command = new AsyncDelegateCommand(async () =>
            {
                wasCalled = true;
                await tcs.Task;
                completed = true;
            });

            command.Execute(null);
            wasCalled.Should().BeTrue();
            completed.Should().BeFalse();

            // To prevent the test from hanging
            tcs.SetResult(true);
        }

        [Fact]
        public async Task CanExecute_returns_false_during_execution_and_true_after_completion()
        {
            var tcs = new TaskCompletionSource<bool>();
            var command = new AsyncDelegateCommand(() => tcs.Task);
            command.Execute(null);
            command.CanExecute(null).Should().BeFalse();
            tcs.SetResult(true);
            await command.WaitForCompletionAsync();
            command.CanExecute(null).Should().BeTrue();
        }
    }
}
