using System;
using System.Collections.Generic;
using System.Text;

namespace MyDownloader.Core.Common
{
    public static class ArrayExtension
    {
        public static int indexOf(this byte[] haystack, byte[] needle)
        {
            //int ret = -1;
            // pos = index of needle 1
            // if pos is not -1
            // check next char
            //Object[] tmphay = (object[])haystack;
            //Object[] tmpneedle = (object[])needle;

            int pos = -1;
            bool found = false;
            pos = Array.IndexOf(haystack, needle[0]);
            if (needle.Length == 1)
                return pos;
            while (pos != -1 && !found)
            {
                if (pos != -1)
                {
                    for (int i = 1; i < needle.Length; i++)
                    {
                        if (haystack[pos + i] != needle[i])
                        {
                            found = false;
                            break;
                        }
                        found = true;
                    }
                }
                if (found)
                    break;
                pos = Array.IndexOf(haystack, needle[0], pos + 1);
            }
            return pos;
        }

        public static byte[] sub(this byte[] arr, int start, int end)
        {
            byte[] ret = new byte[end - start];
            for (int i = start; i < end; i++)
            {
                ret[i - start] = arr[i];
            }
            return ret;
        }
    }
}
