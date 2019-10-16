using System;
using System.Data;

namespace NinetyNine.BigTable
{
    internal class BigTableError
    {
        private static readonly string TITLE = "빅테이블 생성 에러";
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

            string position = string.Format("{0} SHEET ({1})", tableName, cell);
            string message = TITLE + NEWLINE + position + NEWLINE + error;

            throw new Exception(message);
        }
    }
}