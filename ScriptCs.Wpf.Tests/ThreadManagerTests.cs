using System.Threading;

using Should;

using Xunit;

namespace ScriptCs.Wpf.Tests
{
    public class ThreadManagerTests
    {
        public class TheRunInSTAMethod
        {
            [Fact]
            public void ShouldInvokeTheGivenActionOnStaThread()
            {
                var threadManager = new ThreadManager();

                var currentThread = Thread.CurrentThread;

                Thread executedThread = null;
                ApartmentState? apartmentState = null;
                threadManager.RunInSTA(() =>
                {
                    executedThread = Thread.CurrentThread;
                    apartmentState = executedThread.GetApartmentState();
                });

                executedThread.ShouldNotBeSameAs(currentThread);
                apartmentState.ShouldEqual(ApartmentState.STA);
            }
        }
    }
}