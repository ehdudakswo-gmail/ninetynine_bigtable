using System;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    abstract class BigTableMapper : BigTableFunction
    {
        protected const int CONTENT_ROWIDX = 1;
        protected DataTable bigTable;
        protected DataRowCollection bigTableRows;
        protected int bigTableRowsCount;

        abstract internal void Mapping();

        protected BigTableMapper(DataTable bigTable)
        {
            this.bigTable = bigTable;
            this.bigTableRows = bigTable.Rows;
            this.bigTableRowsCount = bigTableRows.Count;
        }

        protected void Mapping(DataRow bigTableRow, Enum bigTableTitle, DataRow mappingRow, Enum mappingTitle)
        {
            int bigTableColIdx = GetColumnIdx(bigTableTitle);
            int mappingColIdx = GetColumnIdx(mappingTitle);

            bigTableRow[bigTableColIdx] = mappingRow[mappingColIdx];
        }

        protected void Mapping(DataRow row, Enum title, string str)
        {
            int colIdx = GetColumnIdx(title);
            row[colIdx] = str;
        }
    }
}