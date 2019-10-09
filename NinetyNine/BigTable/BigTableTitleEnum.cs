using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace NinetyNine
{
    enum BigTableTitle
    {
        [Description("WHERE 1: 프로젝트")]
        WHERE1,

        [Description("WHERE 2: 동")]
        WHERE2,

        [Description("WHERE 3: 타입")]
        WHERE3,

        [Description("WHERE 4: 층")]
        WHERE4,

        [Description("WHAT 1: 구조체")]
        WHAT1,

        [Description("WHAT 2: 부재명")]
        WHAT2,

        [Description("WHAT 3: 부재세부명")]
        WHAT3,

        [Description("WHEN 1: 년")]
        WHEN1,

        [Description("WHEN 2: 분기")]
        WHEN2,

        [Description("WHEN 3: 월(Month)")]
        WHEN3,

        [Description("WHEN 4: 주(Week)")]
        WHEN4,

        [Description("WHEN 5: EST")]
        WHEN5,

        [Description("WHEN 6: EFT")]
        WHEN6,

        [Description("HOW 1: 대공종")]
        HOW1,

        [Description("HOW 2: 중공종")]
        HOW2,

        [Description("HOW 3: 세공종")]
        HOW3,

        [Description("HOW 4: 작업")]
        HOW4,

        [Description("HOW 5: 작업규격")]
        HOW5,

        [Description("WHO 1: 하도급")]
        WHO1,

        [Description("WHO 2: 하도급 세부")]
        WHO2,

        [Description("Result 1: 산출식")]
        RESULT1,

        [Description("Result 2: 결과값")]
        RESULT2,

        [Description("Result 3: 재료비")]
        RESULT3,

        [Description("Result 4: 노무비")]
        RESULT4,

        [Description("Result 5: 경비")]
        RESULT5,

        [Description("Result 6: 단가")]
        RESULT6,

        [Description("Result 7: 금액")]
        RESULT7,
    }

    class BigTableTitleEnum
    {

        private Array values = Enum.GetValues(typeof(BigTableTitle));

        internal List<string> GetAllDescriptions()
        {
            List<string> descriptions = new List<string>();

            foreach (BigTableTitle value in values)
            {
                string description = GetDescription(value);
                descriptions.Add(description);
            }

            return descriptions;
        }

        private string GetDescription(BigTableTitle value)
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

        internal int GetLength()
        {
            return values.Length;
        }

        internal int GetIndex(BigTableTitle value)
        {
            return value.GetHashCode();
        }
    }
}