using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum HowTitle
    {
        [Description("빅테이블: 대공종")]
        BigTable_WorkLarge,

        [Description("빅테이블: 중공종")]
        BigTable_WorkMedium,

        [Description("빅테이블: 세공종")]
        BigTable_WorkSmall,

        [Description("빅테이블: 작업")]
        BigTable_WorkName,
    }

    class DataTableTemplateHow : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(HowTitle));
            SetTitleDescriptions(row, values);
        }
    }
}
