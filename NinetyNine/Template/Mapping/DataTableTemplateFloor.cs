using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum FloorTitle
    {
        [Description("빅테이블: 층")]
        BigTable_Floor,

        [Description("공정표: 구분")]
        Schedule_Floor,
    }

    class DataTableTemplateFloor : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(FloorTitle));
            SetTitleDescriptions(row, values);
        }
    }
}