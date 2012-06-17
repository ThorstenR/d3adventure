/*
 *  Original Credits goto UnknOwned
 *      http://www.ownedcore.com/forums/diablo-3/diablo-3-bots-programs/diablo-3-memory-editing/356250-autoit-diablo-3-click-move-interaction-actor-indexing.html
 *  Conversion by UserNamex32
 *      http://code.google.com/p/d3adventure/
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using Utilities.MemoryHandling;
using D3_Adventures.Structures;
using System.IO;

namespace D3_Adventures
{
    class Program
    {
        public static string exeName = "Diablo III";
        public static ReadWriteMemory mem = new ReadWriteMemory(Utilities.GetProcessHandle(exeName));
        public static bool debugMessages = false;
        public static bool screwWarden = true; // turn to true to use things that use memory writing. Such actions might be detectable by warden in the future.

        static void Main(string[] args)
        {
            var elem1 = (UIElement)mem.ReadMemory(0x1cba8344, typeof(UIElement));
            Console.Write(elem1.GetName());
            uint counter = 0;
            uint uielemePointer;
            List<UIElement> Elems = new List<UIElement>();
            while ((Offsets.uielements + counter * 4) < 0x19aa4000)
            {
                try
                {
                    uielemePointer = mem.ReadMemoryAsUint((Offsets.uielements + counter * 4));
                    counter++;
                    if (uielemePointer == 0) continue;
                    var elem = (UIElement)mem.ReadMemory(uielemePointer, typeof(UIElement));
                    File.AppendAllText(@"c:\Toto.txt","PARENT : " +  elem.GetName() + "\r\n");
                    while (elem.ParentElement != 0)
                    {
                        elem = (UIElement)mem.ReadMemory(elem.ParentElement, typeof(UIElement));
                        File.AppendAllText(@"c:\Toto.txt", "\t\t" + elem.GetName() + "\r\n");
                    }
                    //Elems.Add(elem);
                    
                }
                catch { }
                //elem.Unknown.wh
                
            }
            var u = Elems.Where(p => p.ParentElement != 0).Take(20).ToList();
            foreach (var child in u)
            {
                var parent = (UIElement)mem.ReadMemory(child.ParentElement, typeof(UIElement));
                File.AppendAllText(@"c:\Toto.txt",parent.GetName() + "\r\n");
                File.AppendAllText(@"c:\Toto.txt", "\t\t" + child.GetName() + "\r\n");
            }
            Console.Read();
            /*if (!Utilities.isAdmin(System.Diagnostics.Process.GetCurrentProcess().ProcessName))
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
            }*/
            
        }
    }
}
