using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum BigTableWhatTitle
    {
        [Description("빅테이블: 구조체")]
        BigTable_Structure,

        [Description("빅테이블: 구조체구분")]
        BigTable_StructureSeparation,

        [Description("빅테이블: 층")]
        BigTable_Floor,
    }

    class DataTableTemplateWhat : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(BigTableWhatTitle));
            SetTitleDescriptions(row, values);
        }
    }
}