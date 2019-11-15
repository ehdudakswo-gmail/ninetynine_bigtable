using System.Data;

namespace NinetyNine.Template
{
    enum StatementTitle
    {
        Note,
        Name,
        Standard,
        Unit,
        Quantity,
        MaterialCost,
        MaterialSum,
        LaborCost,
        LaborSum,
        ExpenseCost,
        ExpenseSum,
        Total,
    }

    internal class DataTableTemplateStatement : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
        }
    }
}