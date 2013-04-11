using System;
using System.Threading;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class ThreadManager : IThreadManager
    {
        public void RunInSTA(Action action)
        {
            var thread = new Thread(() => action());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}