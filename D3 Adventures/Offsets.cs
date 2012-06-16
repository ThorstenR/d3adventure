using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utilities;
using Utilities.MemoryHandling;

namespace D3_Adventures
{
    public static class Offsets
    {
        private static ReadWriteMemory mem = Program.mem;

        public static uint objectManager = 0x01580A2C;
        public static uint objmanagerActorOffsetA = 0x8b0;
        public static uint objmanagerActorCount = 0x108;
        public static uint objmanagerActorOffsetB = 0x148;
        public static uint objmanagerActorLinkToCTM = 0x380;
        public static uint objmanagerStrucSize = 0x428;

        public static uint itrObjectManagerA = mem.ReadMemoryAsUint(objectManager);
        public static uint itrObjectManagerB = mem.ReadMemoryAsUint(itrObjectManagerA + objmanagerActorOffsetA);

        public static uint itrObjectManagerCount = itrObjectManagerB + objmanagerActorCount;
        public static uint itrObjectManagerC = mem.ReadMemoryAsUint(itrObjectManagerB + objmanagerActorOffsetB);

        public static uint itrObjectManagerD = mem.ReadMemoryAsUint(itrObjectManagerC);
        public static uint itrObjectManagerE = mem.ReadMemoryAsUint(itrObjectManagerD);

        public static uint myToon
        {
            get
            {
                if (Program.debugMessages)  Console.WriteLine("Looking for local player");
                uint curOffset = itrObjectManagerD;
                uint count = Data.getActorCount();

                for (int i = 0; i < count; i++)
                {
                    uint guid = mem.ReadMemoryAsUint(curOffset + 0x4);
                    string name = mem.ReadMemoryAsString(curOffset + 0x8, 64);
                    if (guid == 0x77BC0000)
                    {
                        if (Program.debugMessages) Console.WriteLine("My toon located at: " + curOffset.ToString("X") + " GUID: " + guid.ToString("X") + " Name: " + name);
                        return curOffset;
                    }
                    curOffset = curOffset + objmanagerStrucSize;
                }
                return uint.MaxValue; // 0xffffffff for a no find
            }
        }

        public static uint interact = 0x01580A14;
        public static uint interactOffsetA = 0xA8;
        public static uint interactOffsetB = 0x58;
        public static uint interactOffsetUNK1 = 0x7F20; // set to 777C
        public static uint interactOffsetUNK2 = 0x7F44; // set to 1 for NPC interaction
        public static uint interactOffsetUNK3 = 0x7F7C; // set to 7546 for NPC interaction, 7545 for loot interaction
        public static uint interactOffsetUNK4 = 0x7F80; // set to 7546 for NPC interaction, 7545 for loot interaction
        public static uint interactOffsetMousestate = 0x7F84; // mouse state 1 = clicked, 2 = mouse down
        public static uint interactOffsetGUID = 0x7F88; // set to the GUID of the actor you want to interact with

        public static uint itrInteractA = mem.ReadMemoryAsUint(interact);
        public static uint itrInteractB = mem.ReadMemoryAsUint(itrInteractA);
        public static uint itrInteractC = mem.ReadMemoryAsUint(itrInteractB);
        public static uint itrInteractD = mem.ReadMemoryAsUint(itrInteractC + interactOffsetA);
        public static uint itrInteractE = itrInteractD + interactOffsetB;

        public static uint fixSpeed = 0x20; // 69736
        public static uint toggleMove = 0x34;
        public static uint moveToXoffset = 0x3c;
        public static uint moveToYoffset = 0x40;
        public static uint moveToZoffset = 0x44;
        public static uint currentX = 0xA4;
        public static uint currentY = 0xA8;
        public static uint currentZ = 0xAC;
        public static uint rotationOffset = 0x170;

        public static uint clickToMoveMain = mem.ReadMemoryAsUint(myToon + objmanagerActorLinkToCTM);
        public static uint clickToMoveRotation = clickToMoveMain + rotationOffset;
        public static uint clickToMoveCurX = clickToMoveMain + currentX;
        public static uint clickToMoveCurY = clickToMoveMain + currentY;
        public static uint clickToMoveCurZ = clickToMoveMain + currentZ;
        public static uint clickToMoveToX = clickToMoveMain + moveToXoffset;
        public static uint clickToMoveToY = clickToMoveMain + moveToYoffset;
        public static uint clickToMoveToZ = clickToMoveMain + moveToZoffset;
        public static uint clickToMoveToggle = clickToMoveMain + toggleMove;
        public static uint clickToMoveFix = clickToMoveMain + fixSpeed;
    }
}
