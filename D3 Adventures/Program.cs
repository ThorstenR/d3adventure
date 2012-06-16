using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using Utilities.MemoryHandling;

namespace D3_Adventures
{
    class Program
    {
        public static string exeName = "Diablo III";
        private static IntPtr test = Utilities.GetProcessHandle(exeName);
        public static ReadWriteMemory mem = new ReadWriteMemory(test);
        public static bool debugMessages = false;

        static void Main(string[] args)
        {
            if (!Utilities.isAdmin(System.Diagnostics.Process.GetCurrentProcess().ProcessName))
            {
                Console.WriteLine("You must be running as an administrator!");
                Console.ReadKey();
            }
            else
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                Data.gameObject[] objs = Data.iterateObjectList();
                st.Stop();
                Console.WriteLine("Took: "+st.Elapsed.ToString());
                Console.ReadKey();
            }
            
        }
    }
}
