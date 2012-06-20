using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Numerics;
using System.IO;
using D3_Adventures.Internals;
using D3_Adventures.Injector;

namespace D3_Adventures.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NUIElement
    {
        public IntPtr eventHandlersPtr;
        public int int_0;
        public int int_1;
        public int int_2;
        public int int_3;
        public int int_4;
        public int int_5;
        public int int_6;
        public int int_7;
        public int int_8;
        public int visible; 
        public int int_10; // 0x28
        public ulong hash; // 0x30
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] name; //130
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x109)]
        public IntPtr[] byte_1; // 0x554
        public IntPtr intptr_1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 370)]
        public int[] int_11;
        public uint pntrText;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4e)]
        public int[] int_12;
        public int int_13;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NUIElementHandlers
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xD4)]
        private IntPtr[] Handlers;

        public IntPtr GetMouseEnter()
        {
            return Handlers[0x44 / 4];
        }
        public IntPtr GetMouseLeave()
        {
            return Handlers[0x48 / 4];
        }
        public IntPtr GetClick()
        {
            return Handlers[0x20];
        }
    }
    public class UIElement
    {
        public NUIElementHandlers Handlers
        {
            get
            {
                return (NUIElementHandlers)Globals.mem.ReadMemory(InternalStruct.eventHandlersPtr, typeof(NUIElementHandlers));
            }
        }
        public ulong Hash { get { return InternalStruct.hash; } }
        public uint BaseAdress { get; set; }
        public string Name { get { return InternalStruct.name.ToUTF8String(); } }
        public NUIElement InternalStruct { get; set; }
        private Label mText;
        public Label Text
        {
            get
            {
                if (InternalStruct.pntrText == 0)
                    return null;
                if (mText == null)
                    mText = new Label(InternalStruct.pntrText);
                return mText;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return ((this.InternalStruct.int_13 & 1) != 0);
            }
        }
        public bool IsVisible { get { return InternalStruct.visible == 1; } }

        internal UIElement(uint baseAdress)
        {
            BaseAdress = baseAdress;
            InternalStruct = (NUIElement)Globals.mem.ReadMemory(baseAdress, typeof(NUIElement));
        }
        public void Click()
        {
            Globals.mem.Injector.CallFunction((uint)this.InternalStruct.byte_1[261], CallingConvention.ThisCall, BaseAdress); 
        }
        public void MouseEnter()
        {
               Globals.mem.Injector.CallFunction((uint)this.Handlers.GetMouseEnter(),CallingConvention.ThisCall,BaseAdress); 
        }
        public void MouseOut()
        {
            Globals.mem.Injector.CallFunction((uint)this.Handlers.GetMouseLeave(), CallingConvention.ThisCall, BaseAdress); 

        }
        static public UIElement GetByHash(ulong hash)
        {            
            uint uint1 = (uint)(hash >> 32);
            uint uint2 = (uint)hash;
            
            uint tmpCalc =  uint1 ^ uint2;
            uint pntr = Globals.mem.ReadMemoryAsUint((Globals.mem.ReadMemoryAsUint(Globals.mem.ReadMemoryAsUint(Offsets.objectManager) + 0x924)));
            tmpCalc &= Globals.mem.ReadMemoryAsUint(pntr + 0x40);
            uint pntFinal = Globals.mem.ReadMemoryAsUint(Offsets.uielements + tmpCalc * 4);
            while (Globals.mem.ReadMemoryAsUint(pntFinal + 0x08) != uint2 && Globals.mem.ReadMemoryAsUint(pntFinal + 0x0c) != uint1)
            {
                if (pntFinal == 0)
                    throw new Exception("UIElement not found");
                else
                    pntFinal = Globals.mem.ReadMemoryAsUint(pntFinal);
            }
            uint nPnt = Globals.mem.ReadMemoryAsUint(pntFinal + 0x210);
            return new UIElement(nPnt);
        }
        static public UIElement GetByName(string name)
        {
            return GetByHash(UIHelper.GetHashForString(name));
        }
        static public List<UIElement> GetAll()
        {
            List<UIElement> elements = new List<UIElement>();
            List<UIElement> Elems = new List<UIElement>();
            uint counter = 0;
            uint uielemePointer = 0;
            while (counter < Offsets.UIelementCount)
            {
                try
                {
                    counter++;
                    uielemePointer = Globals.mem.ReadMemoryAsUint((Offsets.uielements + counter * 4));
                    while (uielemePointer != 0)
                    {
                        uint nPnt = Globals.mem.ReadMemoryAsUint(uielemePointer + 0x210);
                        UIElement mElem = new UIElement(nPnt);
                        elements.Add(mElem);

                        uielemePointer = Globals.mem.ReadMemoryAsUint(uielemePointer);
                        //elements.Add(mElem);
                    }

                }
                catch 
                { 
                
                }
            }
            return elements;
        }
    }

    static public class UIHelper
    {
        static private BigInteger hashseed = new BigInteger(1099511628211);
        static private BigInteger offset = new BigInteger(14695981039346656037);
        static private BigInteger mod = power(new BigInteger(2),64);

        private static BigInteger power(BigInteger number, int exponent)
        {
            if (exponent == 0)
                return new BigInteger(1);
            if (exponent == 1)
                return number;
            if (exponent % 2 == 0)
                return square(power(number, exponent / 2));
            else
                return number * square(power(number, (exponent - 1) / 2));
        }

        private static BigInteger square(BigInteger num)
        {
            return num * num;
        }
        static public ulong GetHashForString(string value)
        {

            ulong hash = 0xCBF29CE484222325;
            for (int i = 0; i < value.Length; i++)
            {
                hash ^= (uint)value[i];
                hash = hash * 0x100000001B3;
            hash = hash & 0xFFFFFFFFFFFFFFFF;
            }
            

        
            return hash;
        }

        private static bool IsPowerOfTwo(int number)
        {
            return (number & (number - 1)) == 0;
        }

    }

}
