using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum WhoTitle
    {
        [Description("빅테이블: 세공종")]
        BigTable_WorkSmall,

        [Description("조직도: WHO1")]
        Organization_WHO1,

        [Description("조직도: WHO2")]
        Organization_WHO2,

        [Description("조직도: WHO3")]
        Organization_WHO3,

        [Description("조직도: WHO4")]
        Organization_WHO4,

        [Description("조직도: WHO5")]
        Organization_WHO5,
    }

    class DataTableTemplateWho : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(WhoTitle));
            SetTitleDescriptions(row, values);
        }
    }
}