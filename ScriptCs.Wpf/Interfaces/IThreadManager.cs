using System;

namespace ScriptCs.Wpf.Interfaces
{
    public interface IThreadManager 
    {
        void RunInSTA(Action action);
    }
}