using System;

using Moq;

using ScriptCs.Wpf.Interfaces;

using Should;

using Xunit;
using Xunit.Extensions;

namespace ScriptCs.Wpf.Tests
{
    public class ViewLocatorTests
    {
        public class TheLocateViewForMethod
        {
            [Fact]
            public void ShouldThrowIfTypeNameDoesntEndWithViewModel()
            {
                var fileSystem = new Mock<IFileSystem>();
                var viewLocator = new ViewLocator(fileSystem.Object);

                var exception = Assert.Throws<InvalidOperationException>(() => viewLocator.LocateViewFor("SomeClass"));

                exception.Message.ShouldContain("The ViewModel type name has to end with 'ViewModel'");
            }

            [Fact]
            public void ShouldThrowIfFileDoesntExist()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.SetupGet(x => x.CurrentDirectory).Returns("C:\\");
                fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

                var viewLocator = new ViewLocator(fileSystem.Object);

                var exception = Assert.Throws<InvalidOperationException>(() => viewLocator.LocateViewFor("TestViewModel"));

                exception.Message.ShouldContain("Tried: C:\\TestView.xaml");
            }

            [Fact]
            public void ShouldThrowOnEmptyFileName()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.SetupGet(x => x.CurrentDirectory).Returns("C:\\");
                fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

                var viewLocator = new ViewLocator(fileSystem.Object);

                var exception = Assert.Throws<ArgumentNullException>(() => viewLocator.LocateViewFor(string.Empty));

                exception.Message.ShouldContain("Parameter name: viewModelName");
            }

            [Theory]
            [InlineData("ViewModel", "View.xaml")]
            [InlineData("TestViewModel", "TestView.xaml")]
            public void ShouldReturnCorrectFileName(string viewModel, string fileName)
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.SetupGet(x => x.CurrentDirectory).Returns(string.Empty);
                fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

                var viewLocator = new ViewLocator(fileSystem.Object);

                var result = viewLocator.LocateViewFor(viewModel);

                result.ShouldEqual(fileName);
            }

            [Fact]
            public void ShouldReturnViewNameIfFileExists()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.SetupGet(x => x.CurrentDirectory).Returns("C:\\");
                fileSystem.Setup(x => x.FileExists("C:\\TestView.xaml")).Returns(true);

                var viewLocator = new ViewLocator(fileSystem.Object);

                var result = viewLocator.LocateViewFor("TestViewModel");

                result.ShouldEqual("C:\\TestView.xaml");
            }
        }

        public class AllMethods
        {
            [Fact]
            public void ShouldYieldTheSameResult()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.SetupGet(x => x.CurrentDirectory).Returns("C:\\");
                fileSystem.Setup(x => x.FileExists("C:\\TestView.xaml")).Returns(true);

                var viewLocator = new ViewLocator(fileSystem.Object);

                var result = viewLocator.LocateViewFor("TestViewModel");
                var secondResult = viewLocator.LocateViewFor<TestViewModel>();

                result.ShouldEqual(secondResult);
            }

            private abstract class TestViewModel { }
        }
    }
}