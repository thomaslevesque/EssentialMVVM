using FluentAssertions;
using Xunit;

namespace EssentialMVVM.Tests
{
    public class BindableBaseTests
    {
        [Fact]
        public void PropertyChanged_is_raised_for_modified_property()
        {
            var vm = new MyViewModel();
            vm.MonitorEvents();
            vm.X = 42;
            vm.ShouldRaisePropertyChangeFor(_ => _.X);
        }

        [Fact]
        public void PropertyChanged_is_not_raised_if_property_is_set_to_same_value()
        {
            var vm = new MyViewModel();
            vm.MonitorEvents();
            vm.X = 0;
            vm.ShouldNotRaisePropertyChangeFor(_ => _.X);
        }

        [Fact]
        public void PropertyChanged_is_raised_for_dependent_property()
        {
            var vm = new MyViewModel();
            vm.MonitorEvents();
            vm.X = 42;
            vm.ShouldRaisePropertyChangeFor(_ => _.Y);
        }

        [Fact]
        public void PropertyChanged_is_not_raised_for_dependent_property_if_property_is_set_to_same_value()
        {
            var vm = new MyViewModel();
            vm.MonitorEvents();
            vm.X = 0;
            vm.ShouldNotRaisePropertyChangeFor(_ => _.X);
        }

        [Fact]
        public void Callback_is_executed_when_property_is_modified()
        {
            var vm = new MyViewModel();
            vm.X = 42;
            vm.CallbackCount.Should().Be(1);
        }

        [Fact]
        public void Callback_is_not_executed_when_property_is_set_to_same_value()
        {
            var vm = new MyViewModel();
            vm.X = 0;
            vm.CallbackCount.Should().Be(0);
        }

        class MyViewModel : BindableBase
        {
            private int _x;
            public int X
            {
                get => _x;
                set =>
                    Set(ref _x, value)
                    .AndNotifyPropertyChanged(nameof(Y))
                    .AndExecute(() => CallbackCount++);
            }

            public int Y => X + 1;

            public int CallbackCount { get; set; }
        }
    }
}
