scriptcs-wpf
============

## About

This is a [Script Pack](https://github.com/scriptcs/scriptcs/wiki/Script-Packs) 
for [scriptcs](https://github.com/scriptcs/scriptcs) that allows you to script 
WPF applications fast and easy :smile:

Scripting a WPF application requires you to write lots of boilerplate code:
 - A WPF application requires several references to assemblies and importing of namespaces
 - The application and all XAML resources must be loaded and run on a [Single Thread Apartment](http://msdn.microsoft.com/en-us/library/windows/desktop/ms680112.aspx) thread.
 - Since XAML resources aren't compiled (no generated partial/codebehind), they have to be dynamically loaded at runtime.

This Script Pack is created to assist you with these tasks, help you get rid of boilerplate code and get on with the scripting! :sunglasses:

## Installation
To intall this Script Pack, simply navigate to your script folder and run `scriptcs -install ScriptCs.Wpf`. :metal:

## Usage
Using the pack in your scripts is as easy as :cake:! After it has been installed, it will automatically be loaded (from the bin folder).

The pack will reference the following assemblies:
 - WindowsBase
 - PresentationCore
 - PresentationFramework
 - System.Xaml
 - System.Xml

And import the following namespaces:
 - System.ComponentModel
 - System.Diagnostics
 - System.Threading
 - System.Windows
 - System.Windows.Input

To use the pack's utility methods in your scripts, use `Require<Wpf>` to get the Script Pack context.

The context has several handy utility methods:

```csharp
void RunApplication<TViewModel>()
void RunApplication<TViewModel>(string xamlFile)
void RunApplication<TViewModel>(string xamlFile, TViewModel viewModel)
void RunApplication<TViewModel>(TViewModel viewModel)
FrameworkElement LoadXaml(string xamlFile)
void RunInSTA(Action action)
```

### `LoadXaml` and `RunInSTA`

`LoadXaml` loads the given a XAML resource as a `FrameworkElement`.  
`RunInSTA` wraps and invokes the given action in an STA thread. This method will block until the application is terminated. 

The following is a more verbose script that shows the usage of the `LoadXaml` and `RunInSTA` methods:

```csharp
public class MyApplication : Application
{
    private readonly Window _mainWindow;
 
    public MyApplication(Window mainWindow)
    {
        _mainWindow = mainWindow;
    }
 
    protected override void OnStartup(StartupEventArgs e)
    {
        _mainWindow.Show();
    }
}
 
var wpf = Require<Wpf>(); // Gets the WPF Script Pack context
wpf.RunInSTA(() =>
{
  // Load the View from XAML
  var view = wpf.LoadXaml("CalculatorView.xaml");
  
  // Set the ViewModel
  view.DataContext = new CalculatorViewModel();
 
  // Create a new Window
  var mainWindow = new Window { Content = view, SizeToContent = SizeToContent.WidthAndHeight };
  
  // Create a new Application
  var application = new MyApplication(mainWindow);
  
  // Run the Application
  application.Run();
});
```

### `RunApplication<TViewModel>`

The `RunApplication<TViewModel>` methods will take care of creating an `Application` object for you.  
In addition to that, they will load the XAML View, set its `DataContext`, create a `Window` and run the application, 
all in an STA thread :sunny:

The two methods that **don't** take a `string` will look for a XAML file (View) for the given ViewModel type by convention.  
This is a [super simple convention](https://github.com/khellang/scriptcs-wpf/blob/master/ScriptCs.Wpf/Wpf.cs#L69) that will look for a XAML file in the script directory with the same name as the ViewModel, but without the 'Model' at the end.
> CalculatorViewModel -> CalculatorView.xaml

```csharp
var wpf = Require<Wpf>();
wpf.RunApplication("CalculatorView.xaml", new CalculatorViewModel());

wpf.RunApplication<CalculatorViewModel>("CalculatorView.xaml");

wpf.RunApplication(new CalculatorViewModel()); // Uses convention to find View for ViewModel
 
wpf.RunApplication<CalculatorViewModel>();  // Uses convention to find View for ViewModel
```

## Sample

For a working sample application, please check out the [sample folder](https://github.com/khellang/scriptcs-wpf/tree/master/sample)...

## Bugs and Feature Requests
If you find any bugs or would like to see new features/changes in this Script Pack, 
please let me know by [filing an issue](https://github.com/khellang/scriptcs-wpf/issues/new) or sending a pull request :grin:  


Did you count how many times you read the words "script" and "pack" in this readme? :stuck_out_tongue_closed_eyes:
