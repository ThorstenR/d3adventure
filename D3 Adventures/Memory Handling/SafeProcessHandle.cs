using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace D3_Adventures.Memory_Handling
{

    [SuppressUnmanagedCodeSecurity, HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    internal class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeProcessHandle()
            : base(true)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public SafeProcessHandle(IntPtr handle)
            : base(true)
        {
            base.SetHandle(handle);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        protected internal static extern bool CloseHandle(IntPtr hObject);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            Console.WriteLine("Releasing handle on " + this.handle.ToString("X"));
            return CloseHandle(base.handle);
        }
    }
    

}
