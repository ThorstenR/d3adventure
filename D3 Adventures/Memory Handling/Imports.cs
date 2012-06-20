using System.Runtime.InteropServices;
using System;

using System.Security;

namespace D3_Adventures.Memory_Handling
{
    public static class Imports
    {
        // Methods
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, uint lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll")]
        internal static extern int GetLastError();
        [DllImport("kernel32")]
        internal static extern IntPtr GetModuleHandleW([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);
        [SuppressUnmanagedCodeSecurity, DllImport("kernel32", CharSet = CharSet.Ansi)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("kernel32.dll")]
        internal static extern SafeProcessHandle OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessID);
        [DllImport("kernel32", SetLastError = true)]
        internal static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern unsafe bool ReadProcessMemory(SafeProcessHandle hProcess, IntPtr lpBaseAddress, byte* lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        [DllImport("kernel32")]
        internal static extern IntPtr VirtualAllocEx(SafeProcessHandle hProcess, uint dwAddress, int nSize, AllocationType dwAllocationType, MemoryProtection dwProtect);
        [DllImport("kernel32")]
        internal static extern bool VirtualFreeEx(SafeProcessHandle hProcess, IntPtr dwAddress, int nSize, MemoryFree dwFreeType);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr lpHandle, IntPtr lpAddress, byte[] lpBuffer, int lpSize, out int lpBytesWrote);

        // Nested Types
        [Flags]
        internal enum AllocationType
        {
            Commit = 0x1000,
            Decommit = 0x4000,
            LargePages = 0x20000000,
            Physical = 0x400000,
            Release = 0x8000,
            Reserve = 0x2000,
            Reset = 0x80000,
            TopDown = 0x100000,
            WriteWatch = 0x200000
        }

        [Flags]
        internal enum MemoryFree
        {
            MemDecommit = 0x4000,
            MemRelease = 0x8000
        }

        [Flags]
        internal enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            GuardModifierflag = 0x100,
            NoAccess = 1,
            NoCacheModifierflag = 0x200,
            ReadOnly = 2,
            ReadWrite = 4,
            WriteCombineModifierflag = 0x400,
            WriteCopy = 8
        }

        [Flags]
        internal enum ProcessAccessFlags : uint
        {
            All = 0x1f0fff,
            CreateThread = 2,
            DupHandle = 0x40,
            QueryInformation = 0x400,
            SetInformation = 0x200,
            Synchronize = 0x100000,
            Terminate = 1,
            VMOperation = 8,
            VMRead = 0x10,
            VMWrite = 0x20
        }
    }
}
