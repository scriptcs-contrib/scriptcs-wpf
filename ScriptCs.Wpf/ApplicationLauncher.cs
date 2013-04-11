using System;
using System.Windows;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class ApplicationLauncher : IApplicationLauncher
    {
        public void CreateAndRunApplication(DependencyObject view, object viewModel)
        {
            if (view == null) throw new ArgumentNullException("view");

            var mainWindow = new Window
            {
                Content = view,
                DataContext = viewModel,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            new WpfApplication(mainWindow).Run();
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