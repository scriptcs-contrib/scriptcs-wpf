using System.Windows;

namespace ScriptCs.Wpf.Interfaces
{
    public interface IXamlLoader
    {
        DependencyObject LoadXaml(string path);
    }
}