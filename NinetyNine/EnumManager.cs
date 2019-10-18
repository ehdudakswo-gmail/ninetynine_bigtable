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
    }
}