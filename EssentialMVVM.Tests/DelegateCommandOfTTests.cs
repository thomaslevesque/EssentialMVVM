using FluentAssertions;
using Xunit;

namespace EssentialMVVM.Tests
{
    public class DelegateCommandOfTTests
    {
        [Fact]
        public void CanExecute_returns_true_if_not_specified()
        {
            var command = new DelegateCommand<int>(x => { });
            command.CanExecute(0).Should().BeTrue();
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(11, false)]
        [InlineData(42, true)]
        public void CanExecute_returns_value_from_specified_delegate(int parameter, bool canExecute)
        {
            var command = new DelegateCommand<int>(x => { }, x => x % 2 == 0);
            command.CanExecute(parameter).Should().Be(canExecute);
        }

        [Fact]
        public void Execute_calls_specified_delegate()
        {
            int receivedParameter = 0;
            var command = new DelegateCommand<int>(x => { receivedParameter = x; });
            command.Execute(42);
            receivedParameter.Should().Be(42);
        }
    }
}