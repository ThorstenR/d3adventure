using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace D3_Adventures.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 0x2D0)] //Offsets.ACDSize)
    unsafe public struct ActorCommonData
    {
        public uint id_acd;                // 0x000
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] _name;       // 0x004
        public uint unk_0;                 // 0x084 
        public uint id_unk1;               // 0x088 
        public uint id_unk2;               // 0x08C 
        public uint id_snow;               // 0x090 
        public fixed byte unknown_94[32];  // 0x094 
        public uint id_acd_gBall;          // 0x0B4  
        public fixed byte unknown_B8[24];  // 0x0B8 
        public Vec3 PosWorld;         // 0x0D0 
        public fixed byte unknown_DC[36];  // 0x0DC 
        public float RadiusDefault;        // 0x100 
        public fixed byte unknown_104[4];  // 0x104 
        public uint id_world;              // 0x108 
        public fixed byte unknown_10C[4];  // 0x10C 
        public uint id_owner;              // 0x110 
        public fixed byte unknown_114[12]; // 0x114 
        public uint id_attrib;             // 0x120 
        public uint id_unk3;               // 0x124 
        public fixed byte unknown_128[244];// 0x128 
        public byte NBAD848;               // 0x21C 
        public byte RadiusType;            // 0x21D 
        public byte NBB56E9;               // 0x21E 
        public byte NBBA038;               // 0x21F 
        public fixed byte unknown_220[24]; // 0x220 
        public float RadiusScaled;         // 0x238 
        public fixed byte unknown_23C[148];// 0x23C 

        public string name
        {
            get
            {
                return new string(_name).TrimEnd(new char[] { (char)0 });
            }
        }
    }

}
