using System;
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

        public static readonly string[] countSuffixes = new string[] { "", " K", " M", " B", " T", " Q", " Qt", " S" };
        public static string NumberFormat(long money)
        {
            float ost = 0;
            int sufID = 0;
            while (money >= 1000)
            {
                ost = money % 1000;
                money = money / 1000;
                sufID++;
            }
            return (money + (ost / 1000)).ToString("F2") + countSuffixes[sufID];
        }
    }
}
