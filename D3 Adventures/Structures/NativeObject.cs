using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Memory_Handling;
using System.Runtime.InteropServices;

namespace D3_Adventures.Structures
{
    public abstract class NativeObject
    {
        private IntPtr mPntr;

        protected NativeObject(IntPtr ptr)
        {
            this.mPntr = ptr;
        }

        protected NativeObject(uint ptr)
        {
            this.mPntr = (IntPtr)ptr;
        }

        protected IntPtr GetVFunc(IntPtr vtable, int index)
        {
            return this.Memory.GetPtrFromVTable(vtable, index);
        }

        public static implicit operator IntPtr(NativeObject @object)
        {
            return @object.BaseAddress;
        }

        public T[] ReadSerializedData<T>(int offset, int size) where T : struct
        {
            if ((offset != 0) && (size != 0))
            {
                Type t = typeof(T);
                int num = Marshal.SizeOf(t);
                int elements = size / num;
                return this.Memory.ReadArray<T>(this.BaseAddress + offset, elements);
            }
            return new T[0];
        }

        public void UpdatePointer(IntPtr ptr)
        {
            this.mPntr = ptr;
        }

        public IntPtr BaseAddress
        {
            get
            {
                return this.mPntr;
            }
        }

        public bool IsValid
        {
            get
            {
                return (this.BaseAddress != IntPtr.Zero);
            }
        }

        protected MemoryManager Memory
        {
            get
            {
                return Globals.mem;
            }
        }
    }
}
