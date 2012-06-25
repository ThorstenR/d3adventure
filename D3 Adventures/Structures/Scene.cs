using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using D3_Adventures.Enumerations;

namespace D3_Adventures.Structures
{
    public static class SceneManager
    {
        public static Scene[] getScenes()
        {
            uint offset = Offsets.itrObjectManagerA + 0x8F4;
            uint sceneCountPtr = Globals.mem.ReadMemoryAsUint(offset) + 0x10c;
            uint sceneListPtr = Globals.mem.ReadMemoryAsUint(offset) + 0x148;
            sceneListPtr = Globals.mem.ReadMemoryAsUint(sceneListPtr);

            int numScenes = Globals.mem.ReadMemoryAsInt(sceneCountPtr);

            List<Scene> scenes = new List<Scene>();
            for (int i = 0; i < numScenes; i++)
            {
                Scene s = new Scene(new IntPtr(sceneListPtr));
                scenes.Add(s);

//                 using (new D3_Adventures.Injector.ASMExecutorMonitor(Globals.mem.Injector))
//                 {
//                     var executor = Globals.mem.Injector;
//                     var x = executor.CallFunction(0x8A30F0, CallingConvention.Cdecl, s.SceneId);
// 
//                     var sss = new Scene( new IntPtr(0x1F4BA004));
//                 }



                sceneListPtr += 0x2A8;
            }
            return scenes.ToArray();
        }
    }

    public class Scene
    {
        private IntPtr _baseAddress;

        public Scene(IntPtr address)
        {
            _baseAddress = address;
        }

        private Struct43 Native
        {
            get
            {
                return (Struct43)Globals.mem.ReadMemory(_baseAddress, typeof(Struct43));

            }
        }

        public int DynamicWorldId
        {
            get
            {
                return this.Native.struct40_0.int_1;
            }
        }

        public int EntranceExitWorldSNO
        {
            get
            {
                return this.Native.struct40_0.int_9;
            }
        }

        public int LevelAreaSNO
        {
            get
            {
                return this.Native.struct40_0.int_5;
            }
        }

        public AABB MarkerSetBounds
        {
            get
            {
                return this.Native.struct40_0.aabb_1;
            }
        }

        public Vec2 Position
        {
            get
            {
                return this.Native.struct40_0.vector2_0;
            }
        }

        public int SceneId
        {
            get
            {
                return this.Native.struct40_0.int_0;
            }
        }

        public int SceneSNO
        {
            get
            {
                return this.Native.struct40_0.int_36;
            }
        }

        public int SquareCountX
        {
            get
            {
                return this.Native.struct40_0.int_33;
            }
        }

        public int SquareCountY
        {
            get
            {
                return this.Native.struct40_0.int_34;
            }
        }

        private NavZone _zone = null;
        public NavZone Zone
        {
            get
            {
                if (_zone != null) return _zone;

                _zone = new NavZone(Native.struct40_0.intptr_0);
                return _zone;
            }
        }


        public SNORecordScene SceneInfo
        {
            get
            {
                try
                {
                    return Globals.SNO[SNO.ClientSNOTable.Scene].GetRecord<SNORecordScene>(this.SceneSNO);
                }
                catch (System.Exception ex)
                {
                    return null;
                }
            }
        }


