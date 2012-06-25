using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Enumerations;
using System.Runtime.InteropServices;
using D3_Adventures.Injector;
using System.Reflection;
using System.Globalization;

namespace D3_Adventures.Structures
{
    public class SNOTable : NativeObject, IDisposable
    {
        internal SNO.ClientSNOTable clientSNOTable_0;
        internal readonly Dictionary<int, SNORecord> dictionary_0;

        internal SNOTable(IntPtr ptr, SNO.ClientSNOTable type)
            : base(ptr)
        {
            this.dictionary_0 = new Dictionary<int, SNORecord>();
            this.clientSNOTable_0 = type;
        }

        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.IsDisposing = true;
                using (new ASMExecutorMonitor(base.Memory.Injector))
                {
                    foreach (KeyValuePair<int, SNORecord> pair in this.dictionary_0)
                    {
                        pair.Value.Dispose();
                    }
                    this.dictionary_0.Clear();
                }
                this.IsDisposing = false;
                this.IsDisposed = true;
            }
        }

        ~SNOTable()
        {
            this.Dispose();
        }

        public T GetRecord<T>(int snoId) where T : SNORecord
        {
            using (new ASMExecutorMonitor(base.Memory.Injector))
            {
                IntPtr ptr = this.method_0(snoId);
                if (ptr != IntPtr.Zero)
                {
                    return (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { ptr, this }, CultureInfo.InvariantCulture);
                }
            }
            return default(T);
        }

        private IntPtr method_0(int snoId)
        {
            if (this.dictionary_0.ContainsKey(snoId))
            {
                return this.dictionary_0[snoId];
            }
            ASMExecutor executor = base.Memory.Injector;
            lock (executor.publicLock)
            {
                using (MemoryInjector class2 = new MemoryInjector(Globals.mem, 4))
                {
                    executor.Clear();
                    executor.AddLineSecured("push {0}", new object[] { 0 });
                    executor.AddLineSecured("push {0}", new object[] { snoId });
                    executor.AddLineSecured("mov edx, [{0}]", new object[] { base.BaseAddress });
                    executor.AddLineSecured("test edx, edx");
                    executor.AddLineSecured("jz @out");
                    executor.AddLineSecured("mov ecx, edx");
                    executor.AddLineSecured("call {0}", new object[] { Offsets.fnGetSnoInfoForSnoId });
                    executor.AddLineSecured("mov [{0}], eax", class2.Address);
                    executor.AddLineSecured("@out:");
                    executor.AddLineSecured("retn");
                    executor.ExecuteBuffer(null);

                    uint recAddress = base.Memory.ReadMemoryAsUint((uint)class2.Address);
                    return new IntPtr(recAddress);
                }
            }
        }

        private IntPtr method_1(int snoRecordId)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(snoRecordId);
                uint num = (uint)(bytes[3] ^ (0x1000193 * (bytes[2] ^ (0x1000193 * (bytes[1] ^ (0x1000193 * (bytes[0] ^ (0x1000193 * (snoRecordId ^ -2128831035)))))))));
                Struct15 native = this.Native;
                Struct25 struct3 = base.Memory.UnsafeReadToStruct<Struct25>(native.intptr_3);
                num &= struct3.uint_0;
                IntPtr address = base.Memory.UnsafeReadToStruct<IntPtr>(new IntPtr((uint)struct3.intptr_0 + (num * 4)));
                if (!(address != IntPtr.Zero))
                {
                    return IntPtr.Zero;
                }
                Struct24 struct4 = base.Memory.UnsafeReadToStruct<Struct24>(address);
                while (struct4.uint_0 != snoRecordId)
                {
                    if (struct4.intptr_0 == IntPtr.Zero)
                    {

                        return IntPtr.Zero;
                    }
                    struct4 = base.Memory.UnsafeReadToStruct<Struct24>(struct4.intptr_0);
                }
                IntPtr[] addresses = new IntPtr[] { base.BaseAddress + 60, (IntPtr)0x148, (IntPtr)(0x10 * struct4.uint_1) };
                IntPtr ptr2 = base.Memory.UnsafeReadPointersToStruct<IntPtr>(false, addresses);
                if (!(ptr2 != IntPtr.Zero))
                {
                    return IntPtr.Zero;
                }
                return ptr2;
            }
            catch
            {
            }
            return IntPtr.Zero;

        }

        internal void method_2(IntPtr recordPtr)
        {
            /*if (base.Memory.UnsafeReadToStruct<Struct23>(recordPtr).uint_0 != 0)
            {
                ASMExecutor executor = base.Memory.Injector;
                lock (executor.publicLock)
                {
                    using (MemoryInjector class3 = new MemoryInjector(base.Memory,4))
                    {
                        class3.Write<IntPtr>(0, recordPtr);
                        executor.Clear();
                        executor.AddLineSecured("mov ecx, [{0}]", new object[] { base.BaseAddress });
                        executor.AddLineSecured("push {0}", new object[] { class3.Address });
                        executor.AddLineSecured("call {0}", new object[] { Offsets.struct16_0.intptr_458 });
                        executor.AddLineSecured("retn");
                        executor.ExecuteBuffer("PurgeSNORecord");
                    }
                }
            }*/
        }

        public void Purge(int maxInstances)
        {
            ASMExecutor executor = base.Memory.Injector;
            lock (executor.publicLock)
            {
                IntPtr vFunc = base.GetVFunc(base.Memory.UnsafeReadToStruct<IntPtr>((IntPtr)this), 8);
                executor.Clear();
                executor.AddLineSecured("push {0}", new object[] { maxInstances });
                executor.AddLineSecured("mov ecx, [{0}]", new object[] { (IntPtr)this });
                executor.AddLineSecured("call {0}", new object[] { vFunc });
                executor.AddLineSecured("retn");
                executor.ExecuteBuffer(null);
            }
        }

        private static uint smethod_0(IntPtr address, ref uint value)
        {
            uint num;
            value = num = value - 1;
            Globals.mem.WriteStructure<uint>(address, num);
            return value;
        }

        public int InstancesCount
        {
            get
            {
                return base.Memory.UnsafeReadToStruct<int>(this.Native.intptr_0 + 0x10c);
            }
        }

        public bool IsDisposed
        {
            get;
            set;
        }

        public bool IsDisposing { get; set; }

        public int MaxInstances
        {
            get
            {
                return base.Memory.UnsafeReadToStruct<int>(this.Native.intptr_0 + 0x100);
            }
        }

        private Struct15 Native
        {
            get
            {
                return base.Memory.UnsafeReadToStruct<Struct15>(Globals.mem.UnsafeReadToStruct<IntPtr>(base.BaseAddress));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct15
        {
            private readonly int int_0;
            private readonly int int_1;
            private readonly int int_2;
            private readonly int int_3;
            private readonly int int_4;
            private readonly int int_5;
            private readonly int int_6;
            private readonly int int_7;
            private readonly int int_8;
            private readonly int int_9;
            private readonly int int_10;
            private readonly int int_11;
            private readonly int int_12;
            private readonly int int_13;
            private readonly int int_14;
            public readonly IntPtr intptr_0;
            public readonly IntPtr intptr_1;
            public readonly int int_15;
            public readonly IntPtr intptr_2;
            private readonly int int_16;
            private readonly int int_17;
            private readonly int int_18;
            private readonly int int_19;
            private readonly int int_20;
            private readonly int int_21;
            private readonly int int_22;
            private readonly int int_23;
            private readonly int int_24;
            private readonly int int_25;
            private readonly int int_26;
            private readonly int int_27;
            private readonly int int_28;
            private readonly int int_29;
            private readonly int int_30;
            private readonly int int_31;
            private readonly int int_32;
            private readonly int int_33;
            private readonly int int_34;
            public readonly int int_35;
            public readonly IntPtr intptr_3;
            private readonly int int_36;
            private readonly int int_37;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct23
        {
            public readonly int int_0;
            public uint uint_0;
            public readonly uint uint_1;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct24
        {
            public readonly IntPtr intptr_0;
            public readonly uint uint_0;
            public readonly uint uint_1;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct25
        {
            private readonly int int_0;
            private readonly int int_1;
            public readonly IntPtr intptr_0;
            private readonly int int_2;
            private readonly int int_3;
            private readonly int int_4;
            private readonly int int_5;
            private readonly int int_6;
            private readonly int int_7;
            private readonly int int_8;
            private readonly int int_9;
            private readonly int int_10;
            private readonly int int_11;
            private readonly int int_12;
            private readonly int int_13;
            private readonly int int_14;
            public readonly uint uint_0;
        }
    }
}

