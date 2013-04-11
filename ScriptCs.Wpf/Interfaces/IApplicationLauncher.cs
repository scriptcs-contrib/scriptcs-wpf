using System.Windows;

namespace ScriptCs.Wpf.Interfaces
{
    public interface IApplicationLauncher
    {
        void CreateAndRunApplication(DependencyObject xamlFile, object viewModel);
    }
}