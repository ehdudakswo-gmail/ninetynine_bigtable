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
    }
}