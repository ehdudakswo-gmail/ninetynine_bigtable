using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NinetyNine
{
    enum BigTableTitle
    {
        [Description("프로젝트")]
        WHERE1,

        [Description("동")]
        WHERE2,

        [Description("타입")]
        WHERE3,

        [Description("층")]
        WHERE4,

        [Description("구조체")]
        WHAT1,

        [Description("구조체세부")]
        WHAT2,

        [Description("부재명")]
        WHAT3,

        [Description("부재세부명")]
        WHAT4,

        [Description("년")]
        WHEN1,

        [Description("분기")]
        WHEN2,

        [Description("월(Month)")]
        WHEN3,

        [Description("주(Week)")]
        WHEN4,

        [Description("시작일(EST)")]
        WHEN5,

        [Description("종료일(EFT)")]
        WHEN6,

        [Description("대공종")]
        HOW1,

        [Description("중공종")]
        HOW2,

        [Description("세공종")]
        HOW3,

        [Description("작업")]
        HOW4,

        [Description("작업규격")]
        HOW5,

        [Description("하도급")]
        WHO1,

        [Description("하도급 세부")]
        WHO2,

        [Description("단위")]
        ATTRIBUTE1,

        [Description("재료비단가")]
        ATTRIBUTE2,

        [Description("노무비단가")]
        ATTRIBUTE3,

        [Description("경비단가")]
        ATTRIBUTE4,

        [Description("단가")]
        ATTRIBUTE5,

        [Description("산출식")]
        RESULT1,

        [Description("수량")]
        RESULT2,

        [Description("수량변환")]
        RESULT2_1,

        [Description("재료비")]
        RESULT3,

        [Description("노무비")]
        RESULT4,

        [Description("경비")]
        RESULT5,

        [Description("총금액")]
        RESULT6,
    }

    class BigTableTitleEnum
    {

        private static Array values = Enum.GetValues(typeof(BigTableTitle));

        internal static Array GetValues()
        {
            return values;
        }

        internal static string GetDescription(BigTableTitle value)
        {
            return EnumManager.GetDescription(value);
        }

        internal static int GetLength()
        {
            return EnumManager.GetLength(values);
        }

        internal static int GetIndex(BigTableTitle value)
        {
            return EnumManager.GetIndex(value);
        }
    }
}