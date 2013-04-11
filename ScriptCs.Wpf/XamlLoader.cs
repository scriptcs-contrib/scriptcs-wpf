using System;
using System.Windows;
using System.Windows.Markup;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class XamlLoader : IXamlLoader 
    {
        private readonly IFileSystem _fileSystem;

        public XamlLoader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public DependencyObject LoadXaml(string xamlFile)
        {
            var xamlLines = _fileSystem.ReadAllText(xamlFile);

            var resource = XamlReader.Parse(xamlLines) as DependencyObject;
            if (resource != null) return resource;

            // This should never happen with valid XAML
            throw new InvalidOperationException(
                string.Format("Could not load XAML resource '{0}': The element has to be a DependencyObject", xamlFile));
        }
    }
}