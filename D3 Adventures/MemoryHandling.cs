using System;
//using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Utilities.MemoryHandling
{
    public static class Imports
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            ProcessAccessFlags dwDesiredAccess,
            bool bInheritHandle,
            Int32 dwProcessID);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            uint lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(
            IntPtr lpHandle,
            IntPtr lpAddress,
            byte[] lpBuffer,
            int lpSize,
            out int lpBytesWrote);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
          IntPtr hProcess,
          IntPtr lpBaseAddress,
          [Out] byte[] lpBuffer,
          int dwSize,
          out int lpNumberOfBytesRead
         );

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(
            IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern Int32 GetLastError();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

    }

    public class ReadWriteMemory
    {
        private IntPtr m_lpHandle;
        private int m_lpBytesWrote;
        private int m_lpBytesRead;

        #region Constructors & Destructor
        public ReadWriteMemory(IntPtr hwnd)
        {
            uint idOut;
            Imports.GetWindowThreadProcessId(hwnd, out idOut);
            m_lpHandle = Imports.OpenProcess(Imports.ProcessAccessFlags.All, false, (int)idOut);

            if (m_lpHandle.ToInt32() == 0)
            {
                Console.WriteLine("Failed To Open Process: " + hwnd + ". Error: " + Imports.GetLastError(), "Error");
            }
        }

        public ReadWriteMemory(int hwnd)
        {
            m_lpHandle = Imports.OpenProcess(Imports.ProcessAccessFlags.All, false, hwnd);

            if (m_lpHandle.ToInt32() == 0)
            {
                Console.WriteLine("Failed To Open Process: " + hwnd + ". Error: " + Imports.GetLastError(), "Error");
            }
        }

        ~ReadWriteMemory()
        {
            Imports.CloseHandle(m_lpHandle);
        }
        #endregion

        #region ReadMemory
        public bool ReadMemory(IntPtr memoryLocation, int bufferLength, out byte[] lpBuffer)
        {
            lpBuffer = new byte[bufferLength];
            if (m_lpHandle.ToInt32() == 0) return false;
            if (Imports.ReadProcessMemory(m_lpHandle, memoryLocation, lpBuffer, bufferLength, out m_lpBytesRead) == false)
            {
                Console.WriteLine("Failed to read from Address: {0:X}. Error: {1}", memoryLocation, Imports.GetLastError());
                return false;
            }
            if (m_lpBytesRead != bufferLength)
            { 
                Console.WriteLine("Failed to read the correct amount of bytes"); 
                return false; 
            }
            return true;
        }

        public bool ReadMemory(uint memoryLocation, int bufferLength, out byte[] lpBuffer)
        {
            lpBuffer = new byte[bufferLength];
            if (m_lpHandle.ToInt32() == 0) return false;
            if (Imports.ReadProcessMemory(m_lpHandle, (IntPtr)memoryLocation, lpBuffer, bufferLength, out m_lpBytesRead) == false)
            {
                Console.WriteLine("Failed to read from Address: {0:X}. Error: {1}", memoryLocation, Imports.GetLastError());
                return false;
            }
            return true;
        }

        public bool ReadMemoryRetry(uint memoryLocation, int bufferLength, out byte[] lpBuffer)
        {
            bool worked = false;
            lpBuffer = new byte[bufferLength];
            if (m_lpHandle.ToInt32() == 0) return false;
            while (!worked)
            {
                try
                {
                    Imports.ReadProcessMemory(m_lpHandle, (IntPtr)memoryLocation, lpBuffer, bufferLength, out m_lpBytesRead);
                    worked = true;
                }
                catch
                {

                }
            }
            return true;
        }

        public bool ReadMemory(int memoryLocation, int bufferLength, out byte[] lpBuffer)
        {
            lpBuffer = new byte[bufferLength];
            if (m_lpHandle.ToInt32() == 0) return false;
            if (Imports.ReadProcessMemory(m_lpHandle, (IntPtr)memoryLocation, lpBuffer, bufferLength, out m_lpBytesRead) == false)
            {
                Console.WriteLine("Failed to read from Address: {0:X}. Error: {1}", memoryLocation, Imports.GetLastError());
                return false;
            }
            return true;
        }

        public bool ReadMemory(uint memoryLocation, int bufferLength, out uint uintOut)
        {
            uintOut = 0x0;
            if (m_lpHandle.ToInt32() == 0) return false;
            byte[] lpBuffer = new byte[sizeof(uint)];
            if (Imports.ReadProcessMemory(m_lpHandle, (IntPtr)memoryLocation, lpBuffer, bufferLength, out m_lpBytesRead) == false)
            {
                Console.WriteLine("Failed to read from Address: {0:X}. Error: {1}", memoryLocation, Imports.GetLastError());
                return false;
            }
            uintOut = BitConverter.ToUInt32(lpBuffer, 0);
            return true;
        }

        public Object ReadMemory(IntPtr address, Type type) // Thanks ApacheChief @ http://www.edgeofnowhere.cc/viewtopic.php?p=3117314
        {
            byte[] data;
            this.ReadMemory(address, Marshal.SizeOf(type), out data);
            return RawDeserialize(data, 0, type);
        }

        public Object ReadMemory(uint address, Type type) // Thanks ApacheChief @ http://www.edgeofnowhere.cc/viewtopic.php?p=3117314
        {
            return ReadMemory((IntPtr)address, type);
        }


        public int ReadMemoryAsInt(uint location)
        {
            byte[] bytesRead;
            ReadMemory(location, 4, out bytesRead);
            return BitConverter.ToInt32(bytesRead, 0);
        }

        public uint ReadMemoryAsUint(uint location)
        {
            byte[] bytesRead;
            ReadMemory(location, 4, out bytesRead);
            return BitConverter.ToUInt32(bytesRead, 0);
        }

        public float ReadMemoryAsFloat(uint location)
        {
            byte[] bytesRead;
            ReadMemory(location, 4, out bytesRead);
            return BitConverter.ToSingle(bytesRead, 0);
        }

        public int ReadMemoryAsIntRetry(uint location)
        {
            byte[] bytesRead;
            ReadMemoryRetry(location, 4, out bytesRead);
            return BitConverter.ToInt32(bytesRead, 0);
        }

        public string ReadMemoryAsString(uint location, int length)
        {
            byte[] bytesRead;
            ReadMemory(location, length, out bytesRead);
            return ASCIIEncoding.ASCII.GetString(bytesRead, 0, length).TrimEnd(new char[] { (char)0 });
        }

        public string ReadMemoryAsHex(uint location, int length)
        {
            byte[] bytesRead;
            ReadMemory(location, length, out bytesRead);
            StringBuilder sb = new StringBuilder();
            string hexTemp = "";
            int count = 0;
            foreach (byte b in bytesRead)
            {
                hexTemp = b.ToString("X");

                if (b < 16)
                    sb.Append("0" + hexTemp + " ");
                else
                    sb.Append(hexTemp + " ");

                count++;

                /* Posted by kal in chat
                 * 
                      public static string ByteArrayToString(byte[] ba)
                        {
                            string hex = BitConverter.ToString(ba);
                            return hex.Replace("-"," ");
                        }
                 *
                 * I should probably try this one out and bench mark the two next time ;)
                 * surely it will be faster and is much less complicated :p
                 */


                // beautiful formatting like CE does
                //if (count >= 16)
                //{
                //    count = 0;
                //    hex = hex + "\n";
                //}
            }

            //string hex = BitConverter.ToInt32(bytesRead, 0).ToString("X");
            //string left, right;
            //int count = 0;

            //// put spaces every two chars
            //for (int i = 0; i < hex.Length - 2; i = i + 2)
            //{
            //    left = hex.Remove(i + count);
            //    right = hex.Substring(i + count);
            //    hex = left + " " + right;
            //    count++;
            //}

            return sb.ToString().Trim();
        }

        public bool WriteMemoryAsInt(uint location, int integer)
        {
            byte[] bytes = BitConverter.GetBytes(integer);
            return WriteMemory(location, bytes.Length, ref bytes);
        }

        public bool WriteMemoryAsFloat(uint location, float _float)
        {
            byte[] bytes = BitConverter.GetBytes(_float);
            return WriteMemory(location, bytes.Length, ref bytes);
        }

#endregion

        #region Serializers
        public static object RawDeserialize(byte[] rawData, int position, Type anyType) // http://bytes.com/topic/c-sharp/answers/249770-byte-structure#post1009966
        {
            int rawsize = Marshal.SizeOf(anyType);
            if (rawsize > rawData.Length)
                return null;
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anyType);
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }

        public static byte[] RawSerialize(object anything) // http://bytes.com/topic/c-sharp/answers/249770-byte-structure#post1009966
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }

        //public static T PinAndCast<T>(this Array o) where T : new()
        //{
        //    var handle = System.Runtime.InteropServices.GCHandle.Alloc(o, GCHandleType.Pinned);
        //    T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        //    handle.Free();
        //    return result;
        //}
        #endregion

        #region WriteMemory
        public bool WriteMemory(IntPtr memoryLocation, int bufferLength, ref byte[] lpBuffer)
        {
            if (m_lpHandle.ToInt32() == 0) return false;
            if (Imports.WriteProcessMemory(m_lpHandle, memoryLocation, lpBuffer, bufferLength, out m_lpBytesWrote) == false)
            {
                //Console.WriteLine("Failed to write to Address: {0:X}. Error: {1}", memoryLocation, Imports.GetLastError());
                return false;
            }
            return true;
        }

        public bool WriteMemory(uint memoryLocation, int bufferLength, ref byte[] lpBuffer)
        {
            return WriteMemory((IntPtr)memoryLocation, bufferLength, ref lpBuffer);
        }
