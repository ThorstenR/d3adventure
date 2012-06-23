using System;
using System.Collections;
using System.Text;
using System.DirectoryServices;
using System.Management;
using System.Security.Principal;

/// Used by CpuUsage
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Utilities.ProcessTools
{
    public static class PT
    {

        public static int GetProcessID(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length == 0)
                return -1;
            else
                return p[0].Id;
        }

        public static IntPtr GetProcessHandle(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length == 0)
                return IntPtr.Zero;
            else
                return p[0].MainWindowHandle;
        }

        public static bool isAdmin(string processName)
        {

            string ProcessOwner = "";
            string ProcessDomain = "";

            System.Management.ObjectQuery x = new System.Management.ObjectQuery("Select * From Win32_Process where Name='" + processName + ".exe" + "'");
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(x);
            foreach (System.Management.ManagementObject mo in mos.Get())
            {
                string[] s = new string[2];
                mo.InvokeMethod("GetOwner", (object[])s);
                ProcessOwner = s[0].ToString();
                ProcessDomain = s[1].ToString();
                break;
            }

            string userPath = ProcessDomain + "/" + ProcessOwner;

            using (DirectoryEntry groupEntry = new DirectoryEntry("WinNT://./Administrators,group"))
            {
                foreach (object member in (IEnumerable)groupEntry.Invoke("Members"))
                {
                    using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                    {
                        Console.WriteLine(memberEntry.Path);
                        if (memberEntry.Path.Contains(userPath))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        public class CpuUsage
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool GetSystemTimes(
                        out ComTypes.FILETIME lpIdleTime,
                        out ComTypes.FILETIME lpKernelTime,
                        out ComTypes.FILETIME lpUserTime
                        );

            ComTypes.FILETIME _prevSysKernel;
            ComTypes.FILETIME _prevSysUser;

            TimeSpan _prevProcTotal;

            Int16 _cpuUsage;
            DateTime _lastRun;
            long _runCount;

            public CpuUsage()
            {
                _cpuUsage = -1;
                _lastRun = DateTime.MinValue;
                _prevSysUser.dwHighDateTime = _prevSysUser.dwLowDateTime = 0;
                _prevSysKernel.dwHighDateTime = _prevSysKernel.dwLowDateTime = 0;
                _prevProcTotal = TimeSpan.MinValue;
                _runCount = 0;
            }

            public short GetUsage()
            {
                short cpuCopy = _cpuUsage;
                if (Interlocked.Increment(ref _runCount) == 1)
                {
                    if (!EnoughTimePassed)
                    {
                        Interlocked.Decrement(ref _runCount);
                        return cpuCopy;
                    }

                    ComTypes.FILETIME sysIdle, sysKernel, sysUser;
                    TimeSpan procTime;

                    Process process = Process.GetCurrentProcess();
                    procTime = process.TotalProcessorTime;

                    if (!GetSystemTimes(out sysIdle, out sysKernel, out sysUser))
                    {
                        Interlocked.Decrement(ref _runCount);
                        return cpuCopy;
                    }

                    if (!IsFirstRun)
                    {
                        UInt64 sysKernelDiff = SubtractTimes(sysKernel, _prevSysKernel);
                        UInt64 sysUserDiff = SubtractTimes(sysUser, _prevSysUser);

                        UInt64 sysTotal = sysKernelDiff + sysUserDiff;

                        Int64 procTotal = procTime.Ticks - _prevProcTotal.Ticks;

                        if (sysTotal > 0)
                        {
                            _cpuUsage = (short)((100.0 * procTotal) / sysTotal);
                        }
                    }

                    _prevProcTotal = procTime;
                    _prevSysKernel = sysKernel;
                    _prevSysUser = sysUser;

                    _lastRun = DateTime.Now;

                    cpuCopy = _cpuUsage;
                }
                Interlocked.Decrement(ref _runCount);

                return cpuCopy;

            }

            private UInt64 SubtractTimes(ComTypes.FILETIME a, ComTypes.FILETIME b)
            {
                UInt64 aInt = ((UInt64)(a.dwHighDateTime << 32)) | (UInt64)a.dwLowDateTime;
                UInt64 bInt = ((UInt64)(b.dwHighDateTime << 32)) | (UInt64)b.dwLowDateTime;

                return aInt - bInt;
            }

            private bool EnoughTimePassed
            {
                get
                {
                    const int minimumElapsedMS = 250;
                    TimeSpan sinceLast = DateTime.Now - _lastRun;
                    return sinceLast.TotalMilliseconds > minimumElapsedMS;
                }
            }

            private bool IsFirstRun
            {
                get
                {
                    return (_lastRun == DateTime.MinValue);
                }
            }

            /// <summary>
            /// Method to check not only if a file is already open, but if the
            /// but also if read and write permissions exist
            /// </summary>
            /// <param name="file">The file we wish to check</param>
            /// <returns></returns>
            public bool IsFileOpenOrReadOnly(string file)
            {
                try
                {
                    //first make sure it's not a read only file
                    if ((File.GetAttributes(file) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                    {
                        //first we open the file with a FileStream
                        using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                        {
                            try
                            {
                                stream.ReadByte();
                                return false;
                            }
                            catch (IOException)
                            {
                                return true;
                            }
                            finally
                            {
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                    }
                    else
                        return true;
                }
                catch (IOException)
                {
                    return true;
                }
            }
        }

    }
}
