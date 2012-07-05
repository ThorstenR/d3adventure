using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace D3_Adventures.Structures
{

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 0x428)] //Offsets.objmanagerStrucSize)
    unsafe public struct Actor
    {
        public uint id_actor;     // 0x000 
        public uint id_acd;     // 0x004 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        private char[] _name;     // 0x008               //The name the actor, not the name that are shown ingame, most likely model name
        public uint id_sno;     // 0x088                //Actor type, every mob type have it's own ID
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] unknown_8C;// 0x08C 
        public Vec3 Rot;     // 0x090 
        public float fUnk_Rot;     // 0x09C 
        public Vec3 Pos;     // 0x0A0 
        public float fUnk_130;     // 0x0AC 
        public Vec3 Pos1;                       // 0x0B0 
        public float fUnk_734;                  // 0x0BC 
        public Vec3 Pos2;                       // 0x0C0 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] unknown_CC;// 0x0CC 
        public float RadiusDefault;             // 0x0D0 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] unknown_D4; // 0x0D4 
        public uint guid_world;                 // 0x0D8 
        public uint guid_Unk;                   // 0x0DC
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] unknown_E0; // 0x0E0
        public Vec3 Pos3;                       // 0x100
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] unknown_10C;// 0x10C
        public uint FAG; // 0x120                               // FastAttribGroups
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public byte[] unknown_124;// 0x124
        public Vec3 Pos4;                       // 0x140
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] unknown_14C; // 0x14C
        public uint N13E1EB50;                  // 0x15C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        public byte[] unknown_160; // 0x160
        public uint unknown_data3;              // 0x1C0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] unknown_1C4; // 0x1C4
        public uint unknown_data2; // 0x1CC
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
        public byte[] unknown_1D0; // 0x1D0
        public uint unknown_data1;// 0x1FC
        public Vec3 PosObj;     // 0x210 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 356)]
        public byte[] unknown_21C; // 0x21C 
        public ActorMovement* Mov;     // 0x380 
        public float Direction;     // 0x384 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] unknown_388; // 0x388 
        public Vec3 Vel;     // 0x3A0 
        public Vec3 Pos6;     // 0x3AC 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
        public byte[] unknown_3B8;
		public long Alive; // 0x3B8 
        public uint Frame;     // 0x418 
        public uint Diff;     // 0x41C 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] unknown_420; // 0x420
        // END OF MEM STRUCT

        public uint mem_location; // used for checking for changes later on

        public string name
        {
            get
            {
                return new string(_name).TrimEnd(new char[] { (char)0 });
            }
        }

        public bool IsAlive()
        {

            if (this.Alive == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public double distanceFromMe
        {
            get
            {
                Vec3 currentLoc = Data.GetCurrentPos();
                return GameUtilities.Distance(currentLoc, Pos1);
            }
        }
        //using the Actor struct Alive, will indicate if the actor or atlest monster is alive... 
        public bool Exists()
        {
            if (mem_location == null)
                throw new Exception("Memory Location of The Actor Must Be Set Before isAlive Can Be Called.");
            return (id_acd == Globals.mem.ReadMemoryAsUint(mem_location + 0x4)); // maybe just check the id_actor and base address
        }

        /// <summary>
        /// Checks to see if the object at the memory location is still the same.
        /// </summary>
        /// <returns>Returns true if the GUID/id_actor has not changed.</returns>
        public bool IsValid()
        {
            return (Globals.mem.ReadMemoryAsUint(mem_location) == id_actor);
        }

        public string ToString()
        {
            FieldInfo[] fis = typeof(Actor).GetFields();
            
            StringBuilder sb = new StringBuilder();

            foreach (FieldInfo fi in fis)
            {
                if (fi.FieldType.Name == "UInt32")
                    sb.Append(" [" + fi.Name + " = 0x" + ((uint)fi.GetValue(this)).ToString("X") + "] ");
                else
                    sb.Append(" [" + fi.Name + " = " + fi.GetValue(this) + "] ");
            }
            return sb.ToString(); ;
        }

        public Dictionary<string, string> Fields()
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            FieldInfo[] fis = typeof(Actor).GetFields();

            foreach (FieldInfo fi in fis)
            {
                if (fi.FieldType.Name == "UInt32")
                    fields.Add(fi.Name, "0x" + ((uint)fi.GetValue(this)).ToString("X"));
                else
                    fields.Add(fi.Name, fi.GetValue(this).ToString());
            }

            return fields;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 0x428)] //Offsets.objmanagerStrucSize)
    unsafe public struct ActorMovement
    {
        public uint* vt;     // 0x000 
        public uint Active;     // 0x004 
        public float Speed;     // 0x008 
        public float SpeedCurrent;     // 0x00C 
        public fixed byte unknown_10[12];     // 0x010 
        public float ScaleSize;     // 0x01C 
        public uint Flags;     // 0x020 
        public fixed byte unknown_24[16];     // 0x024 
        public uint IsMoving;     // 0x034 
        public uint PathIsWalkable;     // 0x038 
        public Vec3 MovTo;     // 0x03C 
        public fixed byte unknown_48[4];     // 0x048 
        public Vec3 Pos2;     // 0x04C 
        public fixed byte unknown_58[28];     // 0x058 
        public Vec3 Tp;     // 0x074 
        public fixed byte unknown_80[8];     // 0x080 
        public uint unk;     // 0x088 
        public fixed byte unknown_8C[24];     // 0x08C 
        public Vec3 Pos3;     // 0x0A4 
        public fixed byte unknown_B0[8];     // 0x0B0 
        public float Speed2;     // 0x0B8 
        public fixed byte unknown_BC[160];     // 0x0BC 
        public uint id_actor;     // 0x15C 
        public uint FrameMov;     // 0x160 
        public uint Frame;     // 0x164 
        public uint FramePrev;     // 0x168 
        public fixed byte unknown_16C[4];     // 0x16C 
        public float Direction;     // 0x170 
        public fixed byte unknown_174[707];     // 0x174 
    }; 

}
