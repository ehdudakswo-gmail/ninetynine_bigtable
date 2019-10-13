using System.Data;

namespace NinetyNine.DataTableTemplate
{
    internal class DataTableTemplateSchedule : DataTableTemplate
    {
        public override DataTable Create(string tableName)
        {
            DataTable dataTable = excelDataManager.GetBasicDataTable(tableName);
            DataRow row0 = dataTable.NewRow();

            var rows = dataTable.Rows;
            rows.Add(row0);

            SetRow0(row0);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            row[0] = "공정표";
        }
    }
}