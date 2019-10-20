using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum StatementTitle
    {
        [Description("품명")]
        Name,

        [Description("규격")]
        Standard,

        [Description("단위")]
        Unit,

        [Description("수량")]
        Quantity,

        [Description("재료비단가")]
        MaterialCost,

        [Description("노무비단가")]
        LaborCost,

        [Description("경비단가")]
        Expenses,

        [Description("계")]
        Total,
    }


    internal class DataTableTemplateStatement : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(StatementTitle));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }
    }
}