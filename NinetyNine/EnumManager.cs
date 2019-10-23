using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace NinetyNine
{
    class EnumManager
    {
        internal static string GetDescription(Enum value)
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return value.ToString();
        }

        internal static List<string> GetAllDescriptions(Array values)
        {
            List<string> descriptions = new List<string>();

            foreach (Enum value in values)
            {
                string description = GetDescription(value);
                descriptions.Add(description);
            }

            return descriptions;
        }

        internal static int GetLength(Array values)
        {
            return values.Length;
        }

        internal static int GetIndex(Enum value)
        {
            return value.GetHashCode();
        }

        internal static int GetIndex(Array values, string description)
        {
            foreach (Enum value in values)
            {
                if (GetDescription(value) == description)
                {
                    return GetIndex(value);
                }
            }

            return -1;
        }

        internal static List<string> GetTexts(Array values)
        {
            List<string> texts = new List<string>();

            foreach (Enum value in values)
            {
                string text = GetText(value);
                texts.Add(text);
            }

            return texts;
        }

        internal static string GetText(Enum value)
        {
            string name = value.ToString();
            string description = GetDescription(value);

            return string.Format("{0}: {1}", name, description);
        }

        internal static Enum[] GetEnums(Array values)
        {
            List<Enum> list = new List<Enum>();
            foreach (Enum value in values)
            {
                list.Add(value);
            }

            int len = list.Count;
            Enum[] enums = new Enum[len];
            for (int i = 0; i < enums.Length; i++)
            {
                enums[i] = list[i];
            }

            return enums;
        }
    }
}