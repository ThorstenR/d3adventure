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
    public class Program
    {
        public static string exeName = "Diablo III";
        public static MemoryManager mem = new MemoryManager(Utilities.GetProcessHandle(exeName));
        public static bool debugMessages = false;
        public static bool screwWarden = true; // turn to true to use things that use memory writing. Such actions might be detectable by warden in the future.

        static void Main(string[] args)
        {
            ulong hash = 11552879775495564696;
            UIElement elem1 = UIElement.GetByHash(hash);
            int t = (int)(hash >> 20);
            int t2 = (int)hash;
            var elems = UIElement.GetAll().OrderBy(p => p.Name).ToList();
            var pri = elems.Where(p => p.Text != null && p.Name.Contains("Root.NormalLayer.BattleNetAuctionHouse_main.LayoutRoot.OverlayContainer.TabContentContainer.SearchTabContent.SearchListContent.SearchItemList.ItemListContainer.ItemList.item 0 list.")).ToList();
            foreach (var elem in elems)
                File.AppendAllText(@"c:\UIDump.txt", "Hash: " + elem.Hash + " " + elem.Name + Environment.NewLine);
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
