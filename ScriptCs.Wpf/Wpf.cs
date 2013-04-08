using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

using ScriptCs.Contracts;

namespace ScriptCs.Wpf
{
    public class Wpf : IScriptPackContext
    {
        public void RunApplication<TViewModel>()
            where TViewModel : new()
        {
            RunApplication(new TViewModel());
        }

        public void RunApplication<TViewModel>(TViewModel viewModel)
        {
            var xamlFile = LocateViewFor<TViewModel>();

            if (string.IsNullOrEmpty(xamlFile))
                throw new InvalidOperationException(string.Format("Could not locate view for {0}", typeof(TViewModel).Name));

            RunApplication(xamlFile, viewModel);
        }

        public void RunApplication<TViewModel>(string xamlFile)
            where TViewModel : new()
        {
            RunApplication(xamlFile, new TViewModel());
        }

        public void RunApplication<TViewModel>(string xamlFile, TViewModel viewModel)
        {
            RunInSTA(() =>
            {
                var view = LoadXaml(xamlFile);

                if (view == null)
                    throw new InvalidOperationException(string.Format("Failed to load XAML file '{0}'", xamlFile));

                view.DataContext = viewModel;

                var mainWindow = new Window { Content = view, SizeToContent = SizeToContent.WidthAndHeight };
                var application = new WpfApplication(mainWindow);

                application.Run();
            });
        }

        public FrameworkElement LoadXaml(string xamlFile)
        {
            using (var fileStream = File.OpenRead(xamlFile))
            {
                return XamlReader.Load(fileStream) as FrameworkElement;
            }
        }

        public void RunInSTA(Action action)
        {
            var thread = new Thread(() => action());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private static string LocateViewFor<T>()
        {
            const string ViewModelString = "ViewModel";

            var viewModelName = typeof(T).Name;

            if (!viewModelName.EndsWith(ViewModelString, StringComparison.InvariantCultureIgnoreCase))
                return null;

            var viewName = viewModelName.Replace(ViewModelString, string.Empty);
            if (!string.IsNullOrEmpty(viewName))
            {
                var fileName = string.Format("{0}View.xaml", viewName);
                var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                if (File.Exists(filePath))
                    return filePath;
            }

            return null;
        }

        private class WpfApplication : Application
        {
            private readonly Window _mainWindow;

            public WpfApplication(Window mainWindow)
            {
                _mainWindow = mainWindow;
            }

            protected override void OnStartup(StartupEventArgs e)
            {
                _mainWindow.Show();
            }
        }
    }
}