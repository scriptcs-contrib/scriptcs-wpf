using System;
using System.IO;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class ViewLocator : IViewLocator
    {
        private readonly IFileSystem _fileSystem;

        public ViewLocator(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string LocateViewFor<T>() where T : class
        {
            return LocateViewFor(typeof(T).Name);
        }

        public string LocateViewFor(string viewModelName)
        {
            if (string.IsNullOrEmpty(viewModelName)) throw new ArgumentNullException("viewModelName");

            const string ViewModelString = "ViewModel";

            if (!viewModelName.EndsWith(ViewModelString, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException(
                    "The ViewModel type name has to end with 'ViewModel' to use convention loading.");
            }

            var viewName = viewModelName.Replace(ViewModelString, string.Empty);

            var filePath = Path.Combine(_fileSystem.CurrentDirectory, string.Format("{0}View.xaml", viewName));

            if (_fileSystem.FileExists(filePath))
            {
                return filePath;
            }

            throw new InvalidOperationException(string.Format("Could not locate View for ViewModel: {0}. Tried: {1}", viewModelName, filePath));
        }
    }
}