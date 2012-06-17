using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace D3_Adventures.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 0x428)] //Offsets.objmanagerStrucSize)
    unsafe public struct Actor
    {
        uint id_actor;     // 0x000 
        uint id_acd;     // 0x004 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        char[] _Name;     // 0x008 
        uint id_sno;     // 0x088 
        fixed byte unknown_8C[4];     // 0x08C 
        Data.Vec3 Rot;     // 0x090 
        float fUnk_Rot;     // 0x09C 
        Data.Vec3 Pos;     // 0x0A0 
        float fUnk_130;     // 0x0AC 
        Data.Vec3 Pos1;     // 0x0B0 
        float fUnk_734;     // 0x0BC 
        Data.Vec3 Pos2;     // 0x0C0 
        fixed byte unknown_CC[4];     // 0x0CC 
        float RadiusDefault;     // 0x0D0 
        fixed byte unknown_D4[4];     // 0x0D4 
        uint guid_world;     // 0x0D8 
        uint guid_Unk;     // 0x0DC 
        fixed byte unknown_E0[32];     // 0x0E0 
        Data.Vec3 Pos3;     // 0x100 
        fixed byte unknown_10C[52];     // 0x10C 
        Data.Vec3 Pos4;     // 0x140 
        fixed byte unknown_14C[16];     // 0x14C 
        uint N13E1EB50;     // 0x15C 
        fixed byte unknown_160[176];     // 0x160 
        Data.Vec3 PosObj;     // 0x210 
        fixed byte unknown_21C[356];     // 0x21C 
        ActorMovement* Mov;     // 0x380 
        float Direction;     // 0x384 
        fixed byte unknown_388[24];     // 0x388 
        Data.Vec3 Vel;     // 0x3A0 
        Data.Vec3 Pos6;     // 0x3AC 
        fixed byte unknown_3B8[96];     // 0x3B8 
        uint Frame;     // 0x418 
        uint Diff;     // 0x41C 
        fixed byte unknown_420[8];     // 0x420 
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 0x428)] //Offsets.objmanagerStrucSize)
    unsafe public struct ActorMovement
    { 
        uint* vt;     // 0x000 
        uint Active;     // 0x004 
        float Speed;     // 0x008 
        float SpeedCurrent;     // 0x00C 
        fixed byte unknown_10[12];     // 0x010 
        float ScaleSize;     // 0x01C 
        uint Flags;     // 0x020 
        fixed byte unknown_24[16];     // 0x024 
        uint IsMoving;     // 0x034 
        uint PathIsWalkable;     // 0x038 
        Data.Vec3 MovTo;     // 0x03C 
        fixed byte unknown_48[4];     // 0x048 
        Data.Vec3 Pos2;     // 0x04C 
        fixed byte unknown_58[28];     // 0x058 
        Data.Vec3 Tp;     // 0x074 
        fixed byte unknown_80[8];     // 0x080 
        uint unk;     // 0x088 
        fixed byte unknown_8C[24];     // 0x08C 
        Data.Vec3 Pos3;     // 0x0A4 
        fixed byte unknown_B0[8];     // 0x0B0 
        float Speed2;     // 0x0B8 
        fixed byte unknown_BC[160];     // 0x0BC 
        uint id_actor;     // 0x15C 
        uint FrameMov;     // 0x160 
        uint Frame;     // 0x164 
        uint FramePrev;     // 0x168 
        fixed byte unknown_16C[4];     // 0x16C 
        float Direction;     // 0x170 
        fixed byte unknown_174[707];     // 0x174 
    }; 
}
