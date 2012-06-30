 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3_Adventures.Memory_Handling;

namespace D3_Adventures
{
    public static class Offsets
    {
        private static MemoryManager mem = Globals.mem;
        public static uint uielements
        {
            get
            {
                return mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(objectManager) + 0x924)) + 0x08);
            }
        }
        public static uint UIelementCount
        {
            get
            {
                return mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(objectManager) + 0x924)) + 0x40);
            }
        }
        #region Object Manager
        public static uint objectManager = 0x15A1BEC; // 1.0.3.100235
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
        #endregion
        public static uint myToon
        {
            get
            {
                if (Globals.debugMessages) Console.WriteLine("Looking for local player");
                uint curOffset = itrObjectManagerD;
                uint count = Data.GetActorCount();

                for (int i = 0; i < count; i++)
                {
                    uint guid = mem.ReadMemoryAsUint(curOffset + 0x4);
                    string name = mem.ReadMemoryAsString(curOffset + 0x8, 64);
                    if (guid == Data.toonID) // your toon's guid
                    {
                        if (Globals.debugMessages) Console.WriteLine("My toon located at: " + curOffset.ToString("X") + " GUID: " + guid.ToString("X") + " Name: " + name);
                        return curOffset;
                    }
                    curOffset = curOffset + objmanagerStrucSize;
                }
                return uint.MaxValue; // 0xffffffff for a no find
            }
        }

        #region Interaction
        public static uint interact = 0x15A1BD4; // 1.0.3.100235
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
        public static uint currentX = 0xA8;
        public static uint currentY = 0xAC;
        public static uint currentZ = 0xB0;
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
        #endregion

        #region Actor Common Data 
        // http://www.ownedcore.com/forums/diablo-3/diablo-3-bots-programs/diablo-3-memory-editing/356250-autoit-diablo-3-click-move-interaction-actor-indexing.html#post2311789
        public static uint ACDBase = 0x15A1BEC; // 1.0.3.100235
        public static uint ACDOffset1 = 0x850;
        public static uint ACDOffset2 = 0;
        public static uint ACDOffset3 = 0x11C;
        public static uint ACDOffset4 = 0x148;
        public static uint ACDOffset5 = 0;
        public static uint ACDCount = mem.ReadMemory(ACDBase, new uint[] { ACDOffset1, ACDOffset2, ACDOffset3 });
        public static uint firstACD = mem.ReadMemory(ACDBase, new uint[] { ACDOffset1, ACDOffset2, ACDOffset4, ACDOffset5 });
        public static uint ACDSize = 0x2D0;
        #endregion

        #region SNOs
        public static class Sno
        {
            static public IntPtr Actor { get { return (IntPtr)0x15ED108; } }
            static public IntPtr Worlds { get { return (IntPtr)0x15EEAD8; } }
            static public IntPtr Monster { get { return (IntPtr)0x15DCE00; } }
            static public IntPtr Act { get { return (IntPtr)0x15DBF20; } }
            static public IntPtr Power { get { return (IntPtr)0x15E8E20; } }
            static public IntPtr Scene { get { return (IntPtr)0x15F3190; } }
            static public IntPtr MarkerSet { get { return (IntPtr)0x15E8788; } }
            static public IntPtr Quest { get { return (IntPtr)0x15E0C00; } }
            static public IntPtr SkillKit { get { return (IntPtr)0x15DFDA8; } }
            static public IntPtr StringList { get { return (IntPtr)0x15E9808; } }
            static public IntPtr GameBalance { get { return (IntPtr)0x15A7008; } }
        }


        #endregion
        #region functions
        public static uint fnLocalPlayerGUID = 0x97EC20;
        public static uint fnUsePowerToActor = 0x97C9B0;
        public static uint fnUsePowerToLocation = 0x97C770;
        public static uint fnGetSnoInfoForSnoId = 0x8A7500; // 1.0.3.100235
        public static uint gnGetDisplayedNameForSnoId = 0x826B10; // 1.0.3.100235
        #endregion


        // UI elements pointer table is : 0x19AA2000
    }
}
