using System;
using System.Windows;

using Moq;

using ScriptCs.Wpf.Interfaces;

using Xunit;

namespace ScriptCs.Wpf.Tests
{
    public class WpfTests
    {
        public class TheRunApplicationMethod
        {
            private readonly Mock<IApplicationLauncher> _applicationLauncher;

            private readonly Mock<IViewLocator> _viewLocator;

            private readonly Mock<IXamlLoader> _xamlLoader;

            private readonly Mock<IThreadManager> _threadManager;

            private readonly Wpf _wpf;

            public TheRunApplicationMethod()
            {
                _applicationLauncher = new Mock<IApplicationLauncher>();
                _viewLocator = new Mock<IViewLocator>();
                _xamlLoader = new Mock<IXamlLoader>();
                _threadManager = new Mock<IThreadManager>();

                _wpf = new Wpf(
                    _applicationLauncher.Object,
                    _viewLocator.Object, 
                    _xamlLoader.Object, 
                    _threadManager.Object);
            }

            [Fact]
            public void ShouldLocateViewByConvention()
            {
                _viewLocator.Setup(x => x.LocateViewFor<TestViewModel>()).Returns("TestView.xaml");

                _wpf.RunApplication<TestViewModel>();

                _viewLocator.Verify(x => x.LocateViewFor<TestViewModel>(), Times.Once());
            }

            [Fact]
            public void ShouldLoadXaml()
            {
                _threadManager.Setup(x => x.RunInSTA(It.IsAny<Action>())).Callback<Action>(action => action());
                _viewLocator.Setup(x => x.LocateViewFor<TestViewModel>()).Returns("TestView.xaml");

                _wpf.RunApplication<TestViewModel>();

                _xamlLoader.Verify(x => x.LoadXaml("TestView.xaml"));
            }

            [Fact]
            public void ShouldLaunchApplication()
            {
                _threadManager.Setup(x => x.RunInSTA(It.IsAny<Action>())).Callback<Action>(action => action());

                _wpf.RunApplication<TestViewModel>("TestView.xaml");

                _applicationLauncher.Verify(x => x.CreateAndRunApplication(It.IsAny<DependencyObject>(), It.IsAny<object>()), Times.Once());
            }

            [Fact]
            public void ShouldLaunchOnStaThread()
            {
                _wpf.RunApplication("TestView.xaml");

                _threadManager.Verify(x => x.RunInSTA(It.IsAny<Action>()), Times.Once());
            }

            [Fact]
            public void ShouldThrowOnEmptyXamlFile()
            {
                Assert.Throws<ArgumentNullException>(() => _wpf.RunApplication(string.Empty));
            }

            private class TestViewModel { }
        }

        public class TheLoadXamlMethod
        {
            private readonly Mock<IApplicationLauncher> _applicationLauncher;

            private readonly Mock<IViewLocator> _viewLocator;

            private readonly Mock<IXamlLoader> _xamlLoader;

            private readonly Mock<IThreadManager> _threadManager;

            private readonly Wpf _wpf;

            public TheLoadXamlMethod()
            {
                _applicationLauncher = new Mock<IApplicationLauncher>();
                _viewLocator = new Mock<IViewLocator>();
                _xamlLoader = new Mock<IXamlLoader>();
                _threadManager = new Mock<IThreadManager>();

                _wpf = new Wpf(
                    _applicationLauncher.Object,
                    _viewLocator.Object, 
                    _xamlLoader.Object, 
                    _threadManager.Object);
            }

            [Fact]
            public void ShouldLoadXaml()
            {
                _wpf.LoadXaml("XamlFile.xaml");

                _xamlLoader.Verify(x => x.LoadXaml("XamlFile.xaml"), Times.Once());
            }
        }

        public class TheRunInSTAMethod
        {
            private readonly Mock<IApplicationLauncher> _applicationLauncher;

            private readonly Mock<IViewLocator> _viewLocator;

            private readonly Mock<IXamlLoader> _xamlLoader;

            private readonly Mock<IThreadManager> _threadManager;

            private readonly Wpf _wpf;

            public TheRunInSTAMethod()
            {
                _applicationLauncher = new Mock<IApplicationLauncher>();
                _viewLocator = new Mock<IViewLocator>();
                _xamlLoader = new Mock<IXamlLoader>();
                _threadManager = new Mock<IThreadManager>();

                _wpf = new Wpf(
                    _applicationLauncher.Object,
                    _viewLocator.Object,
                    _xamlLoader.Object,
                    _threadManager.Object);
            }

            [Fact]
            public void ShouldRunTheGivenActionOnStaThread()
            {
                _wpf.RunInSTA(() => Console.WriteLine("Testing..."));

                _threadManager.Verify(x => x.RunInSTA(It.IsAny<Action>()), Times.Once());
            }
        }
    }
}