using FluentAssertions;
using Xunit;

namespace EssentialMVVM.Tests
{
    public class DelegateCommandTests
    {
        [Fact]
        public void CanExecute_returns_true_if_not_specified()
        {
            var command = new DelegateCommand(() => { });
            command.CanExecute(null).Should().BeTrue();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CanExecute_returns_value_from_specified_delegate(bool canExecute)
        {
            var command = new DelegateCommand(() => { }, () => canExecute);
            command.CanExecute(null).Should().Be(canExecute);
        }

        [Fact]
        public void Execute_calls_specified_delegate()
        {
            bool wasCalled = false;
            var command = new DelegateCommand(() => { wasCalled = true; });
            command.Execute(null);
            wasCalled.Should().BeTrue();
        }
    }
}
