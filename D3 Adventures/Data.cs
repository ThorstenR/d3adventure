using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3_Adventures.Structures;
using D3_Adventures.Memory_Handling;

namespace D3_Adventures
{
    public static class Data
    {
        private static MemoryManager mem = Globals.mem;

        public static uint toonID = 0x77BC0000; // your toon's guid

        public static Actor GetMe()
        {
            return IterateActors().Where(o => o.id_acd == toonID).FirstOrDefault();
        }

        public static uint GetActorCount()
        {
            uint count = mem.ReadMemoryAsUint(Offsets.itrObjectManagerCount);
            if (Globals.debugMessages) Console.WriteLine("Number of Actors: " + count);
            return count;
        }

        public static Vec3 GetCurrentPos()
        {
            Vec3 ret;
            ret.x = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurX);
            ret.y = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurY);
            ret.z = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurZ);
             
            // if all three == 0 then the user needs to click first
            //  could throw an error or just send a click to the center of the window
            return ret;//Globals.Me.Pos1;//
        }

        public static Actor[] IterateActors()
        {
            if (Globals.debugMessages)
            {
                Console.WriteLine("Iterating through Actors");
                Console.WriteLine("First Actor Location At: " + Offsets.itrObjectManagerD.ToString("X"));
            }

            uint curOffset = Offsets.itrObjectManagerD;
            uint count = GetActorCount();
            Actor[] actors = new Actor[count];

            for (int i = 0; i < count; i++)
            {
                actors[i] = (Actor)mem.ReadMemory(curOffset, typeof(Actor));
                actors[i].mem_location = curOffset;
                curOffset = curOffset + Offsets.objmanagerStrucSize;
            }

            return actors;
        }

        public static ActorCommonData[] IterateACD()
        {
            if (Globals.debugMessages)
            {
                Console.WriteLine("Iterating through ACD");
                Console.WriteLine("First ACD Location At: " + Offsets.firstACD.ToString("X"));
            }

            uint curOffset = Offsets.firstACD;
            uint count = Offsets.ACDCount;
            ActorCommonData[] acds = new ActorCommonData[count];

            for (int i = 0; i < count; i++)
            {
                acds[i] = (ActorCommonData)mem.ReadMemory(curOffset, typeof(ActorCommonData));
                curOffset = curOffset + Offsets.ACDSize;
            }

            return acds;
        }

        public static Actor[] GetItems()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data1 == 2 && a.unknown_data2 == uint.MaxValue).ToArray<Actor>();
        }

        public static Actor[] GetMonsters()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data2 == 29944).ToArray<Actor>();
        }
        //FOR maphack, the next version of maphack will use getMonsters.
        public static Actor[] GetMapItems()
        {
            List<Actor> filteredItems = new List<Actor>();
            Actor[] items = GetMonsters();
            filteredItems = items.Where(mob =>
                                        mob.id_acd != Data.toonID &&
                                        mob.distanceFromMe != 0 &&
                                        mob.Exists() &&            /*is do NOT check if the monster is dead - use Actor.Alive*/
                                        mob.unknown_1C4[0] == 1 && /* Were it is a "monster"*/
                                        mob.unknown_data2 == 29944 &&
                                        mob.isAlive() == true
                                        ).ToList();
            return filteredItems.ToArray();
        }

    }
}
