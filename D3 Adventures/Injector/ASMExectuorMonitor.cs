using System;
using System.Runtime.CompilerServices;
using System.Threading;
namespace D3_Adventures.Injector
{
    internal class ASMExecutorMonitor : IDisposable
    {
        [CompilerGenerated]
        private ASMExecutor class18_0;
        private static int int_0;

        public ASMExecutorMonitor(ASMExecutor executor)
        {
            if (int_0 == 0)
            {
                this.Executor = executor;
                Monitor.Enter(this.Executor.publicLock);
                this.Executor.method_0();
            }
            Interlocked.Increment(ref int_0);
        }

        public void Dispose()
        {
            Interlocked.Decrement(ref int_0);
            if (int_0 == 0)
            {
                this.Executor.method_1();
                Monitor.Exit(this.Executor.publicLock);
            }
        }

        public ASMExecutor Executor
        {
            get;
            set;
        }
    }
}

