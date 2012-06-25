using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Memory_Handling;

namespace D3_Adventures.Injector
{
    public class MemoryInjector : IDisposable
    {
        public MemoryManager Mngr { get; set; }
        public IntPtr Address { get; set; }


        public MemoryInjector(MemoryManager mngr, int size)
        {
            Mngr = mngr;
            Address = mngr.AllocateMemory(size, Imports.AllocationType.Commit, Imports.MemoryProtection.ExecuteReadWrite);
        }

        public void Dispose()
        {
            Mngr.FreeMemory(Address);
            Address = IntPtr.Zero;
        }
    }
}
