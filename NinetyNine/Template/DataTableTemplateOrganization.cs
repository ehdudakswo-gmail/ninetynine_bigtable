using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateOrganization : DataTableTemplate
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
            row[0] = "조직도";
        }
    }
}