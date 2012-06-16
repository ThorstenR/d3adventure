using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities.MemoryHandling;

namespace D3_Adventures
{
    public static class Data
    {
        private static ReadWriteMemory mem = Program.mem;
        private static bool screwWarden = false; // turn to true to use things that use memory writing

        public struct Vec2  { 
            public float x;    // 0x000 
            public float y;    // 0x004 
        };

        public struct Vec3  {
            public float x;    // 0x000 
            public float y;    // 0x004 
            public float z;    // 0x008 
        };

        public struct Vec4
        {
            public float x;    // 0x000 
            public float y;    // 0x004 
            public float z;    // 0x008 
            public float w;    // 0x00C 
        };

        // checks if you want to use possibly dangerous functions
        //  and throws an error if you haven't set the screwWarden Boolean to true
        private static void wardenCheck()
        {
            if (!screwWarden)
                throw new Exception("This action may be dangerous and cause warden to detect and ban you, if you want to use this function set screwWarden to true in Data.cs");
        }

        public static uint getActorCount()
        {
            uint count = mem.ReadMemoryAsUint(Offsets.itrObjectManagerCount);
            if (Program.debugMessages) Console.WriteLine("Number of Actors: " + count);
            return count;
        }

        public static Vec3 getCurrentPos()
        {
            Vec3 ret;
            ret.x = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurX);
            ret.y = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurY); ;
            ret.z = mem.ReadMemoryAsFloat(Offsets.clickToMoveCurZ); ;
            return ret;
        }

        private static System.Timers.Timer movementTimer = new System.Timers.Timer(10);

        // NEEDS TO BE THREADED! or timer'ed ; )
        //  timered for now until someone changes it, or sees how it works first 
        public static void moveToPos(float x, float y, float z)
        {
            wardenCheck();
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToX, x);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToY, y);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToZ, z);
            mem.WriteMemoryAsInt(Offsets.clickToMoveToggle, 1);
            mem.WriteMemoryAsInt(Offsets.clickToMoveFix, 69736);

            movementTimer.Elapsed +=new System.Timers.ElapsedEventHandler(movementTimer_Elapsed);
            movementTimer.Enabled=true;
        }

        static void movementTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Vec3 pos = getCurrentPos();
            double distance = Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
            if (distance < 2) movementTimer.Enabled = false;
            if (mem.ReadMemoryAsFloat(Offsets.clickToMoveToggle) == 0) movementTimer.Enabled = false;
        }

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
    }
}
