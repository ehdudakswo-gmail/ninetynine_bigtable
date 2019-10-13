using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NinetyNine
{
    enum MainTabPage
    {
        [Description("산출서")]
        Form,

        [Description("내역서")]
        Statement,

        [Description("공정표")]
        Schedule,

        [Description("조직도")]
        Organization,

        [Description("빅테이블")]
        BigTable
    }

    class MainTabPageEnum
    {
        private static Array values = Enum.GetValues(typeof(MainTabPage));

        internal static Array GetValues()
        {
            return values;
        }

        internal static string GetDescription(MainTabPage value)
        {
            return EnumManager.GetDescription(value);
        }

        internal static List<string> GetAllDescriptions()
        {
            return EnumManager.GetAllDescriptions(values);
        }
    }
}