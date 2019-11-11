using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperWhat : BigTableMapper
    {
        internal DataTable bigTable { get; set; }
        internal Dictionary<string, DataRow> whatDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.WHERE2 };
            Enum[] whatKeys = new Enum[] { bigTableKeys[0] };

            var bigTableRows = bigTable.Rows;
            int bigTableRowsCount = bigTableRows.Count;

            for (int bigTableRowIdx = 0; bigTableRowIdx < bigTableRowsCount; bigTableRowIdx++)
            {
                if (bigTableRowIdx < templateRowsCount)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[bigTableRowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(bigTableRowIdx, bigTableKeys);

                    string whatKey = BigTableDictionary.GetKey(bigTableRow, whatKeys);
                    if (whatDictionary.ContainsKey(whatKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow whatRow = whatDictionary[whatKey];
                    Mapping(bigTableRow, BigTableTitle.WHAT1, whatRow, BigTableWhatTitle.BigTable_Structure);
                    Mapping(bigTableRow, BigTableTitle.WHAT2, whatRow, BigTableWhatTitle.BigTable_StructureSeparation);
                }
            }
        }
    }
}