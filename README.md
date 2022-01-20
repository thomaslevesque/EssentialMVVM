# EssentialMVVM

[![NuGet version](https://img.shields.io/nuget/v/EssentialMVVM.svg?logo=nuget)](https://www.nuget.org/packages/EssentialMVVM)
[![AppVeyor build](https://img.shields.io/appveyor/ci/thomaslevesque/essentialmvvm.svg?logo=appveyor)](https://ci.appveyor.com/project/thomaslevesque/essentialmvvm)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/thomaslevesque/essentialmvvm.svg?logo=appveyor)](https://ci.appveyor.com/project/thomaslevesque/essentialmvvm/build/tests)

A minimalist MVVM framework, which contains the basic building blocks I was tired of
rewriting in each of my projects. It doesn't do much, but strives to do it well.

It targets .NET Framework 4.5, .NET Standard 1.3 and .NET Standard 2.0, so it should be
usable on most XAML platforms.

## Features

### `BindableBase`

A class that implements `INotifyPropertyChanged` and can be used as a  base class for
ViewModels. It exposes two methods:

- `OnPropertyChanged`: raises the `PropertyChanged` event for the specified property
  name (if the name is not specified, it's inferred for the calling member).
- `Set`: assigns the specified value to the backing field of a property, if different
  from the current value, and raises `PropertyChanged` for that property. Can be
  chained to perform other actions if the value has changed. For instance:

    ```csharp
    private string _foo;
    public string Foo
    {
        get => _foo;
        set => Set(ref _foo, value)
            .AndNotifyPropertyChanged(nameof(PropertyThatDependOnFoo))
            .AndRaiseCanExecuteChanged(_barCommand)
            .AndExecute(() => DoSomething())
    }

    public string PropertyThatDependsOnFoo => Foo?.ToUpper();

    private DelegateCommand _barCommand;
    public ICommand BarCommand => _barCommand ??= new DelegateCommand(Bar);

    private void Bar() { }
    private void DoSomething() { }
    ```

    It can also be used in a condition, like this:

    ```csharp
    private string _foo;
    public string Foo
    {
        get => _foo;
        set
        {
            if (Set(ref _foo, value))
            {
                // Do something if the value changed
            }
        }
    }
    ```

### `DelegateCommand` and related classes

An implementation of `ICommand` that accepts a delegate to specify what the command does.
Exists in multiple flavors:

- `DelegateCommand`: a synchronous command that takes no parameter.
- `DelegateCommand<T>`: a synchronous command that takes a parameter of type `T`.
- `AsyncDelegateCommand`: an asynchronous command that takes no parameter.
- `AsyncDelegateCommand<T>`: an asynchronous command that takes a parameter of type `T`.

An asynchronous command will not allow execution (i.e. `CanExecute` will return false) if
the previous execution is incomplete.
