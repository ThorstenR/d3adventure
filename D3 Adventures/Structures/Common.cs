using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace D3_Adventures.Structures
{
    public struct Vec2
    {
        public float x;    // 0x000 
        public float y;    // 0x004 
        public override string ToString()
        {
            return "X: " + x + ", Y: " + y;
        }
    };

    public struct Vec3
    {
        public float x;    // 0x000 
        public float y;    // 0x004 
        public float z;    // 0x008 
        public override string ToString()
        {
            return "X: " + x + ", Y: " + y + ", Z: " + z;
        }
    };

    public struct Vec4
    {
        public float x;    // 0x000 
        public float y;    // 0x004 
        public float z;    // 0x008 
        public float w;    // 0x00C 
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct AABB
    {
        public Vec3 Min;
        public Vec3 Max;
        public override string ToString()
        {
            return string.Format("Min:{0} Max:{1}", this.Min, this.Max);
        }
    }
}
