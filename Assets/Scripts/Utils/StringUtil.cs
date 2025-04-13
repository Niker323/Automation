using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public static class StringUtil
    {
        public static bool FastStartsWith(string value, string reference)
        {
            if (reference.Length > value.Length)
            {
                return false;
            }

            for (int i = 0; i < reference.Length; i++)
            {
                if (value[i] != reference[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static int CountChars(this string text, char c)
        {
            int num = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == c)
                {
                    num++;
                }
            }

            return num;
        }
    }
}
