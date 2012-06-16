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

        private static uint fixSpeed = 0x20; // 69736
        private static uint toggleMove = 0x34;
        private static uint moveToXoffset = 0x3c;
        private static uint moveToYoffset = 0x40;
        private static uint moveToZoffset = 0x44;
        private static uint currentX = 0xA4;
        private static uint currentY = 0xA8;
        private static uint currentZ = 0xAC;
        private static uint rotationOffset = 0x170;

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
