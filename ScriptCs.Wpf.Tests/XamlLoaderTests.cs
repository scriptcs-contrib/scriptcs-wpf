using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

using Moq;

using ScriptCs.Wpf.Interfaces;

using Should;

using Xunit;

namespace ScriptCs.Wpf.Tests
{
    public class XamlLoaderTests
    {
        public class TheLoadXamlMethod
        {
            [Fact]
            public void ShouldThrowIfFileDoesntExist()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Throws<FileNotFoundException>();

                var xamlLoader = new XamlLoader(fileSystem.Object);

                Assert.Throws<FileNotFoundException>(() => xamlLoader.LoadXaml("RandomFile.xaml"));
            }

            [Fact]
            public void ShouldThrowIfElementIsNotADependencyObject()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>()))
                    .Returns("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" />");

                var xamlLoader = new XamlLoader(fileSystem.Object);

                var exception = Assert.Throws<InvalidOperationException>(() => xamlLoader.LoadXaml("RandomFile.xaml"));

                exception.Message.ShouldContain("The element has to be a DependencyObject");
            }

            [Fact]
            public void ShouldThrowOnInvalidXaml()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns("<Style />");

                var xamlLoader = new XamlLoader(fileSystem.Object);

                var exception = Assert.Throws<XamlParseException>(() => xamlLoader.LoadXaml("RandomFile.xaml"));

                exception.Message.ShouldContain("Cannot create unknown type 'Style'");
            }

            [Fact]
            public void ShouldReturnElementOnValidXaml()
            {
                var fileSystem = new Mock<IFileSystem>();
                fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>()))
                    .Returns("<UserControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" />");

                var xamlLoader = new XamlLoader(fileSystem.Object);

                DependencyObject result = null;
                Assert.DoesNotThrow(() => result = xamlLoader.LoadXaml("RandomFile.xaml"));

                result.ShouldNotBeNull();
            }
        }
    }
}