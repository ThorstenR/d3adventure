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
        public static ReadWriteMemory mem = new ReadWriteMemory(Utilities.GetProcessHandle(exeName));
        public static bool debugMessages = false;
        public static bool screwWarden = false; // turn to true to use things that use memory writing

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

                Data.Vec3 pos = Data.getCurrentPos();
                Console.WriteLine("X:" + pos.x + " Y:" + pos.y + " Z:" + pos.z);

                Actions.moveToPos(418, 411, 0);

                pos = Data.getCurrentPos();
                Console.WriteLine("X:" + pos.x + " Y:" + pos.y + " Z:" + pos.z);

                Console.ReadKey();
            }
            
        }
    }
}
