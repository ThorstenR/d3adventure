using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities.MemoryHandling;

namespace D3_Adventures
{
    public static class Actions
    {
        private static ReadWriteMemory mem = Program.mem;

        // checks if you want to use possibly dangerous functions
        //  and throws an error if you haven't set the screwWarden Boolean to true
        private static void wardenCheck()
        {
            if (!Program.screwWarden)
                throw new Exception("This action may be dangerous and cause warden to detect and ban you, if you want to use this function set screwWarden to true in Data.cs");
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

            movementTimer.Elapsed += new System.Timers.ElapsedEventHandler(movementTimer_Elapsed);
            movementTimer.Enabled = true;
            movementTimer.Start();
        }

        static void movementTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Data.Vec3 pos = Data.getCurrentPos();
            double distance = Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
            if (distance < 2 || mem.ReadMemoryAsFloat(Offsets.clickToMoveToggle) == 0)
            {
                movementTimer.Enabled = false;
                movementTimer.Stop();
            }
        }

        private static System.Timers.Timer interactTimer = new System.Timers.Timer(10);

        // NEEDS TO BE THREADED! or timer'ed ; )
        //  timered for now until someone changes it, or sees how it works first 
        public static void interactGUID(uint guid, uint snoPower)
        {
            wardenCheck();

            Data.Vec3 pos = Data.getCurrentPos();
            
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetUNK1, 0x777C);
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetUNK2, 0x1);
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetUNK3, (int)snoPower);
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetUNK4, (int)snoPower);
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetMousestate, 0x1);
            mem.WriteMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetGUID, (int)guid);

            mem.WriteMemoryAsFloat(Offsets.clickToMoveToX, pos.x + 1);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToY, pos.y);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToZ, pos.z);
            mem.WriteMemoryAsInt(Offsets.clickToMoveToggle, 1);
            mem.WriteMemoryAsInt(Offsets.clickToMoveFix, 69736);

            interactTimer.Elapsed += new System.Timers.ElapsedEventHandler(interactTimer_Elapsed);
            interactTimer.Enabled = true;
            interactTimer.Start();
        }

        static void interactTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int temp = mem.ReadMemoryAsInt(Offsets.itrInteractE + Offsets.interactOffsetUNK2);
            if (temp != 1)
            {
                interactTimer.Enabled = false;
                interactTimer.Stop();
            }
        }
    }
}
