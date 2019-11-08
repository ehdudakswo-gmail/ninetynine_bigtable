using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum BigTableTitle
    {
        [Description("프로젝트")]
        WHERE1,

        [Description("층")]
        WHERE2,

        [Description("평형")]
        WHERE3,

        [Description("실")]
        WHERE4,

        [Description("구조체")]
        WHAT1,

        [Description("구조체구분")]
        WHAT2,

        [Description("부재")]
        WHAT3,

        [Description("부재명")]
        WHAT4,

        [Description("대공종")]
        HOW1,

        [Description("중공종")]
        HOW2,

        [Description("세공종")]
        HOW3,

        [Description("작업")]
        HOW4,

        [Description("규격")]
        HOW5,

        [Description("년")]
        WHEN1,

        [Description("분기")]
        WHEN2,

        [Description("월")]
        WHEN3,

        [Description("주")]
        WHEN4,

        [Description("Plan EST")]
        WHEN5,

        [Description("Plan EFT")]
        WHEN6,

        [Description("Actual EST")]
        WHEN7,

        [Description("Actual EFT")]
        WHEN8,

        [Description("도급")]
        WHO1,

        [Description("작업조")]
        WHO2,

        [Description("관리회사")]
        WHO3,

        [Description("관리조직")]
        WHO4,

        [Description("관리자")]
        WHO5,

        [Description("비고구분")]
        WHY1,

        [Description("단위")]
        ATTRIBUTE1,

        [Description("재료비단가")]
        ATTRIBUTE2,

        [Description("노무비단가")]
        ATTRIBUTE3,

        [Description("경비단가")]
        ATTRIBUTE4,

        [Description("총단가")]
        ATTRIBUTE5,

        [Description("산출식")]
        RESULT1,

        [Description("수량")]
        RESULT2,

        [Description("수량변환")]
        RESULT3,

        [Description("재료비")]
        RESULT4,

        [Description("노무비")]
        RESULT5,

        [Description("경비")]
        RESULT6,

        [Description("총금액")]
        RESULT7,
    }

    internal class DataTableTemplateBigTable : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            DataRow row0 = dataTable.NewRow();
            SetRow0(row0);
            rows.Add(row0);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            Array values = Enum.GetValues(typeof(BigTableTitle));
            SetTitleTexts(row, values);
        }
    }
}