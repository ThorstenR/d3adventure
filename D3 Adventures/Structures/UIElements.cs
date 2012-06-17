using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace D3_Adventures.Structures
{

    [StructLayout(LayoutKind.Sequential,Pack = 4,CharSet=CharSet.Ansi, Size = 536)]
    unsafe public struct UIElement
    {
        public uint ParentElement;
        public uint UnknownElement1;
        public ulong ElementHash;
        public fixed byte ElementName[250];
        public fixed byte Unknown[270];

        public string GetName()
        {
            fixed (byte* p = ElementName)
            {
                int stopChar = 250;
                
                for (var i = 0; i < 250; i++)
                {
                    if (p[i] == 0)
                    {
                        stopChar = i;
                        break;
                    }
                }
                byte[] t = new byte[stopChar];
                Marshal.Copy((IntPtr)p, t, 0, stopChar);
                return System.Text.ASCIIEncoding.ASCII.GetString(t);
            }
        }
    }
}
