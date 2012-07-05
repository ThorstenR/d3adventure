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
                return mem.ReadMemory(objectManager, new uint[] { 0x924, 0x0, 0x08 }); // UNTESTED
                // return mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(objectManager) + 0x924)) + 0x08);
            }
        }
        public static uint UIelementCount
        {
            get
            {
                return mem.ReadMemory(objectManager, new uint[] { 0x924, 0x0, 0x40 }); // UNTESTED
                //return mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(mem.ReadMemoryAsUint(objectManager) + 0x924)) + 0x40);
            }
        }

        #region Object Manager
        public static uint objectManager = 0x15A1BEC; // 1.0.3.100235
        public static uint objmanagerActorOffsetA = 0x8b0;
        public static uint objmanagerActorCount = 0x108;
        public static uint objmanagerActorOffsetB = 0x148;
        public static uint objmanagerActorLinkToCTM = 0x380;
        public static uint objmanagerStrucSize = 0x428;

        public static uint itrObjectManagerA { get { return mem.ReadMemoryAsUint(objectManager); } }
        public static uint itrObjectManagerB { get { return mem.ReadMemoryAsUint(itrObjectManagerA + objmanagerActorOffsetA); } }

        public static uint itrObjectManagerCount { get { return  itrObjectManagerB + objmanagerActorCount; } }
        public static uint itrObjectManagerC { get { return  mem.ReadMemoryAsUint(itrObjectManagerB + objmanagerActorOffsetB); } }

        public static uint itrObjectManagerD { get { return  mem.ReadMemoryAsUint(itrObjectManagerC); } }
        public static uint itrObjectManagerE { get { return mem.ReadMemoryAsUint(itrObjectManagerD); } }
        #endregion

        #region Attributes
        //GET ACTORATRIB
        public static uint ofs_ActorAtrib_Base = 0x15A2EA4;//0x015A1EA4
        public static uint ofs_ActorAtrib_ofs1 = 0x390;
        public static uint ofs_ActorAtrib_ofs2 = 0x2E8;
        public static uint ofs_ActorAtrib_ofs3 = 0x148;
        public static uint ofs_ActorAtrib_Count = 0x108; // 0x0 0x0

        public static uint ofs_ActorAtrib_Indexing_ofs1 = 0x10;
        public static uint ofs_ActorAtrib_Indexing_ofs2 = 0x8;
        public static uint ofs_ActorAtrib_Indexing_ofs3 = 0x250;
        public static uint ofs_ActorAtrib_StrucSize = 0x180;

        public static uint ofs_LocalPlayer_HPBARB = 0x34;
        public static uint ofs_LocalPlayer_HPWIZ = 0x38;

