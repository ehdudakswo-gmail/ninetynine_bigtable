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

    internal class BigTableError : Exception
    {
        private static readonly string SPACE = " ";
        private static readonly string NEWLINE = "\n";

        private static BigTableError instance;
        private ExcelDataManager excelDataManager = ExcelDataManager.GetInstance();
        private string[] columns;

        private DataTable dataTable;
        private BigTableErrorCell[] cells;
        private string message;

        private BigTableError()
        {
            columns = excelDataManager.GetBasicColumnHeaderNames();
        }

        internal static BigTableError GetInstance()
        {
            if (instance == null)
            {
                instance = new BigTableError();
            }

            return instance;
        }

        internal void ThrowException(BigTableErrorData data)
        {
            dataTable = data.dataTable;
            cells = data.cells;
            message = CreateMessage(data);

            throw this;
        }

        private string CreateMessage(BigTableErrorData data)
        {
            string tableName = dataTable.TableName;
            string cellInfo = GetCellInfo(dataTable, cells);
            string error = data.error;
            string message = tableName + SPACE + cellInfo + NEWLINE + error;

            return message;
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

                string row = (rowIdx + 1).ToString();
                string col = columns[colIdx];
                string position = col + row;

                cellArr[i] = string.Format("{0}", position);
            }

            string cellInfo = string.Join(SPACE, cellArr);
            return cellInfo;
        }

        internal DataTable GetDataTable()
        {
            return dataTable;
        }

        internal BigTableErrorCell[] GetCells()
        {
            return cells;
        }

        internal string GetMessage()
        {
            return message;
        }
    }
}