using System;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    abstract class BigTableMapper
    {
        protected readonly string ERROR_DATA_NONE = "{0} 데이터 없음";

        protected BigTableError bigTableError = BigTableError.GetInstance();
        abstract internal void Mapping();

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
        }

        protected int GetRowIndex(DataTable dataTable, DataRow row)
        {
            int rowIdx = dataTable.Rows.IndexOf(row);
            return rowIdx;
        }

        protected void ThrowException(DataTable dataTable, BigTableErrorCell[] cells, string error)
        {
            BigTableErrorData errrorData = new BigTableErrorData
            {
                dataTable = dataTable,
                cells = cells,
                error = error,
            };

            bigTableError.ThrowException(errrorData);
        }

        protected BigTableErrorCell[] GetErrorCells(int rowIdx, Enum[] keys)
        {
            int len = keys.Length;
            BigTableErrorCell[] cells = new BigTableErrorCell[len];

            for (int i = 0; i < len; i++)
            {
                cells[i] = new BigTableErrorCell
                {
                    rowIdx = rowIdx,
                    colIdx = GetColumnIdx(keys[i]),
                };
            }

            return cells;
        }
    }
}