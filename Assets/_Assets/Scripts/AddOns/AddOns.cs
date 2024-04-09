using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoundKnights
{
    public static class AddOns
    {
        public static bool ToBool(this int val) => val == 1;

        public static Vector2 TOV2(this Vector3 v3) => new(v3.x, v3.z);
        public static Vector3 ToV3(this Vector2 v2) => new(v2.x, 0f, v2.y);

        public static float XzSqrMagnitude(this Vector3 v)
            => v.x * v.x + v.z * v.z;

        public static bool InRange(this Vector2 v, Vector2 range)
            => v.x <= range.x && v.x >= 0 && v.y <= range.y && v.y >= 0;

        public static float Range(this Vector2 v2, float f)
            => v2.x + (v2.y - v2.x) * f;

        public static int Range(this Vector2Int v2, float f)
            => v2.x + (int)((v2.y - v2.x) * Mathf.Clamp01(f));

        public static int ToMs(this float f) => (int)(f * 1000);

        public static float Random(this Vector2 v2) => UnityEngine.Random.Range(v2.x, v2.y);
        public static int Random(this Vector2Int v2) => UnityEngine.Random.Range(v2.x, v2.y);

        static StringBuilder _builder = new();

        public static int ConstantHash(this Transform trs)
        {
            if (_builder == null)
                _builder = new StringBuilder();

            _builder.Clear();

            int hierarchyCount = trs.hierarchyCount;
            Transform current = trs;
            for (int i = 0; i < hierarchyCount - 2; i++)
            {
                if (current == null)
                    break;

                _builder.Append(current.name);
                current = current.parent;
            }

            return _builder.ToString().GetHashCode();
        }

        public static T Random<T>(this List<T> ls) => ls[UnityEngine.Random.Range(0, ls.Count)];

        public static T Random<T>(this HashSet<T> hs)
            => hs.ElementAt(UnityEngine.Random.Range(0, hs.Count));

        public static float PseudoRandom(ref long seed)
        {
            seed = (1664525 * seed + 1013904223) % int.MaxValue;
            return (float)(seed / (double)int.MaxValue);
        }

        public static Vector3 Divide(Vector3 v1, Vector3 v2)
        {
            return new Vector3
            {
                x = v1.x / v2.x,
                y = v1.y / v2.y,
                z = v1.z / v2.z,
            };
        }

        #region Shorts
        
        static readonly string[] s_StaticShorthands = { "", "K", "M", "B", "T" };

        public static string ToShort(this ulong i_Value)
        {
            if (i_Value < 1000) return i_Value.ToString("F2").TrimEnd('0').TrimEnd('.');

            string integer = i_Value.ToString("F0");

            const int aCode = 'a';
            const int zCode = 'z';
            const int alphabetCount = zCode - aCode + 1;

            string absValue = integer;

            if (absValue[0] == '-') absValue = absValue[1..];
            
            int valueDigitCount = absValue.Length;
            int shorthandIndex = (valueDigitCount - 1) / 3;

            string shorthand;
            if (shorthandIndex < s_StaticShorthands.Length) shorthand = s_StaticShorthands[shorthandIndex];
            else
            {
                int shiftedShorthandIndex = shorthandIndex - s_StaticShorthands.Length;
                shorthand = "".PadLeft(shiftedShorthandIndex / alphabetCount + 1, 'a') +
                            Convert.ToChar(aCode + shiftedShorthandIndex % alphabetCount);
            }

            int intDigits = valueDigitCount - 3 * shorthandIndex;
            if (intDigits > valueDigitCount || intDigits < 0) intDigits = valueDigitCount;
            string intPart = absValue[..intDigits];

            int floatDigits = valueDigitCount - intDigits;
            string floatPart = "";
            if (floatDigits > 0)
            {
                if (floatDigits > 2) floatDigits = 2;

                floatPart = absValue.Substring(intDigits, floatDigits).TrimEnd('0');

                if (floatPart.Length > 0) floatPart = "." + floatPart;
            }

            return intPart + floatPart + shorthand;
        }
        
        #endregion
    }
}