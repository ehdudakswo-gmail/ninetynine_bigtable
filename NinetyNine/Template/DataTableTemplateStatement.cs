using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateStatement : DataTableTemplate
    {
        public static readonly string[] DATA_COLUMNS = new string[]
        {"품명", "규격","단위","수량","재료비단가","노무비단가","경비단가","계",};

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
            for (int i = 0; i < DATA_COLUMNS.Length; i++)
            {
                row[i] = DATA_COLUMNS[i];
            }
        }
    }
}