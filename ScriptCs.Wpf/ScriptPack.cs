using System.Collections.Generic;

using ScriptCs.Contracts;

namespace ScriptCs.Wpf
{
    public class ScriptPack : IScriptPack
    {
        public void Initialize(IScriptPackSession session)
        {
            var references = new List<string>
            {
                "WindowsBase",
                "PresentationCore",
                "PresentationFramework",
                "System.Xaml",
                "System.Xml"
            };

            references.ForEach(session.AddReference);

            var namespaces = new List<string>
            {
                "System.ComponentModel",
                "System.Diagnostics",
                "System.Threading",
                "System.Windows",
                "System.Windows.Input"
            };

            namespaces.ForEach(session.ImportNamespace);
        }

        public IScriptPackContext GetContext()
        {
            return new Wpf();
        }

        public void Terminate() { }
    }
}