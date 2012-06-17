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
        uint id_acd;                // 0x000 
        fixed char name[128];       // 0x004
        uint unk_0;                 // 0x084 
        uint id_unk1;               // 0x088 
        uint id_unk2;               // 0x08C 
        uint id_snow;               // 0x090 
        fixed byte unknown_94[32];  // 0x094 
        uint id_acd_gBall;          // 0x0B4  
        fixed byte unknown_B8[24];  // 0x0B8 
        Data.Vec3 PosWorld;         // 0x0D0 
        fixed byte unknown_DC[36];  // 0x0DC 
        float RadiusDefault;        // 0x100 
        fixed byte unknown_104[4];  // 0x104 
        uint id_world;              // 0x108 
        fixed byte unknown_10C[4];  // 0x10C 
        uint id_owner;              // 0x110 
        fixed byte unknown_114[12]; // 0x114 
        uint id_attrib;             // 0x120 
        uint id_unk3;               // 0x124 
        fixed byte unknown_128[244];// 0x128 
        byte NBAD848;               // 0x21C 
        byte RadiusType;            // 0x21D 
        byte NBB56E9;               // 0x21E 
        byte NBBA038;               // 0x21F 
        fixed byte unknown_220[24]; // 0x220 
        float RadiusScaled;         // 0x238 
        fixed byte  unknown_23C[148];// 0x23C 
    }

}
