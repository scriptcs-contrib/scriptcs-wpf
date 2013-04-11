using System;
using System.Windows;

using ScriptCs.Contracts;

namespace ScriptCs.Wpf.Interfaces
{
    public interface IWpfContext : IScriptPackContext
    {
        void RunApplication<TViewModel>()
            where TViewModel : class, new();

        void RunApplication<TViewModel>(TViewModel viewModel)
            where TViewModel : class;

        void RunApplication<TViewModel>(string xamlFile)
            where TViewModel : class, new();

        void RunApplication<TViewModel>(string xamlFile, TViewModel viewModel)
            where TViewModel : class;

        void RunApplication(string xamlFile);

        void RunInSTA(Action action);

        DependencyObject LoadXaml(string xamlFile);
    }
}