// !REFRACTOR THIS TO USE THE OFFSET ARRAY MEMORY READER INSTEAD OF THIS CHUNKY CODE (after finished porting)
        public static uint ActorAtrib_Base = mem.ReadMemoryAsUint(ofs_ActorAtrib_Base);
        public static uint ActorAtrib_1 = mem.ReadMemoryAsUint(ActorAtrib_Base + ofs_ActorAtrib_ofs1);
        public static uint ActorAtrib_2 = mem.ReadMemoryAsUint(ActorAtrib_1 + ofs_ActorAtrib_ofs2);
        public static uint ActorAtrib_3 = mem.ReadMemoryAsUint(ActorAtrib_2 + ofs_ActorAtrib_ofs3);
        public static uint ActorAtrib_4 = mem.ReadMemoryAsUint(ActorAtrib_3);
        public static uint ActorAtrib_Count = ActorAtrib_2 + ofs_ActorAtrib_Count;

        #endregion

        #region Local Actor

        //GET LOCAL ACTOR STRUC
        public static uint ofs_LocalActor_ofs1 = 0x378; //instead of ofs_ActorAtrib_ofs2
        public static uint ofs_LocalActor_ofs2 = 0x148;
        public static uint ofs_LocalActor_Count = 0x108;
        public static uint ofs_LocalActor_atribGUID = 0x120;
        public static uint ofs_LocalActor_StrucSize = 0x2D0; // 0x0 0x0

        // !REFRACTOR THIS TO USE THE OFFSET ARRAY MEMORY READER INSTEAD OF THIS CHUNKY CODE (after finished porting)
        public static uint LocalActor_1 = mem.ReadMemoryAsUint(ActorAtrib_1 + ofs_LocalActor_ofs1);
        public static uint LocalActor_2 = mem.ReadMemoryAsUint(LocalActor_1 + ofs_LocalActor_ofs2);
        public static uint LocalActor_3 = mem.ReadMemoryAsUint(LocalActor_2);
        public static uint LocalActor_Count = mem.ReadMemory(ofs_ActorAtrib_Base, new uint[] { ofs_ActorAtrib_ofs1, ofs_LocalActor_ofs1, ofs_ActorAtrib_Count });//LocalActor_1 + ofs_LocalActor_Count;

        #endregion

        /// <summary>
        /// Returns your player's GUID
        /// </summary>
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
        public static uint moveToXoffset = 0x40;
        public static uint moveToYoffset = 0x44;
        public static uint moveToZoffset = 0x48;
        public static uint currentX = 0xA8;
        public static uint currentY = 0xAC;
        public static uint currentZ = 0xB0;
        public static uint rotationOffset = 0x174;

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

        // thanks to Jens 
        public enum Sno : uint
        {
            Actor = 0x015ED108,
            Adventure = 0x015E7EB0,
            AmbientSound = 0x016554C0,
            Anim = 0x015EA148,
            Anim2D = 0x0164F880,
            AnimSet = 0x015ECC98,
            Appearance = 0x0163AD50,
            Hero = 0x015CDC58,
            Cloth = 0x01653330,
            Conversation = 0x015E6608,
            ConversationList = 0x015E5C20,
            EffectGroup = 0x01653BF8,
            Encounter = 0x015E8290,
            Explosion = 0x01654670,
            FlagSet = 0x015E5840,
            Font = 0x0164FE90,
            GameBalance = 0x015A7008,
            Globals = 0x015F5BD0,
            LevelArea = 0x015E3F88,
            Light = 0x0164A518,
            MarkerSet = 0x015E8788,
            Monster = 0x015DCE00,
            Observer = 0x015F5390,
            Particle = 0x016505B8,
            Physics = 0x01654C80,
            Power = 0x015E8E20,
            Quest = 0x015E0C00,
            Rope = 0x01644D60,
            Scene = 0x015F3190,
            SceneGroup = 0x015E5348,
            ShaderMap = 0x01655F30,
            Shaders = 0x0164DE38,
            Shakes = 0x01642E20,
            SkillKit = 0x015DFDA8,
            Sound = 0x01647220,
            SoundBank = 0x01649B30,
            StringList = 0x015E9808,
            Surface = 0x01654320,
            Textures = 0x01656168,
            Trail = 0x01652AF0,
            UI = 0x0164B128,
            Weather = 0x016435D0,
            Worlds = 0x015EEAD8,
            Recipe = 0x015DCCE0,
            Condition = 0x015D9EB0,
            TreasureClass = 0x015D90F8,
            Account = 0x015D0098,
            TimedEvent = 0x015DCBC0,
            Act = 0x015DBF20,
            Material = 0x01642B58,
            QuestRange = 0x015E08A8,
            Lore = 0x015DBB40,
            Reverb = 0x01641DA0,
            PhysMesh = 0x015FA770,
            Music = 0x016462C0,
            Tutorial = 0x015D8450,
            BossEncounter = 0x015D88B8
        }

        #endregion

        #region Functions
        public static uint fnLocalPlayerGUID = 0x97EC20;
        public static uint fnUsePowerToActor = 0x97C9B0;
        public static uint fnUsePowerToLocation = 0x97C770;
        public static uint fnGetSnoInfoForSnoId = 0x8A7500; // 1.0.3.100235
        public static uint gnGetDisplayedNameForSnoId = 0x826B10; // 1.0.3.100235
        #endregion

    }
}
