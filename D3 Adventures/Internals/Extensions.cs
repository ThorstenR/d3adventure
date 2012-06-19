using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3_Adventures.Internals
{
    public static class Extensions
    {
        public static string ToUTF8String(this byte[] arr)
        {
            return arr.ToUTF8String(0);
        }

        public static string ToUTF8String(this byte[] arr, int startIndex)
        {
            return arr.ToUTF8String(startIndex, (arr.Length - startIndex));
        }

        public static string ToUTF8String(this byte[] arr, int startIndex, int maxCount)
        {
            int index = startIndex;
            for (int i = 0; i < maxCount; i++)
            {
                if (arr[index] == 0)
                {
                    return Encoding.UTF8.GetString(arr, startIndex, i);
                }
                index++;
            }
            throw new Exception("String is not null terminated!");
        }
    }
}
