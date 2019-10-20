using System;
using System.Data;

namespace NinetyNine.BigTable
{
    internal class BigTableErrorData
    {
        internal DataTable dataTable { get; set; }
        internal BigTableErrorCell[] cells { get; set; }
        internal string error { get; set; }
    }

    internal class BigTableErrorCell
    {
        internal int rowIdx { get; set; }
        internal int colIdx { get; set; }
    }

    internal class BigTableError
    {
        private static readonly string SPACE = " ";
        private static readonly string NEWLINE = "\n";

        private ExcelDataManager excelDataManager = ExcelDataManager.GetInstance();
        private static BigTableError instance;

        private BigTableError() { }

        internal static BigTableError GetInstance()
        {
            if (instance == null)
            {
                instance = new BigTableError();
            }

            return instance;
        }

        internal void ThrowException(DataTable dataTable, int rowIdx, int colIdx, string error)
        {
            string tableName = dataTable.TableName;
            string row = (rowIdx + 1).ToString();
            string col = excelDataManager.GetColumnName(colIdx);
            string cell = col + row;

            string position = string.Format("{0} SHEET {1}", tableName, cell);
            string message = position + NEWLINE + error;

            throw new Exception(message);
        }

        internal void ThrowException(BigTableErrorData data)
        {
            DataTable dataTable = data.dataTable;
            BigTableErrorCell[] cells = data.cells;

            string tableName = dataTable.TableName;
            string cellInfo = GetCellInfo(dataTable, cells);
            string error = data.error;

            string message = tableName + NEWLINE + cellInfo + NEWLINE + error;
            throw new Exception(message);
        }

        private string GetCellInfo(DataTable dataTable, BigTableErrorCell[] cells)
        {
            int len = cells.Length;
            string[] columns = excelDataManager.GetBasicColumnHeaderNames();
            string[] cellArr = new string[len];

            for (int i = 0; i < len; i++)
            {
                BigTableErrorCell cell = cells[i];
                int rowIdx = cell.rowIdx;
                int colIdx = cell.colIdx;
                string value = dataTable.Rows[rowIdx][colIdx].ToString();

                string row = (rowIdx + 1).ToString();
                string col = columns[colIdx];
                string position = col + row;

                cellArr[i] = string.Format("{0}({1})", position, value);
            }

            string cellInfo = string.Join(SPACE, cellArr);
            return cellInfo;
        }
    }
}