#endregion

        public void CreateRemoteThread(uint address, IntPtr parameter)
        {
            Imports.CreateRemoteThread(m_lpHandle, IntPtr.Zero, 0, address, parameter, 0, IntPtr.Zero);
        }

        #region Pattern Search Tools - Thanks to BlackMagic - Modified by UserNamex32
        /// <summary>
        /// Finds a pattern or signature inside another process.
        /// </summary>
        /// <param name="dwStart">Address on which the search will start.</param>
        /// <param name="dwEnd">Address on which the search will end.</param>
        /// <param name="bPattern">A byte-array representing the pattern to be found.</param>
        /// <param name="szMask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public uint FindPattern(uint dwStart, uint dwEnd, byte[] bPattern, string szMask)
        {
            if (dwStart > dwEnd)
                throw new ArgumentException("Start Address cannot be bigger than the End Address");

            int nSize = (int)(dwEnd - dwStart);

            if (bPattern == null || bPattern.Length == 0)
                throw new ArgumentNullException("bData");

            if (bPattern.Length != szMask.Length)
                throw new ArgumentException("bData and szMask must be of the same size");

            byte[] bData;
            ReadMemory(dwStart, nSize, out bData);
            if (bData == null)
                throw new Exception("Could not read memory in FindPattern.");
            
            uint loc = FindPattern(bData, bPattern, szMask);

            if(loc == 0)
                return 0;
            else
                return (uint)(dwStart + loc);
        }

        public uint FindPatternRetry(uint dwStart, uint dwEnd, byte[] bPattern, string szMask)
        {
            if (dwStart > dwEnd)
                throw new ArgumentException("Start Address cannot be bigger than the End Address");

            int nSize = (int)(dwEnd - dwStart);

            if (bPattern == null || bPattern.Length == 0)
                throw new ArgumentNullException("bData");

            if (bPattern.Length != szMask.Length)
                throw new ArgumentException("bData and szMask must be of the same size");

            byte[] bData;
            ReadMemoryRetry(dwStart, nSize, out bData);
            if (bData == null)
                throw new Exception("Could not read memory in FindPattern.");

            uint loc = FindPattern(bData, bPattern, szMask);

            if (loc == 0)
                return 0;
            else
                return (uint)(dwStart + loc);
        }

        /// <summary>
        /// Finds a given pattern in an array of bytes.
        /// </summary>
        /// <param name="bData">Array of bytes in which to search for the pattern.</param>
        /// <param name="bPattern">A byte-array representing the pattern to be found.</param>
        /// <param name="szMask">A string of 'x' (match), '!' (not-match), or '?' (wildcard).</param>
        /// <returns>Returns 0 on failure, or the address of the start of the pattern on success.</returns>
        public static uint FindPattern(byte[] bData, byte[] bPattern, string szMask)
        {
            if (bData == null || bData.Length == 0)
                throw new ArgumentNullException("bData");

            if (bPattern == null || bPattern.Length == 0)
                throw new ArgumentNullException("bPattern");

            if (szMask == string.Empty)
                throw new ArgumentNullException("szMask");

            if (bPattern.Length != szMask.Length)
                throw new ArgumentException("Pattern and Mask lengths must be the same.");

            int ix, iy;
            bool bFound = false;
            int PatternLength = bPattern.Length;
            int DataLength = bData.Length - PatternLength;

            for (ix = 0; ix < DataLength; ix++)
            {
                bFound = true;
                for (iy = 0; iy < PatternLength; iy++)
                {
                    if ((szMask[iy] == 'x' && bPattern[iy] != bData[ix + iy]) ||
                        (szMask[iy] == '!' && bPattern[iy] == bData[ix + iy]))
                    {
                        bFound = false;
                        break;
                    }
                }

                if (bFound)
                    return (uint)ix;
            }

            return 0;
        }
        #endregion
    }

}