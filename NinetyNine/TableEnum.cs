using System.ComponentModel;

namespace NinetyNine
{
    enum Table
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

    class TableEnum
    {
        internal static string GetDescription(Table value)
        {
            return EnumManager.GetDescription(value);
        }
    }
}