        //internal Scene structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct Struct40
        {
            public int int_0;
            public int int_1;
            public int int_2;
            public int int_3;
            public int int_4;
            public int int_5;
            public int int_6;
            public int int_7;
            public int int_8;
            public int int_9;
            public int int_10;
            public int int_11;
            public int int_12;
            public int int_13;
            public int int_14;
            public int int_15;
            public int int_16;
            public int int_17;
            public int int_18;
            public int int_19;
            public float float_0;
            public int int_20;
            public int int_21;
            public int int_22;
            public int int_23;
            public int int_24;
            public float float_1;
            public int int_25;
            public int int_26;
            public int int_27;
            public int int_28;
            public int int_29;
            public int int_30;
            public int int_31;
            public int int_32;
            public int int_33;
            public int int_34;
            public Vec3 vector3_0;
            public Vec3 vector3_1;
            public Vec3 vector3_2;
            public Vec3 vector3_3;
            public uint uint_0;
            public uint uint_1;
            public uint uint_2;
            public uint uint_3;
            public int int_35;
            public int int_36;
            public int int_37;
            public uint uint_4;
            public uint uint_5;
            public uint uint_6;
            public Vec2 vector2_0;
            public uint uint_7;
            public uint uint_8;
            public uint uint_9;
            public uint uint_10;
            public uint uint_11;
            public uint uint_12;
            public uint uint_13;
            public uint uint_14;
            public uint uint_15;
            public uint uint_16;
            public uint uint_17;
            public uint uint_18;
            public uint uint_19;
            public uint uint_20;
            public int int_38;
            public int int_39;
            public AABB aabb_0;
            public AABB aabb_1;
            public float float_2;
            public Vec2 vector2_1;
            public float float_3;
            public float float_4;
            public int int_40;
            public IntPtr intptr_0;
            public int int_41;
            public int int_42;
            public int int_43;
            public int int_44;
            public int int_45;
            public int int_46;
            public int int_47;
            public int int_48;
            public int int_49;
            public int int_50;
            public int int_51;
            public int int_52;
            public int int_53;
            public uint uint_21;
            public uint uint_22;
            public uint uint_23;
            public uint uint_24;
            public int int_54;
        }


        [StructLayout(LayoutKind.Sequential, Size = 680)]
        private struct Struct43
        {
            public int int_0;
            public Scene.Struct40 struct40_0;
            public uint uint_0;
            public uint uint_1;
            public uint uint_2;
            public uint uint_3;
            public uint uint_4;
            public uint uint_5;
            public uint uint_6;
            public uint uint_7;
            public uint uint_8;
            public uint uint_9;
            public uint uint_10;
            public uint uint_11;
            public uint uint_12;
            public uint uint_13;
            public uint uint_14;
            public uint uint_15;
            public uint uint_16;
            public uint uint_17;
            public uint uint_18;
            public uint uint_19;
            public uint uint_20;
            public uint uint_21;
            public uint uint_22;
            public uint uint_23;
            public uint uint_24;
            public uint uint_25;
            public uint uint_26;
            public uint uint_27;
            public uint uint_28;
            public uint uint_29;
            public uint uint_30;
            public uint uint_31;
            public uint uint_32;
            public uint uint_33;
            public uint uint_34;
            public uint uint_35;
            public uint uint_36;
            public uint uint_37;
            public uint uint_38;
            public uint uint_39;
            public uint uint_40;
            public uint uint_41;
            public uint uint_42;
            public uint uint_43;
            public uint uint_44;
            public uint uint_45;
            public uint uint_46;
            public uint uint_47;
            public uint uint_48;
            public uint uint_49;
            public uint uint_50;
            public uint uint_51;
            public uint uint_52;
            public uint uint_53;
            public uint uint_54;
        }

    }

    public class NavZone
    {
        private IntPtr _baseAddress;

        public NavZone(IntPtr address)
        {
            _baseAddress = address;
        }

        private Struct42 Native
        {
            get
            {
                return (Struct42)Globals.mem.ReadMemory(_baseAddress, typeof(Struct42));
            }
        }

        private void resolve_NavZoneDef()
        {
            try
            {
                using (new D3_Adventures.Injector.ASMExecutorMonitor(Globals.mem.Injector))
                {
                    var executor = Globals.mem.Injector;
                    var x = executor.CallFunction(0x00923010, CallingConvention.ThisCall, (uint)_baseAddress.ToInt32());
                }
            }
            catch (Exception exception)
            {
            }
        }



        public NavZoneDef NavZoneDef
        {
            get
            {
                if (Native.intptr_0 == IntPtr.Zero)
                {
                    resolve_NavZoneDef();
                }
                if (Native.intptr_0 == IntPtr.Zero)
                    return null;

                return new NavZoneDef(this.Native.intptr_0);
            }
        }

        public Vec2 ZoneMax
        {
            get
            {
                return this.Native.vector2_2;
            }
        }

