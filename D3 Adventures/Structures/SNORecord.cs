
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace D3_Adventures.Structures
{
    public abstract class SNORecord : NativeObject, IDisposable
    {
        protected readonly SNOTable _table;


        internal SNORecord(IntPtr ptr, SNOTable table)
            : base(ptr)
        {
            this._table = table;
            this.ReadRecord();
            table.method_2(ptr);
        }

        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                if (!this._table.IsDisposing && this._table.dictionary_0.ContainsKey((int)base.BaseAddress))
                {
                    this._table.dictionary_0.Remove((int)base.BaseAddress);
                }
                this.IsDisposed = true;
            }
        }

        ~SNORecord()
        {
            this.Dispose();
        }

        public T GetField<T>(int index) where T : struct
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(base.Memory.ReadMemoryAsString((uint)base.Memory.ReadMemory<IntPtr>((IntPtr)(index * 4)), 0x200, Encoding.UTF8), typeof(T));
                }
                return base.Memory.ReadMemory<T>(base.BaseAddress + (index * 4));
            }
            catch
            {
                return default(T);
            }
        }

        public abstract void ReadRecord();
        public new T[] ReadSerializedData<T>(int offset, int size) where T : struct
        {
            if ((offset != 0) && (size != 0))
            {
                Type t = typeof(T);
                int num = Marshal.SizeOf(t);
                int elements = size / num;
                return base.Memory.ReadArray<T>(base.BaseAddress + offset, elements, false);
            }
            return new T[0];
        }

        public bool IsDisposed
        {
            get;
            set;
        }

        public int SNOId
        {
            get
            {
                return this.GetField<int>(0);
            }
        }
    }
}

