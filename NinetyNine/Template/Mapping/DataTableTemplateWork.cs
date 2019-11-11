using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum WorkTitle
    {
        [Description("빅테이블: 작업")]
        BigTable_WorkName,

        [Description("빅테이블: 규격")]
        BigTable_WorkStandard,

        [Description("내역서: 품명")]
        Statement_Name,

        [Description("내역서: 규격")]
        Statement_Standard,

        [Description("공정표: Description")]
        Schedule_Description,
    }

    internal class DataTableTemplateWork : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(WorkTitle));
            SetTitleDescriptions(row, values);
        }
    }
}