using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;

namespace D3_Adventures.Memory_Handling
{
    public unsafe static class StructureWrapper<T>
    {
        public static TypeCode TypeCode = Type.GetTypeCode(typeof(T));
        public static int Size;
        public static Type Type;
        public static bool HasMarshaledFields;
        internal static readonly StructureWrapper<T>.Delegate1 PinnedPointer;

        static StructureWrapper()
        {
            if (typeof(T) == typeof(bool))
            {
                StructureWrapper<T>.Size = 1;
                StructureWrapper<T>.Type = typeof(T);
            }
            else if (typeof(T).IsEnum)
            {
                Type enumUnderlyingType = typeof(T).GetEnumUnderlyingType();
                StructureWrapper<T>.Size = Marshal.SizeOf(enumUnderlyingType);
                StructureWrapper<T>.Type = enumUnderlyingType;
                StructureWrapper<T>.TypeCode = Type.GetTypeCode(enumUnderlyingType);
            }
            else
            {
                StructureWrapper<T>.Size = Marshal.SizeOf(typeof(T));
                StructureWrapper<T>.Type = typeof(T);
            }
            StructureWrapper<T>.HasMarshaledFields = Enumerable.Any<FieldInfo>((IEnumerable<FieldInfo>)StructureWrapper<T>.Type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<FieldInfo, bool>)(m => Enumerable.Any<object>((IEnumerable<object>)m.GetCustomAttributes(typeof(MarshalAsAttribute), true))));
            DynamicMethod dynamicMethod = new DynamicMethod(string.Format("GetPinnedPtr<{0}>", (object)typeof(T).FullName.Replace(".", "<>")), typeof(void*), new Type[1]
      {
        typeof (T).MakeByRefType()
      }, typeof(StructureWrapper<T>).Module);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Conv_U);
            ilGenerator.Emit(OpCodes.Ret);
            StructureWrapper<T>.PinnedPointer = (StructureWrapper<T>.Delegate1)dynamicMethod.CreateDelegate(typeof(StructureWrapper<T>.Delegate1));
        }

        internal delegate void* Delegate1(ref T value);
    }
}
