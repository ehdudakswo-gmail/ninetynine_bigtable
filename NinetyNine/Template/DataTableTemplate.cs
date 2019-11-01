using NinetyNine.BigTable;
using System.Data;

namespace NinetyNine.Template
{
    abstract class DataTableTemplate
    {
        protected ExcelDataManager ExcelDataManager = ExcelDataManager.GetInstance();

        abstract internal DataTable GetTemplateDataTable();

        internal void Refresh(DataTable dataTable)
        {
            dataTable.Clear();
            DataTable templateTable = GetTemplateDataTable();
            DataRowCollection dataRows = dataTable.Rows;
            int columnCount = dataTable.Columns.Count;

            foreach (DataRow templateRow in templateTable.Rows)
            {
                DataRow newRow = dataTable.NewRow();
                CopyValue(newRow, templateRow, columnCount);
                dataRows.Add(newRow);
            }
        }

        protected string GetTemplateTableName()
        {
            string className = GetType().Name;
            return className;
        }

        private void CopyValue(DataRow dataRow, DataRow templateRow, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                dataRow[i] = templateRow[i];
            }
        }
    }
}