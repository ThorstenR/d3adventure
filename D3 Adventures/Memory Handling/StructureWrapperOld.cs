using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace D3_Adventures.Memory_Handling
{
    internal static class StructureWrapperOld<T>
    {
        public static bool bool_0;
        internal static readonly Delegate1 delegate1_0;
        [CompilerGenerated]
        private static Func<FieldInfo, bool> func_0;
        public static int int_0;
        public static Type type_0;
        public static TypeCode typeCode_0;

        static unsafe StructureWrapperOld()
        {
            StructureWrapperOld<T>.typeCode_0 = Type.GetTypeCode(typeof(T));
            if (typeof(T) == typeof(bool))
            {
                StructureWrapperOld<T>.int_0 = 1;
                StructureWrapperOld<T>.type_0 = typeof(T);
            }
            else if (typeof(T).IsEnum)
            {
                Type enumUnderlyingType = typeof(T).GetEnumUnderlyingType();
                StructureWrapperOld<T>.int_0 = Marshal.SizeOf(enumUnderlyingType);
                StructureWrapperOld<T>.type_0 = enumUnderlyingType;
                StructureWrapperOld<T>.typeCode_0 = Type.GetTypeCode(enumUnderlyingType);
            }
            else
            {
                StructureWrapperOld<T>.int_0 = Marshal.SizeOf(typeof(T));
                StructureWrapperOld<T>.type_0 = typeof(T);
            }
            if (StructureWrapperOld<T>.func_0 == null)
            {
                StructureWrapperOld<T>.func_0 = new Func<FieldInfo, bool>(StructureWrapperOld<T>.smethod_0);
            }
            StructureWrapperOld<T>.bool_0 = StructureWrapperOld<T>.type_0.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Any<FieldInfo>(StructureWrapperOld<T>.func_0);
            DynamicMethod method = new DynamicMethod(string.Format("GetPinnedPtr<{0}>", typeof(T).FullName.Replace(".", "<>")), typeof(void*), new Type[] { typeof(T).MakeByRefType() }, typeof(StructureWrapperOld<>).Module);
            ILGenerator iLGenerator = method.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Conv_U);
            iLGenerator.Emit(OpCodes.Ret);
            StructureWrapperOld<T>.delegate1_0 = (Delegate1)method.CreateDelegate(typeof(Delegate1));
        }

        [CompilerGenerated]
        private static bool smethod_0(FieldInfo m)
        {
            return m.GetCustomAttributes(typeof(MarshalAsAttribute), true).Any<object>();
        }

        internal unsafe delegate void* Delegate1(ref T value);
    }
}
