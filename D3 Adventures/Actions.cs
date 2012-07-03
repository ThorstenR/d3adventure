using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Enumerations;
using D3_Adventures.Structures;
using D3_Adventures.Memory_Handling;

namespace D3_Adventures
{
    public static class Actions
    {
        private static MemoryManager mem = Globals.mem;

        public static System.Timers.Timer movementTimer = new System.Timers.Timer(10);
        private static int nearDistance;

        /*;;================================================================================
        ; Function:			MoveToPos($_x,$_y,$_z[,$neardist = 2])
        ; Description:		Move to a desired position.
        ; Parameter(s):		$_x,$_y and $_z - the target position
        ;					$neardist - the distance from the target position
        ;								that the move function will stop
        ;								 
        ; Note(s):			You can use $neardist to make the movement more fluid.
        ;					If the function stop before the desired position
        ;						is reached it won't stop between each point,
        ;						this is why the default $neardist is 2
        ;==================================================================================*/
        // NEEDS TO BE THREADED! or timer'ed ; )
        //  timered for now until someone changes it, or sees how it works first 
        public static void MoveToPos(float x, float y, float z, int nearDistance = 2)
        {
            Actions.nearDistance = nearDistance;

            mem.WriteMemoryAsFloat(Offsets.clickToMoveToX, x);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToY, y);
            mem.WriteMemoryAsFloat(Offsets.clickToMoveToZ, z);
            mem.WriteMemoryAsInt(Offsets.clickToMoveToggle, 1);
            mem.WriteMemoryAsInt(Offsets.clickToMoveFix, 69736);

            movementTimer.Elapsed += new System.Timers.ElapsedEventHandler(movementTimer_Elapsed);
            movementTimer.Enabled = true;
            movementTimer.Start();
        }

        private static void movementTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Vec3 pos = Data.GetCurrentPos();
            double distance = Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
            if (distance < nearDistance || mem.ReadMemoryAsFloat(Offsets.clickToMoveToggle) == 0)
            {
                movementTimer.Enabled = false;
                movementTimer.Stop();
            }
        }

        public static System.Timers.Timer interactTimer = new System.Timers.Timer(10);

        /*;;================================================================================
        ; Function:			PowerUseGUID($_guid,$_snoPower)
        ; Description:		Use a Power on a GUID
        ; Parameter(s):		$_guid - GUID you want to use the power on.
        ;					$_snoPower - the ID of the power you want to use
        ;								 
        ; Note(s):			To interact with NPC's use $_snoPower = 7546
        ;					To Pick up loot use $_snoPower = 7545
        ;					To cast a townportal use $_snoPower = 02EC66 and your GUID
        ;==================================================================================*/
        // NEEDS TO BE THREADED! or timer'ed ; )
        //  timered for now until someone changes it, or sees how it works first 
        public static void PowerUseGUID(uint guid, uint snoPower)
        {
            Vec3 pos = Data.GetCurrentPos();
            
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

        public static void PowerUseGUID(uint guid, SNO.SNOPowerId snoPower)
        {
            PowerUseGUID(guid, (uint)snoPower);
        }

        private static void interactTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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