        public Vec2 ZoneMin
        {
            get
            {
                return this.Native.vector2_1;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct Struct42
        {
            public int int_0;
            public uint uint_0;
            public int int_1;
            public Vec2 vector2_0;
            public uint uint_1;
            public Vec2 vector2_1;
            public Vec2 vector2_2;
            public uint uint_2;
            public uint uint_3;
            public uint uint_4;
            public uint uint_5;
            public uint uint_6;
            public uint uint_7;
            public uint uint_8;
            public uint uint_9;
            public IntPtr intptr_0;
            public IntPtr intptr_1;
            public uint uint_10;
        }
    }

    public class NavZoneDef
    {
        private IntPtr _baseAddress;

        public NavZoneDef(IntPtr address)
        {
            _baseAddress = address;
        }

        private Struct41 Native
        {
            get
            {
                return (Struct41)Globals.mem.ReadMemory(_baseAddress, typeof(Struct41));
            }
        }

        public NavCell GetNavCellByIndex(int index)
        {
            if (((IntPtr)this.Native.int_1[1]) == IntPtr.Zero)
            {
                throw new NullReferenceException("NavCells pointer is invalid!");
            }
            if (index > this.NavCellCount)
            {
                throw new ArgumentOutOfRangeException("index", "index is greater than num loaded navcells!");
            }
            return new NavCell(((IntPtr)this.Native.int_1[1]) + ((int)(index * 0x20L)));
        }

        public NavCellLookup GetNavCellLookup(int index)
        {
            return (NavCellLookup)Globals.mem.ReadMemory(this.Native.intptr_2 + (index * 4), typeof(NavCellLookup));
       }

        public NavCellLookup GetNeighbourNavCellLookup(int index)
        {
            return (NavCellLookup)Globals.mem.ReadMemory(this.Native.intptr_0 + (index * 4), typeof(NavCellLookup));
       }

        public override string ToString()
        {
            return this.Native.ToString();
        }

        public int NavCellCount
        {
            get
            {
                return this.Native.int_0;
            }
        }

        public NavCell[] NavCells
        {
            get
            {
                int navCellCount = this.NavCellCount;
                NavCell[] cellArray = new NavCell[navCellCount];
                for (int i = 0; i < navCellCount; i++)
                {
                    IntPtr ptr = ((IntPtr)this.Native.int_1[1]) + (i * 0x20);
                    cellArray[i] = new NavCell(ptr);
                }
                return cellArray;
            }
        }

        public int NavGridSquareCountX
        {
            get
            {
                return this.Native.int_12;
            }
        }

        public int NavGridSquareCountY
        {
            get
            {
                return this.Native.int_13;
            }
        }

        public NavGridSquare[] NavGridSquares
        {
            get
            {
                IntPtr address = _baseAddress + this.Native.int_16[0];
                int elements = this.Native.int_16[1] / Marshal.SizeOf(typeof(NavGridSquare));
                return Globals.mem.ReadArray<NavGridSquare>(address, elements);
            }
        }

        public float Width
        {
            get
            {
                return this.Native.float_0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Size=0x90)]
        private struct Struct41
        {
            public int int_0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public int[] int_1;
            public int int_2;
            public int int_3;
            public int int_4;
            public int int_5;
            public int int_6;
            public IntPtr intptr_0;
            public int int_7;
            public int int_8;
            public int int_9;
            public float float_0;
            public int int_10;
            public int int_11;
            public int int_12;
            public int int_13;
            public int int_14;
            public IntPtr intptr_1;
            public int int_15;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public int[] int_16;
            public int int_17;
            public int int_18;
            public IntPtr intptr_2;
            public int int_19;
            public int int_20;
            public int int_21;
            public int int_22;
            public int int_23;
            public int int_24;
            public int int_25;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public int[] int_26;
            public int int_27;
            public int int_28;
        }

    }

    public class SNORecordScene : SNORecord
    {
        private NativeSceneSNO nativeSceneSNO_0;
        private NativeNavMeshDef navMeshDef_0;
        private NavZoneDef navZoneDef_0;

        internal SNORecordScene(IntPtr ptr, SNOTable table)
            : base(ptr, table)
        {
        }

        public override void ReadRecord()
        {
            this.nativeSceneSNO_0 = (NativeSceneSNO)base.Memory.ReadMemory(base.BaseAddress, typeof(NativeSceneSNO));
        }

//         public override string ToString()
//         {
//             return string.Format("ApperanceSNO:{0} PhysMeshSNO:{1} AABBBounds:{2} MarkerSetBounds:{3} NavMeshDef:{4} NavZone:{5}", new object[] { this.ApperanceSNO, this.PhysMeshSNO, this.AABBBounds, this.MarkerSetBounds, this.NavMeshDef, this.NavZone });
//         }

        public AABB AABBBounds
        {
            get
            {
                return this.Native.AABBBounds;
            }
        }

        public int ApperanceSNO
        {
            get
            {
                return this.Native.ApperanceSNO;
            }
        }

        public AABB MarkerSetBounds
        {
            get
            {
                return this.Native.MarkerSetBounds;
            }
        }

        public int[] MarkerSets
        {
            get
            {
                IntPtr address = (IntPtr)this.Native.serMarkerSets[0];
                if (address == IntPtr.Zero)
                {
                    return new int[0];
                }
                return base.Memory.ReadArray<int>(address, 14, false);
            }
        }

        private NativeSceneSNO Native
        {
            get
            {
                return this.nativeSceneSNO_0;
            }
        }

//         public Zeta.Internals.SNO.NavMeshDef NavMeshDef
//         {
//             get
//             {
//                 return (this.navMeshDef_0 ?? (this.navMeshDef_0 = new Zeta.Internals.SNO.NavMeshDef(base.BaseAddress + ((int)Marshal.OffsetOf(typeof(NativeSceneSNO), "NavMeshDef")))));
//             }
//         }

        public NavZoneDef NavZone
        {
            get
            {
                return (this.navZoneDef_0 ?? (this.navZoneDef_0 = new NavZoneDef(base.BaseAddress + ((int)Marshal.OffsetOf(typeof(NativeSceneSNO), "NavZoneDef")))));
            }
        }

        public int PhysMeshSNO
        {
            get
            {
                return this.Native.PhysMeshSNO;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeSceneSNO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] Header;
            public int dwordC;
            public AABB AABBBounds;
            public AABB MarkerSetBounds;
            public NativeNavMeshDef NavMeshDef;
            public int ApperanceSNO;
            public int PhysMeshSNO;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] serExclusions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public int[] Exclusions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] serInclusions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public int[] Inclusions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] serMarkerSets;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public int[] MarkerSets;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x48)]
            public byte[] pad228;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] serMsgTriggeredEvents;
            public IntPtr NavZoneDef;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] dword278;
            public int dword310;
        }
        [StructLayout(LayoutKind.Sequential, Size = 0x128)]
        public struct NativeNavMeshDef
        {
            public int GridHeight;
            public int GridWidth;
            public int dword8;
            public int NavMeshCellCount;
            public float float10;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] serNavMeshArraySquares;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public int[] unk;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
            public byte[] FileName;
        }
    }


    public class NavCell : NativeObject
    {
        internal NavCell(IntPtr ptr)
            : base(ptr)
        {
        }

        public override string ToString()
        {
            return string.Format("NativeNavCell: {0}", this.Native);
        }

        public NavCellFlags Flags
        {
            get
            {
                return (NavCellFlags)this.Native.ushort_0;
            }
        }

        public Vec3 Max
        {
            get
            {
                return this.Native.vector3_1;
            }
        }

        public Vec3 Min
        {
            get
            {
                return this.Native.vector3_0;
            }
        }

        private Struct0 Native
        {
            get
            {
                return (Struct0)Globals.mem.ReadMemory(BaseAddress, typeof(Struct0));
            }
        }

        public ushort NeighbourCount
        {
            get
            {
                return this.Native.ushort_1;
            }
        }

        public int NeighboursIndex
        {
            get
            {
                return this.Native.int_0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct0
        {
            public Vec3 vector3_0;
            public Vec3 vector3_1;
            public ushort ushort_0;
            public ushort ushort_1;
            public int int_0;
            public AABB Bounds
            {
                get
                {
                    return new AABB { Max = this.vector3_1, Min = this.vector3_0 };
                }
            }
            public override string ToString()
            {
                return string.Format("Min:{0} Max:{1} Flags:{2} NeighbourCount:{3} NeighboursIndex:{4} Bounds:{5}", new object[] { this.vector3_0, this.vector3_1, (NavCellFlags)this.ushort_0, this.ushort_1, this.int_0, this.Bounds });
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct NavCellLookup
    {
        public short Flags;
        public short NavCellIndex;
    }
    [StructLayout(LayoutKind.Sequential, Size = 6)]
    public struct NavGridSquare
    {
        public short Flags;
        public short word2;
        public short CellLookupIndex;
        public override string ToString()
        {
            return string.Format("Flags: {0}, Word2: {1}, CellLookupIndex: {2}", this.Flags, this.word2, this.CellLookupIndex);
        }
    }

}
