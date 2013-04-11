using System;
using System.Windows;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class Wpf : IWpfContext
    {
        private readonly IApplicationLauncher _applicationLauncher;

        private readonly IViewLocator _viewLocator;

        private readonly IXamlLoader _xamlLoader;

        private readonly IThreadManager _threadManager;

        public Wpf(
            IApplicationLauncher applicationLauncher,
            IViewLocator viewLocator,
            IXamlLoader xamlLoader,
            IThreadManager threadManager)
        {
            _applicationLauncher = applicationLauncher;
            _viewLocator = viewLocator;
            _xamlLoader = xamlLoader;
            _threadManager = threadManager;
        }

        public void RunApplication<TViewModel>()
            where TViewModel : class, new()
        {
            RunApplication(new TViewModel());
        }

        public void RunApplication<TViewModel>(TViewModel viewModel)
            where TViewModel : class
        {
            var xamlFile = _viewLocator.LocateViewFor<TViewModel>();
            RunApplication(xamlFile, viewModel);
        }

        public void RunApplication<TViewModel>(string xamlFile)
            where TViewModel : class, new()
        {
            RunApplication(xamlFile, new TViewModel());
        }

        public void RunApplication<TViewModel>(string xamlFile, TViewModel viewModel)
            where TViewModel : class
        {
            RunApplication(xamlFile, (object) viewModel);
        }

        public void RunApplication(string xamlFile)
        {
            RunApplication(xamlFile, null);
        }

        public DependencyObject LoadXaml(string xamlFile)
        {
            return _xamlLoader.LoadXaml(xamlFile);
        }

        public void RunInSTA(Action action)
        {
            _threadManager.RunInSTA(action);
        }

        private void RunApplication(string xamlFile, object viewModel)
        {
            if (string.IsNullOrEmpty(xamlFile)) throw new ArgumentNullException("xamlFile");

            _threadManager.RunInSTA(() =>
            {
                var view = _xamlLoader.LoadXaml(xamlFile);
                _applicationLauncher.CreateAndRunApplication(view, viewModel);
            });
        }
    }
}