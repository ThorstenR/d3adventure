using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities.MemoryHandling;
using D3_Adventures.Structures;

namespace D3_Adventures
{
    public static class Data
    {
        private static MemoryManager mem = Program.mem;

        public static uint toonID = 0x77BC0000; // your toon's guid

        public static Actor GetMe()
        {
            return IterateActors().Where(o => o.id_acd == toonID).FirstOrDefault();
        }

        public static uint getActorCount()
        {
            uint count = mem.ReadMemoryAsUint(Offsets.itrObjectManagerCount);
            if (Program.debugMessages) Console.WriteLine("Number of Actors: " + count);
            return count;
        }

        public static Vec3 getCurrentPos()
        {
            /*
            Vec3 ret;
            ret.x = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurX);
            ret.y = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurY);
            ret.z = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurZ);
            */
             
            // if all three == 0 then the user needs to click first
            //  could throw an error or just send a click to the center of the window
            return Program.me.Pos1;//ret;
        }

        // Obsolete with new Actor struct.
        public struct gameObject
        {
            public uint guid;
            public string name;
            public Vec3 position;
            public int data;
            public int data2;
            public int data3;
            public double distanceFromMe;
        }

        // Obsolete with new IterateActors
        public static gameObject[] iterateObjectList()
        {
            if (Program.debugMessages)
            {
                Console.WriteLine("Iterating through Actors");
                Console.WriteLine("First Actor Location At: " + Offsets.itrObjectManagerD.ToString("X"));
            }
            uint curOffset = Offsets.itrObjectManagerD;
            uint count = getActorCount();
            gameObject[] objects = new gameObject[count];

            for (int i = 0; i < count; i++)
            {
                uint guid = mem.ReadMemoryAsUint(curOffset + 0x4);
                string name = mem.ReadMemoryAsString(curOffset + 0x8, 64);
                float posX = mem.ReadMemoryAsFloat(curOffset + 0xB0);
                float posY = mem.ReadMemoryAsFloat(curOffset + 0xB4);
                float posZ = mem.ReadMemoryAsFloat(curOffset + 0xB8);
                int data = mem.ReadMemoryAsInt(curOffset + 0x1FC);
                int data2 = mem.ReadMemoryAsInt(curOffset + 0x1CC);
                int data3 = mem.ReadMemoryAsInt(curOffset + 0x1C0);

                Vec3 currentLoc = getCurrentPos();
                float xd = posX - currentLoc.x;
                float yd = posY - currentLoc.y;
                float zd = posZ - currentLoc.z;
                double distance = Math.Sqrt(xd * xd + yd * yd + zd * zd);

                //objects[i][0] = i;
                objects[i].guid = guid;
                objects[i].name = name;
                objects[i].position.x = posX;
                objects[i].position.y = posY;
                objects[i].position.z = posZ;
                objects[i].data = data;
                objects[i].data2 = data2;
                objects[i].data3 = data3;
                objects[i].distanceFromMe = distance;

                if (Program.debugMessages)  Console.WriteLine(i + "\t : " + curOffset.ToString("X") + " guid: " + guid + " : " + data.ToString("X") + " : " + data2.ToString("X") + " : " + data3.ToString("X") + " \t x:" + posX + " y:" + posY + " z:" + posZ + " \t" + name);

                curOffset = curOffset + Offsets.objmanagerStrucSize;
            }
            return objects;
        }

        public static Actor[] IterateActors()
        {
            if (Program.debugMessages)
            {
                Console.WriteLine("Iterating through Actors");
                Console.WriteLine("First Actor Location At: " + Offsets.itrObjectManagerD.ToString("X"));
            }

            uint curOffset = Offsets.itrObjectManagerD;
            uint count = getActorCount();
            Actor[] actors = new Actor[count];

            for (int i = 0; i < count; i++)
            {
                actors[i] = (Actor)mem.ReadMemory(curOffset, typeof(Actor));
                actors[i].mem_location = curOffset;
                curOffset = curOffset + Offsets.objmanagerStrucSize;
            }

            return actors;
        }

        public static ActorCommonData[] iterateACD()
        {
            if (Program.debugMessages)
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

        public static Actor[] getItems()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data1 == 2 && a.unknown_data2 == -1).ToArray<Actor>();
                //.Where(a => a.unknown_data1 == 2 && a.unknown_data2 == -1).OrderBy(a => a.distanceFromMe);
        }

        public static Actor[] getMonsters()
        {
            return IterateActors().OrderBy(a => a.distanceFromMe).Where(a => a.unknown_data2 == 29944).ToArray<Actor>();
            //.Where(a => a.unknown_data1 == 2 && a.unknown_data2 == -1).OrderBy(a => a.distanceFromMe);
        }


    }
}
