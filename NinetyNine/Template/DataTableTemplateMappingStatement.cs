using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum MappingStatementTitle
    {
        [Description("산출서-규격")]
        Form_Standard,

        [Description("내역서-품명")]
        Statement_Name,

        [Description("내역서-규격")]
        Statement_Standard,
    }

    internal class DataTableTemplateMappingStatement : DataTableTemplate
    {
        internal override void Set(DataTable dataTable)
        {
            DataRow row0 = dataTable.NewRow();

            var rows = dataTable.Rows;
            rows.Add(row0);

            SetRow0(row0);
        }

        private void SetRow0(DataRow row)
        {
            Array values = Enum.GetValues(typeof(MappingStatementTitle));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }
    }
}