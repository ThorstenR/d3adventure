using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Structures;
using System.IO;
using D3_Adventures;
using D3_Adventures.Memory_Handling;

namespace UIDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MemoryManager mem = new MemoryManager(Utilities.GetProcessHandle("Diablo III")))
            {
                Globals.mem = mem;
                mem.Attach();
                //UIElement.test();
                ulong hash = 11552879775495564696;
                UIElement elem1 = UIElement.GetByHash(hash);
  
                elem1.MouseEnter();
                //System.Threading.Thread.Sleep(2000);
                elem1.MouseOut();
                elem1.Click();
                //elem1.Click();

            }

            /*ulong hash = 11552879775495564696;
            UIElement elem1 = UIElement.GetByHash(hash);
            int t = (int)(hash >> 20);
            int t2 = (int)hash;
            var elems = UIElement.GetAll().OrderBy(p => p.Name).ToList();
            var pri = elems.Where(p => p.Text != null && p.Name.Contains("Root.NormalLayer.BattleNetAuctionHouse_main.LayoutRoot.OverlayContainer.TabContentContainer.SearchTabContent.SearchListContent.SearchItemList.ItemListContainer.ItemList.item 0 list.")).ToList();
            foreach (var elem in elems)
                File.AppendAllText(@"c:\UIDump.txt", "Hash: " + elem.Hash + " " + elem.Name + Environment.NewLine);
            Console.Read();*/
        }
    }
}
