using System;
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

        [Description("비고")]
        Note,
    }

    internal class DataTableTemplateStatement : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(StatementTitle));
            SetTitleDescriptions(row, values);
        }
    }
}