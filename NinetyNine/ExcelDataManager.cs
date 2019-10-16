using System.Data;
using OfficeOpenXml;

namespace NinetyNine
{
    internal class ExcelDataManager
    {
        private readonly int COLUMN_BASIC_COUNT = 26 * 2;
        private readonly int COLUMN_CREATE_OFFSET = 26;

        private static ExcelDataManager instance;

        private ExcelDataManager() { }

        internal static ExcelDataManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ExcelDataManager();
            }

            return instance;
        }

        internal DataTable GetBasicDataTable(string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            string[] columns = GetColumnHeaderNames(COLUMN_BASIC_COUNT);

            for (int i = 0; i < columns.Length; i++)
            {
                string name = columns[i];
                dataTable.Columns.Add(name);
            }

            return dataTable;
        }

        private string[] GetColumnHeaderNames(int cnt)
        {
            string[] columns = new string[cnt];

            for (int i = 0; i < columns.Length; i++)
            {
                int roopCnt = (i / COLUMN_CREATE_OFFSET);
                int seq = (i % COLUMN_CREATE_OFFSET);
                string name = ((char)('A' + seq)).ToString();

                if (roopCnt == 0)
                {
                    columns[i] = name;
                }
                else
                {
                    string front = ((char)('A' + roopCnt - 1)).ToString();
                    columns[i] = front + name;
                }
            }

            return columns;
        }

        internal DataTable GetDataTable(ExcelWorksheet worksheet)
        {
            string tableName = worksheet.Name;
            DataTable dataTable = new DataTable(tableName);

            SetDataTable(dataTable, worksheet);
            return dataTable;
        }

        private void SetDataTable(DataTable dataTable, ExcelWorksheet worksheet)
        {
            if (worksheet.Dimension != null)
            {
                int excelColumnCount = worksheet.Dimension.End.Column;
                int columnCount = (excelColumnCount < COLUMN_BASIC_COUNT ? COLUMN_BASIC_COUNT : excelColumnCount);
                SetColumnHeaderName(dataTable, columnCount);
                SetDataTableValue(dataTable, worksheet);
            }
            else
            {
                SetColumnHeaderName(dataTable, COLUMN_BASIC_COUNT);
            }
        }

        private void SetColumnHeaderName(DataTable dataTable, int columnCount)
        {
            string[] columns = GetColumnHeaderNames(COLUMN_BASIC_COUNT);

            for (int i = 0; i < columns.Length; i++)
            {
                string name = columns[i];
                dataTable.Columns.Add(name);
            }
        }

        private void SetDataTableValue(DataTable dataTable, ExcelWorksheet worksheet)
        {
            int rowCount = worksheet.Dimension.End.Row;
            int colCount = worksheet.Dimension.End.Column;

            for (int row = 1; row <= rowCount; row++)
            {
                DataRow newRow = dataTable.NewRow();
                for (int col = 1; col <= colCount; col++)
                {
                    var excelValue = worksheet.Cells[row, col].Value;
                    if (excelValue == null)
                    {
                        continue;
                    }

                    int idx = col - 1;
                    newRow[idx] = excelValue.ToString();
                }

                dataTable.Rows.Add(newRow);
            }
        }

        internal string GetColumnName(int idx)
        {
            string[] columns = GetColumnHeaderNames(COLUMN_BASIC_COUNT);
            return columns[idx];
        }
    }
}